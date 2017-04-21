import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';
import { DeselectCategoryComponent } from "../test-sections/deselect-category.component";
import { Question } from '../../questions/question.model';
import { TestDetails } from '../test-details';
import { Category } from '../../questions/category.model';
import { Test, TestCategory, TestQuestion } from '../tests.model';

@Component({
    moduleId: module.id,
    selector: 'test-sections',
    templateUrl: 'test-sections.html'
})

export class TestSectionsComponent implements OnInit {
    testDetails: Test;
    testId: number;
    Categories: Category[];   
    testCategories: TestCategory[];
    testCategoryObj: TestCategory;
    testDetailsObj: TestDetails;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
        this.testCategoryObj = new TestCategory();
        this.testCategories = new Array<TestCategory>();
        this.testDetailsObj = new TestDetails();
        this.getTestSectionsDetails();
        this.Categories = this.testDetailsObj.category;
        this.testDetails = new Test();
    }

    /**
    * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
    */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSectionsDetails();
        this.getTestById(this.testId);
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestById(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testDetails = (response);
        });
    }

    /**
     * To get all the details of a test with list of categories, which can be added in the test
     */
    getTestSectionsDetails() {
        this.testService.getTestDetails(this.test.id).subscribe((response) => {
            this.testDetailsObj = response;
            console.log(this.testDetailsObj);
        });
    }

    /**
       * To Select or Deselect a Category from list
       * @param category
       */
    onSelect(category: Category) {
        if (!category.isSelect)
            category.isSelect = true;
        else {
            this.testService.deselectCategory(category.id, this.testSettings.id).subscribe((IsQuestionAdded) => {
                if (IsQuestionAdded) {
                    this.testCategoryObj.testId = this.testId;
                    this.testCategoryObj.categoryId = category.id;
                    let dialogRef = this.dialog.open(DeselectCategoryComponent, { data: this.testCategoryObj });
                    dialogRef.afterClosed().subscribe(result => {
                        if (result)
                            category.isSelect = false;
                    });
                }
            });
        }
    }

    /**
     * To Save the Selected Categories and redirect user for question selection
     */
    SaveAndNext() {
        this.SaveCategory();
        this.router.navigateByUrl('/tests/' + this.testId + 'questions');
    }

    /**
     * To Save the Selected Categories for Question Selection and redirect the user to the test dashboard
     */
    SaveAndExit() {
        this.SaveCategory();
        this.router.navigateByUrl('/tests');
    }

    /**
     * To save the selected category as selected
     */
    SaveCategory() {
        let categories = this.testDetailsObj.categoryAcList.filter(function (x) {
            return (x.isSelect);
        });
        for (let category of categories) {
            this.testCategoryObj.categoryId = category.id;
            this.testCategoryObj.testId = this.testSettings.id;
            this.testCategories.push(this.testCategoryObj);
            this.testCategoryObj = new TestCategory()
        }
        this.testService.addSelectedCategories(this.testCategories).subscribe((response) => { },
            err => {
                this.snackbarRef.open('Ooops!, something went wrong', 'Dismiss');
            });
    }

    /**
     *To display error message in snackbar when any  error is caught from server
     */
    open(message: string) {
        let config = this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
    }
}

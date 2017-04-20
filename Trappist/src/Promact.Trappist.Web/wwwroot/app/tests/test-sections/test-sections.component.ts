import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar } from '@angular/material';
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
    test: Test;
    testCategories: TestCategory[];
    testCategoryObj: TestCategory;
    testQuestion: Array<TestQuestion>;
    deselectCategoryError: boolean;
    question: Question[] = new Array<Question>();
    testDetailsObj: TestDetails;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
        this.test = new Test();
        this.testCategoryObj = new TestCategory();
        this.testCategories = new Array<TestCategory>();
        this.testQuestion = [];
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
            this.testService.deselectCategory(category.id, this.test.id).subscribe((IsQuestionAdded) => {
                if (IsQuestionAdded) {
                    let dialogRef = this.dialog.open(DeselectCategoryComponent);
                }
            });
        }
    }

    /**
     * To Save the Selected Categories and redirect user for question selection
     */
    SaveNext() {
        this.SaveCategory();
        this.router.navigateByUrl('/tests/' + this.test.id + '/questions');
    }

    /**
     * Save the Selected Categories for Question Selection and redirect the user to the test dashboard
     */
    SaveExit() {
        this.SaveCategory();
        this.router.navigateByUrl('/tests');
    }

    SaveCategory() {
        let categories = this.testDetailsObj.category.filter(function (x) {
            return (x.isSelect);
        });
        for (let category of categories) {
            this.testCategoryObj.categoryId = category.id;
            this.testCategoryObj.testId = this.test.id;
            this.testCategories.push(this.testCategoryObj);
            this.testCategoryObj = new TestCategory()
        }
        this.testService.addSelectedCategories(this.testCategories).subscribe();
    }
}

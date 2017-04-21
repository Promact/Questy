import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar, MdSnackBarRef, SimpleSnackBar, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';
import { DeselectCategoryComponent } from '../test-sections/deselect-category.component';
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
    testCategories: TestCategory[] = [];
    testCategoryObj: TestCategory;
    testDetailsObj: TestDetails;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {       
        this.testCategoryObj = new TestCategory();
        this.testCategories = new Array<TestCategory>();
        //this.testDetailsObj = new TestDetails();
        //this.getTestSectionsDetails();
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

    /**
     * To get all the details of a test with list of categories, which can be added in the test
     */
    //getTestSectionsDetails() {
    //    this.testService.getTestDetails(this.testId).subscribe((response) => {
    //    },
    //        err => {
    //            this.openSnackbar('Something went wrong, try again');

    //        });
    //}

    /**
       * To Select or Deselect a Category from list
       * @param category
       */
    onSelect(category: Category) {
        if (!category.isSelect)
            category.isSelect = true;
        else {
            this.testService.deselectCategory(category.id, this.testDetails.id).subscribe((isQuestionAdded) => {
                if (isQuestionAdded) {
                    this.testCategoryObj.testId = this.testId;
                    this.testCategoryObj.categoryId = category.id;
                    let dialogRef = this.dialog.open(DeselectCategoryComponent, { data: this.testCategoryObj });
                    dialogRef.afterClosed().subscribe(result => {
                        if (result)
                            category.isSelect = false;
                    },
                        err => {
                            this.openSnackbar('Something went wrong, try again');
                        });
                }
                else {
                    category.isSelect = false;
                }
            });
        }
    }

    /**
     * To save the selected categories and move further
     */
    saveCategoryToExitOrMoveNext(isSelectButton: boolean) {
        console.log(isSelectButton);
        //let categories = this.testDetails.categoryAcList.filter(function (x) {
        //    return (x.isSelect);
        //});
        //for (let category of categories) {
        //    this.testCategoryObj.categoryId = category.id;
        //    this.testCategoryObj.testId = this.testDetails.id;
        //    this.testCategories.push(this.testCategoryObj);
        //    this.testCategoryObj = new TestCategory();
        //}
        this.testService.addSelectedCategories(this.testDetails.id, this.testDetails.categoryAcList ).subscribe((response) => {
            if (response) {
                if (isSelectButton)
                    this.router.navigate(['/tests/' + this.testId + '/questions']);
                else
                    this.router.navigate(['/tests']);
            }
        },
            err => {
                this.openSnackbar('Something went wrong, try again');
            });
    }

    /**
     *To display error message in snackbar when any  error is caught from server
     */
    openSnackbar(message: string) {
        return this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
    }
}

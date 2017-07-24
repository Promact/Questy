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
    moduleId: module.id.toString(),
    selector: 'test-sections',
    templateUrl: 'test-sections.html'
})

export class TestSectionsComponent implements OnInit {
    testDetails: Test;
    testId: number;
    testCategories: TestCategory[];
    testCategoryObj: TestCategory;
    testDetailsObj: TestDetails;
    loader: boolean;
    testNameReference: string;
    isEditTestEnabled: boolean;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testCategoryObj = new TestCategory();
        this.testCategories = new Array<TestCategory>();
        this.testCategories = [];
        this.testDetails = new Test();
    }

    /**
    * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
    */
    ngOnInit() {
        this.loader = true;
        this.testId = this.route.snapshot.params['id'];
        this.getTestById(this.testId);
        this.isTestAttendeeExist();
    }

    /**
     * Gets the Settings and details saved for a particular Test
     * @param id contains the value of the test Id from the route
     */
    getTestById(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testDetails = (response);
            this.testNameReference = this.testDetails.testName;
            this.loader = false;
        });
    }

    /**
       * To Select or Deselect a Category from list
       * @param category
       */
    onSelect(category: Category) {
        if (this.isEditTestEnabled) {
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
    }

    /**
     * To save the test categories and move further
     * @param isSelectButton whose value will indicate what to do next, if it is false it will save changes to the test and exit to the test dashboard. And if the isSelectButton is true, it will save changes and move to the question selection page.
     */
    saveCategoryToExitOrMoveNext(isSelectButton: boolean) {
        this.loader = true;
        if (this.isEditTestEnabled) {
            this.testService.addTestCategories(this.testDetails.id, this.testDetails.categoryAcList).subscribe((response) => {
                if (response) {
                    if (isSelectButton) {
                        this.loader = false;
                        this.router.navigate(['/tests/' + this.testId + '/questions']);
                    }
                    else {
                        this.loader = false;
                        this.router.navigate(['/tests']);
                    }
                }
            },
                err => {
                    this.loader = false;
                    this.openSnackbar('Something went wrong, try again');
                });
        }
        else if (isSelectButton)
            this.router.navigate(['/tests/' + this.testId + '/questions']);
        else
            this.router.navigate(['/tests']);
    }

    /**
     *To display message in snackbar whenever required
     */
    openSnackbar(message: string) {
        return this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
    }

    /**
     * Checks if any candidate has taken the test
     */
    isTestAttendeeExist() {
        this.testService.isTestAttendeeExist(this.testId).subscribe((res) => {
            if (res.response) {
                this.isEditTestEnabled = false;
            }
            else {
                this.isEditTestEnabled = true;
            }
        });
    }
}

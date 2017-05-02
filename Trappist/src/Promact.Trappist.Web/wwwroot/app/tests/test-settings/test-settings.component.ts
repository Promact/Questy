import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';
import { TestOrder } from '../enum-testorder';
import { Test } from '../tests.model';
import { BrowserTolerance } from '../enum-browsertolerance';
import { AllowTestResume } from '../enum-allowtestresume';
import { IncompleteTestCreationDialogComponent } from './incomplete-test-creation-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'test-settings',
    templateUrl: 'test-settings.html'
})

export class TestSettingsComponent implements OnInit {
    testDetails: Test;
    testId: number;
    validEndDate: boolean;
    endDate: string;
    validTime: boolean;
    validStartDate: boolean;
    currentDate: Date;
    testNameUpdatedMessage: string;
    testSettingsUpdatedMessage: string;
    testNameRef: string;
    isTestNameExist: boolean;
    QuestionOrder = TestOrder;
    OptionOrder = TestOrder;
    BrowserTolerance = BrowserTolerance;
    AllowTestResume = AllowTestResume;
    response: any;
    errorMessage: string;
    testNameReference: string;
    isSectionOrQuestionAdded: boolean;
    loader: boolean;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testDetails = new Test();
        this.validEndDate = false;
        this.validTime = false;
        this.validStartDate = false;
        this.currentDate = new Date();
        this.testSettingsUpdatedMessage = 'The settings of the Test has been updated successfully';

    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.loader = true;
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
            this.testNameReference = this.testDetails.testName;
            this.loader = false;
        });
    }

    /**
     * Open snackbar
     * @param message contains the message to be displayed when the snackbar gets opened
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
    }

    /**
     * Checks the End Date and Time is valid or not
     * @param endDate contains ths the value of the field End Date and Time
     */
    isEndDateValid(endDate: Date) {
        if (new Date(this.testDetails.startDate) >= new Date(endDate)) {
            this.validEndDate = true;
            this.validStartDate = false;
        }
        else
            this.validEndDate = false;
    }

    /**
     * Checks whether the Start Date selected is valid or not
     */
    isStartDateValid() {
        this.validStartDate = new Date(this.testDetails.startDate) < this.currentDate ? true : false;
        this.validEndDate = new Date(this.testDetails.startDate) >= new Date(this.testDetails.endDate) ? true : false;
    }

    /**
     * Checks whether the Warning Time set is valid
     */
    isWarningTimeValid() {
        this.validTime = +this.testDetails.warningTime >= +this.testDetails.duration ? true : false;
    }

    /**
    *  Updates the settings edited for the selected Test and redirects to the test dashboard after the settings of the selected Test has been successfully updated
    * @param id contains the value of the Id from the route
    * @param testObject is an object of the class Test
    */
    saveTestSettings(id: number, testObject: Test) {
        this.testService.updateTestById(id, testObject).subscribe((response) => {
            this.loader = true;
            let snackBarRef = this.snackbarRef.open('Saved changes successfully', 'Dismiss', {
                duration: 3000,
            });
            snackBarRef.afterDismissed().subscribe(() => {
                this.router.navigate(['/tests']);
                this.loader = false;
            });
        },
            errorHandling => {
                this.loader = false;
                this.response = errorHandling.json();
                this.errorMessage = this.response['error'];
                this.snackbarRef.open(this.errorMessage, 'Dismiss', {
                    duration: 3000,
                });
            },
        );
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        let isCategoryAdded = this.testDetails.categoryAcList.some(function (x) {
            return x.isSelect;
        });
        if (isCategoryAdded) {
            let isQuestionAdded = this.testDetails.categoryAcList.some(function (x) {
                return (x.numberOfSelectedQuestion !== 0);
            });
            if (isQuestionAdded) {
                this.testService.updateTestById(id, testObject).subscribe((response) => {
                    this.openSnackBar(this.testSettingsUpdatedMessage);
                    let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
                    instance.testSettingObject = testObject;
                },
                    errorHandling => {
                        this.response = errorHandling.json();
                        this.errorMessage = this.response['error'];
                        this.snackbarRef.open(this.errorMessage, 'Dismiss', {
                            duration: 3000,
                        });
                    });
            }
            else {             
                this.testDetails.isQuestionMissing = true;
                let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails });
            }
        }
        else {
            this.testDetails.isQuestionMissing = false;
            let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails });
        }
    }

    pause(isTestPaused: boolean) {
        this.testDetails.isPaused = isTestPaused;        
        this.testService.updateTestById(this.testDetails.id, this.testDetails).subscribe();
    }

    resume(isTestPaused: boolean) {
        this.testDetails.isPaused = isTestPaused;
        this.testService.updateTestById(this.testDetails.id, this.testDetails).subscribe();
    }

    saveAndLaunch(isTestLaunched: boolean) {
        this.testDetails.isLaunched = isTestLaunched;
        this.testService.updateTestById(this.testDetails.id, this.testDetails).subscribe((response) => {
            this.router.navigate(['/tests']);
        },
            errorHandling => {
                this.response = errorHandling.json();
                this.errorMessage = this.response['error'];
                this.snackbarRef.open(this.errorMessage, 'Dismiss', {
                    duration: 3000,
                });
            });
    }
}

import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';
import { TestOrder } from '../enum-testorder';
import { Test } from '../tests.model';

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
    response: any;
    errorMessage: string;
    testNameReference: string;
   
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
        if (this.testDetails.startDate > endDate) {
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
        if ((new Date(this.testDetails.startDate)) < this.currentDate || this.testDetails.startDate > this.testDetails.endDate) {
            this.validStartDate = true;
            this.validEndDate = false;
        }
        else
            this.validStartDate = false;
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
            let snackBarRef = this.snackbarRef.open('Saved changes successfully', 'Dismiss', {
                duration: 3000,
            });
            snackBarRef.afterDismissed().subscribe(() => {
                this.router.navigate(['/tests']);
            });
        },
            errorHandling => {
                this.response = errorHandling.json();
                this.errorMessage = this.response['error'];
                this.snackbarRef.open(this.errorMessage, 'Dismiss', {
                    duration : 3000,
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
            },
        );
    }
}

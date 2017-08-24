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
import { TestIPAddress } from '../test-IPAdddress';

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
    isLaunchedAlready: boolean;
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
    isAttendeeExistForTest: boolean;
    testLink: string;
    copiedContent: boolean;
    tooltipMessage: string;
    ipAddressArray: TestIPAddress[] = [];
    numberOfIpFields: number[] = [];
    disablePreview: boolean;

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testDetails = new Test();
        this.isLaunchedAlready = false;
        this.validEndDate = false;
        this.validTime = false;
        this.validStartDate = false;
        this.currentDate = new Date();
        this.testSettingsUpdatedMessage = 'The settings of the Test has been updated successfully.';
        this.isAttendeeExistForTest = false;
        this.copiedContent = true;
        this.tooltipMessage = 'Copy to Clipboard';
        this.disablePreview = false;
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.loader = true;
        this.testId = this.route.snapshot.params['id'];
        this.getTestById(this.testId);
        this.isAttendeeExists();
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestById(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testDetails = (response);
            this.testNameReference = this.testDetails.testName;
            this.disablePreview = this.testDetails.categoryAcList === null || this.testDetails.categoryAcList.every(x => !x.isSelect) || this.testDetails.categoryAcList.every(x => x.numberOfSelectedQuestion === 0);
            this.loader = false;
            let magicString = this.testDetails.link;
            let domain = window.location.origin;
            this.testLink = domain + '/conduct/' + magicString;

            this.testDetails.startDate = this.toDateString(new Date(<string>this.testDetails.startDate));
            this.testDetails.endDate = this.toDateString(new Date(<string>this.testDetails.endDate));
        });
    }

    private toDateString(date: Date): string {
        return (date.getFullYear().toString() + '-'
            + ('0' + (date.getMonth() + 1)).slice(-2) + '-'
            + ('0' + (date.getDate())).slice(-2))
            + 'T' + date.toTimeString().slice(0, 5);
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
        if (new Date(<string>this.testDetails.startDate) >= new Date(endDate)) {
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
        this.validStartDate = new Date(<string>this.testDetails.startDate) < this.currentDate ? true : false;
        this.validEndDate = new Date(<string>this.testDetails.startDate) >= new Date(<string>this.testDetails.endDate) ? true : false;
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
       
        testObject.startDate = new Date(<string>testObject.startDate).toISOString();
        testObject.endDate = new Date(<string>testObject.endDate).toISOString();

        this.testService.updateTestById(id, testObject).subscribe((response) => {
            this.loader = true;
            let snackBarRef = this.snackbarRef.open('Saved changes successfully.', 'Dismiss', {
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
    launchTestDialog(id: number, testObject: Test, isTestLaunched: boolean) {

        testObject.startDate = new Date(<string>testObject.startDate).toISOString();
        testObject.endDate = new Date(<string>testObject.endDate).toISOString();

        let isCategoryAdded = this.testDetails.categoryAcList.some(x => {
            return x.isSelect;
        });
        if (isCategoryAdded) {
            let isQuestionAdded = this.testDetails.categoryAcList.some(function (x) {
                return (x.numberOfSelectedQuestion !== 0);
            });
            if (isQuestionAdded) {
                this.isLaunchedAlready = true;
                this.testDetails.isLaunched = new Date(<string>this.testDetails.startDate).getTime() <= Date.now();
                this.testService.updateTestById(id, testObject).subscribe((response) => {
                    this.ngOnInit();
                    this.openSnackBar('Your test has been launched successfully.');
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
                let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails, disableClose: true, hasBackdrop: true });
            }
        }
        else {
            this.testDetails.isQuestionMissing = false;
            let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails, disableClose: true, hasBackdrop: true });
        }
    }
    /**
    * Pause the test 
    */
    pauseTest() {
        this.testDetails.isPaused = true;
        this.testService.updateTestPauseResume(this.testDetails.id, this.testDetails.isPaused).subscribe((response) => {
            if (response)
                this.openSnackBar('Your test is paused.');
        });
    }

    /**
    * Resumes the test
    */
    resumeTest() {
        this.testDetails.isPaused = false;
        this.testDetails.isLaunched = new Date(<string>this.testDetails.startDate).getTime() <= Date.now();
        this.testService.updateTestById(this.testId, this.testDetails).subscribe((response) => {
            if (response) {
                this.ngOnInit();
                this.openSnackBar('Saved changes and resumed test.');
            }

        });
    }
    /**
     * Adds the IP address fields
     */
    addIpFields() {
        let ip = new TestIPAddress();
        this.testDetails.testIpAddress.push(ip);
    }
    /**
     * Removes ip address fields 
     * @param index
     * @param ipId
     */
    removeIpAddress(index: number, ipId: number) {
        this.testDetails.testIpAddress.splice(index, 1);
        if (ipId !== undefined)
            this.testService.deleteTestipAddress(ipId).subscribe(response => {
            });

    }


    ///**
    // * To check if any attendee for the test exixt or not
    // */
    isAttendeeExists() {
        this.testService.isTestAttendeeExist(this.testId).subscribe((isTestAttendeeExists) => {
            if (isTestAttendeeExists.response) {
                this.isAttendeeExistForTest = true;
            }
        });
    }

    /**
     * Displays the tooltip message
     * @param $event is of type Event and is used to call stopPropagation()
     */
    showTooltipMessage($event: Event, testLink: any) {
        $event.stopPropagation();
        setTimeout(() => {
            testLink.select();
        }, 0);
        this.tooltipMessage = 'Copied';
    }

    /**
     * Changes the tooltip message
     */
    changeTooltipMessage() {
        this.tooltipMessage = 'Copy to Clipboard';
    }
}

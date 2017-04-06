import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog, MdSnackBar } from '@angular/material';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { ActivatedRoute } from '@angular/router';
import { TestLaunchDialogComponent } from './test-launch-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'test-settings',
    templateUrl: 'test-settings.html'
})

export class TestSettingsComponent implements OnInit {
    testSettings: Test;
    testId: number;
    validEndDate: boolean;
    endDate: string;
    validTime: boolean;
    validStartDate: boolean;
    currentDate: Date;
    editName: string;
    testNameUpdatedMessage: string;
    testSettingsUpdatedMessage: string;

    constructor(public dialog: MdDialog, private testService: TestService, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
        this.validEndDate = false;
        this.validTime = false;
        this.validStartDate = false;
        this.currentDate = new Date();
        this.testNameUpdatedMessage = 'Test Name has been updated successfully';
        this.testSettingsUpdatedMessage = 'The settings of the Test has been updated successfully';
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
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
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestSettings(id: number) {
        this.testService.getTestSettings(id).subscribe((response) => {
            this.testSettings = (response);
        });
    }

    /**
     * Selects the Test Name from the text box containing it on focus
     * @param $event is used to select the contents of the target text box
     */
    selectAllContent($event: any) {
        $event.target.select();
    }

    /**
     * Updates the Edited Test Name in the Database om removing focus
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    updateTestName(id: number, testObject: Test) {
        this.testService.updateTestName(id, testObject).subscribe((response) => {
            this.openSnackBar(this.testNameUpdatedMessage);
        });
    }

    /**
     * Checks the End Date and Time is valid or not
     * @param endDate contains ths the value of the field End Date and Time
     */
    isEndDateValid(endDate: Date) {
        this.testSettings.startDate > endDate ? this.validEndDate = true : this.validEndDate = false;
    }

    /**
     * Checks whether the Start Date selected is valid or not
     */
    isStartDateValid() {
        new Date(this.testSettings.startDate) < this.currentDate || this.testSettings.startDate > this.testSettings.endDate ? this.validStartDate = true : this.validStartDate = false;
    }

    /**
     * Checks whether the Warning Time set is valid
     */
    isWarningTimeValid() {
        this.testSettings.warningTime >= this.testSettings.duration ? this.validTime = true : this.validTime = false;
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        this.testService.updateTestSettings(id, testObject).subscribe((response) => {
            this.openSnackBar(this.testSettingsUpdatedMessage);
        });
        let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.testSettingObject = testObject;
    }
}

import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog } from '@angular/material';
import { Test } from '../tests.model';
import { TestSettingService } from '../testsetting.service';
import { ActivatedRoute } from '@angular/router';
import { TestLaunchDialogComponent } from './test-launch-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'test-settings',
    templateUrl: 'test-settings.html'
})

export class TestSettingsComponent implements OnInit {
    testsettings: Test;
    testId: number;
    validEndDate: boolean;
    endDate: string;
    validTime: boolean;
    validStartDate: boolean;
    currentDate: Date;
    editName: string;

    /**
     * Open Launch Test Dialog
     * @param dialog is responsible for opening the Dialog box
     * @param testSettingService is used to get the Url from the testsettings.service file
     * @param route is used to take the value of Id from the active route
     */
    constructor(public dialog: MdDialog, private testSettingService: TestSettingService, private route: ActivatedRoute) {
        this.testsettings = new Test();
        this.validEndDate = false;
        this.validTime = false;
        this.validStartDate = false;
        this.currentDate = new Date();
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
    }

    //Gets the Settings saved for a particular Test
    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestSettings(id: number) {
        this.testSettingService.getSettings(id).subscribe((response) => {
            this.testsettings = (response);
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
     * @param value contains the Test Name of the Test which is to be edited 
     */
    testNameUpdation(id: number, testObject: Test, value: string) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
        });
    }

    /**
     * Checks the End Date and Time is valid or not
     * @param endDate contains ths the value of the field End Date and Time
     */
    validEndDateChecking(endDate: Date) {
        if (this.testsettings.startDate > endDate)
            this.validEndDate = true;
        else
            this.validEndDate = false;
    }

    /**
     * Checks whether the Start Date selected is valid or not
     */
    validStartDateChecking() {
        if (new Date(this.testsettings.startDate) < this.currentDate || this.testsettings.startDate > this.testsettings.endDate)
            this.validStartDate = true;
        else
            this.validStartDate = false;
    }

    /**
     * Checks whether the Warning Time set is valid
     */
    validWarningTimeChecking() {
        if (this.testsettings.warningTime >= this.testsettings.duration)
            this.validTime = true;
        else
            this.validTime = false;
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
        });
        let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.settingObject = testObject;
    }
}



import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { Test } from "../tests.model";
import { TestSettingService } from "../testsetting.service";
import { ActivatedRoute } from '@angular/router';
import { TestLaunchDialogComponent } from "./test-launch-dialog.component";



@Component({
    moduleId: module.id,
    selector: "test-settings",
    templateUrl: "test-settings.html"
})

export class TestSettingsComponent {
    testsettings: Test = new Test();
    testId: number;
    testLength: boolean = false;
    validEndDate: boolean = false;
    validIpAddress: boolean = false;
    messageLength: boolean = false;
    testPattern: boolean = false;
    testNameEmpty: boolean = false;
    warningMessage: string;
    endDate: string;
    ipAddressPattern: string;
    editName: string;


    /**
     * Open Launch Test Dialog
     * @param dialog is responsible for opening the Dialog box
     * @param testSettingService is used to get the Url from the testsettings.service file
     * @param route is used to take the value of Id from the active route
     */
    constructor(public dialog: MdDialog, private testSettingService: TestSettingService, private route: ActivatedRoute) { }

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
    onBlurMethod(id: number, testObject: Test, value: string) {
        if (value.length <= 150 && value != "" && value.match(RegExp("^[a-zA-Z0-9_@ $#%&*^{}+;:<>()-]*$"))) {
            this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
            });
        }
        else
            this.testNameEmpty = true;
    }

    /**
     * Checks the Test Name Length
     * @param value contains the Test Name
     */
    testNameLengthChecking(value: string) {
        if (value.length >= 150)
            this.testLength = true;
        else
            this.testLength = false;
    }

    /**
     * Checks the End Date and Time is valid or not
     * @param endDate contains ths the value of the field End Date and Time
     */
    validEndDateChecking(endDate: string) {
        if (this.testsettings.startDate > endDate)
            this.validEndDate = true;
        else
            this.validEndDate = false;
    }

    /**
     * Checks whether the length of the warning message is within the defined limit or not
     * @param value contains the warning message
     */
    messageLengthChecking(value: string) {
        if (this.testsettings.warningMessage.length > 255)
            this.messageLength = true;
        else
            this.messageLength = false;
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        this.ipAddressPattern = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        this.warningMessage = this.testsettings.warningMessage;
        this.endDate = this.testsettings.endDate;
        if (this.testsettings.toIpAddress.match(RegExp(this.ipAddressPattern)) && this.testsettings.fromIpAddress.match(RegExp(this.ipAddressPattern)))
            this.validIpAddress = true;
        else
            this.validIpAddress = false;
        if (this.warningMessage.length < 10 && this.endDate > this.testsettings.startDate && this.validIpAddress === true) {
            this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
            });
            var instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
            instance.settingObject = testObject;
        }
    }

}


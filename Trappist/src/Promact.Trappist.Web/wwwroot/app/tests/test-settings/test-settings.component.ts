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
     * Updates the Edited Test Name in the Database on removing focus
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    onBlurMethod(id: number, testObject: Test) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
        });
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {
        });
        var instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.settingObject = testObject;
    }
}


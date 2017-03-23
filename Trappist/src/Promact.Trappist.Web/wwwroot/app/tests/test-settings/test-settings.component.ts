import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { Test } from "../tests.model";
import { TestSettingService } from "../testsetting.service";
import { ActivatedRoute } from '@angular/router';


@Component({
    moduleId: module.id,
    selector: "test-settings",
    templateUrl: "test-settings.html"
})

export class TestSettingsComponent{
    testsettings: Test = new Test();
    testId: number;

    // Open Launch Test Dialog
    constructor(public dialog: MdDialog, private testSettingService: TestSettingService, private route: ActivatedRoute) { }

    //Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
    }

    //Gets the Settings saved for a particular Test
    getTestSettings(id:number) {
        this.testSettingService.getSettings(id).subscribe((response) => {
            this.testsettings = (response);
        });
    }

    //Selects the Test Name from the text box containing it on focus 
    selectAllContent($event: any) {
        $event.target.select();
    }   

    //Updates the Edited Test Name in the Database on removing focus
    onBlurMethod(id: number, testObject: Test) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {       
        });
    }

    //Launches the Test Dialog Box and also updates the Settings edited for the selected Test
    launchTestDialog(id: number, testObject: Test) {
        this.testSettingService.updateSettings(id, testObject).subscribe((response) => {         
        });
        var instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.settingObject = testObject;
    }
}

@Component({
    moduleId: module.id,
    selector: 'test-launch-dialog',
    templateUrl: "test-launch-dialog.html"
})

export class TestLaunchDialogComponent {
    copiedContent: boolean = true;
    settingObject: Test = new Test();
    isDisplay: string = this.settingObject.link;
}
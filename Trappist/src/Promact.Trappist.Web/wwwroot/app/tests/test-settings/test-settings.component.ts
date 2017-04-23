import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: "test-settings",
    templateUrl: "test-settings.html"
})

export class TestSettingsComponent{
    
    // Open Launch Test Dialog
    constructor(public dialog: MdDialog) {
       
    }

    launchTestDialog() {
        this.dialog.open(TestLaunchDialogComponent);
    }
}

@Component({
    moduleId: module.id,
    selector: 'test-launch-dialog',
    templateUrl: "test-launch-dialog.html"
})
export class TestLaunchDialogComponent { }


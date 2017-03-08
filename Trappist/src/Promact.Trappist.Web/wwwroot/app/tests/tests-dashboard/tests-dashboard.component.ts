import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: "tests-dashboard",
    templateUrl: "tests-dashboard.html"
})

export class TestsDashboardComponent{
    constructor(public dialog: MdDialog) { }

    createTestDialog() {
        this.dialog.open(TestCreateDialogComponent);
    }
}

@Component({
    moduleId: module.id,
    selector: 'test-create-dialog',
    templateUrl: "test-create-dialog.html"
})
export class TestCreateDialogComponent { }

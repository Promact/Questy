import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { TestService } from "../tests.service";
import { Test } from "../tests.model";


@Component({
    moduleId: module.id,
    selector: "tests-dashboard",
    templateUrl: "tests-dashboard.html"
})

export class TestsDashboardComponent{


    Tests: Test[] = new Array<Test>();

   
    constructor(public dialog: MdDialog, private testService: TestService) {

        this.getAllTests();

    }

    //Get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => { this.Tests = (response); });
        

    }

     // Open Create Test Dialog
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

import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { TestCreateDialogComponent } from "./test-create-dialog.component";
import { DeleteTestDialogComponent } from "./delete-test-dialog.component";
import { TestService } from "../tests.service";
import { Test } from "../tests.model";
import { TestSettingsComponent } from "../../tests/test-settings/test-settings.component";
import { TestSettingService } from "../testsetting.service";


@Component({
    moduleId: module.id,
    selector: "tests-dashboard",
    templateUrl: "tests-dashboard.html"
})

export class TestsDashboardComponent {


    Tests: Test[] = new Array<Test>();
    


    constructor(public dialog: MdDialog, private testService: TestService) {

        this.getAllTests();

    }

    //Get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => { this.Tests = (response), console.log(this.Tests) });


    }

    // Open Create Test Dialog
    createTestDialog() {
        this.dialog.open(TestCreateDialogComponent);
    }

    // Open Delete Test Dialog
    deleteTestDialog() {
      this.dialog.open(DeleteTestDialogComponent);
    }
}

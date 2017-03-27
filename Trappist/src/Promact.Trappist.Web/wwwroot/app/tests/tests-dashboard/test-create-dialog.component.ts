import { Component } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { Response } from '../tests.model';
import { TestsDashboardComponent } from './tests-dashboard.component';
import { MdSnackBar } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'test-create-dialog',
    templateUrl: 'test-create-dialog.html'
})

export class TestCreateDialogComponent {
    test: Test = new Test();
    responseObj: Response = new Response;
    errorMessage: boolean = false;
    testNameReference: string;
    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService, public snackBar: MdSnackBar) {
    }
    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    AddTest(testNameRef: string) {
        this.test.testName = testNameRef;
        this.testService.getTest(this.test.testName).subscribe((response) => {
            this.responseObj = response;
            console.log(response);
            if (this.responseObj.responseValue) {
                this.testService.addTests('api/tests', this.test).subscribe((responses) => {
                    this.dialogRef.close(responses);
                });
            }
            else
                this.errorMessage = true;
        });

    }

    //this method is used to disable the errorMessage
    changeError() {
        this.errorMessage = false;
    }
}

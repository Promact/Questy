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
    responseObj: boolean;
    errorMessage: boolean;
    testNameReference: string;
    public test: Test = new Test()
    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService) {
    }
    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    AddTest(testNameRef: string) {
        this.test.testName = testNameRef;
        this.testService.getTestName(this.test.testName).subscribe((response) => {
            this.responseObj = response;
            if (!this.responseObj) {
                this.testService.addTests('api/tests', this.test).subscribe((responses) => {
                    this.dialogRef.close(responses);
                });
            }
            else
                this.errorMessage = true;
        });
    }

    //this method is used to disable the errorMessage
    ChangeError() {
        this.errorMessage = false;
    }
}

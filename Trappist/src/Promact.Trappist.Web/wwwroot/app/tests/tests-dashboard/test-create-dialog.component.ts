import { Component } from '@angular/core';
import { MdDialog, MdDialogRef, MdSnackBar } from '@angular/material';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { Response } from '../tests.model';
import { TestsDashboardComponent } from './tests-dashboard.component';

@Component({
    moduleId: module.id,
    selector: 'test-create-dialog',
    templateUrl: 'test-create-dialog.html'
})

export class TestCreateDialogComponent {
    responseObj: boolean;
    errorMessage: boolean;
    testNameReference: string;
    test: Test;
    errorStatus: any;
    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService, private snackbar: MdSnackBar ) {
        this.test = new Test();
    }
    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    AddTest(testNameRef: string) {
        this.test.testName = testNameRef;
        this.testService.getTestNameCheck(this.test.testName).subscribe((response) => {
            this.responseObj = (response); 
            if (!this.responseObj) {
                this.testService.addTests('api/tests', this.test).subscribe((responses) => {
                    this.dialogRef.close(responses);
                });
            }
            else
                this.errorMessage = true;
        },          
            errorHandling => {
                this.errorStatus = errorHandling;
                this.snackbar.open(this.errorStatus);                
            });
    }
    //this method is used to disable the errorMessage
    ChangeError() {
        this.errorMessage = false;
    }
    open(message: string)
    {
    let config = this.snackbar.open(message, 'Dismiss', {
        duration: 4000,
    });
            }
}


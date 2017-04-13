
import { Component } from '@angular/core';
import { MdDialog, MdDialogRef, MdSnackBar } from '@angular/material';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { TestsDashboardComponent } from './tests-dashboard.component';

@Component({
    moduleId: module.id,
    selector: 'test-create-dialog',
    templateUrl: 'test-create-dialog.html'
})

export class TestCreateDialogComponent {
    errorMessage: boolean;
    test: Test;
    testNameReference: string;
    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService, private snackbar: MdSnackBar) {
        this.test = new Test();
    }
    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    AddTest(testNameRef: string) {
        this.test.testName = testNameRef;
        this.testService.IsTestNameUnique(testNameRef, this.test.id ).subscribe((isTestNameUnique) => {
            if (isTestNameUnique) {
                this.testService.addTests(this.test).subscribe((responses) => {
                    this.dialogRef.close(responses);
                });
            }
            else
                this.errorMessage = true;
        },
            errorHandling => {
                this.snackbar.open(errorHandling);
            });
    }
    /**
    this method is used to disable the errorMessage
    */
    ChangeError() {
        this.errorMessage = false;
    }
    /**
    to display error message in snackbar when any  error is caught from server
    */
    open(message: string) {
        let config = this.snackbar.open(message, 'Dismiss', {
            duration: 4000,
        });
    }
}
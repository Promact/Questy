import { Component } from '@angular/core';
import { Router } from '@angular/router';
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
    isWhiteSpaceError: boolean;
    isButtonClicked: boolean;

    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService, private snackbar: MdSnackBar, public route: Router) {
        this.test = new Test();
        this.isButtonClicked = false;
    }
    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    AddTest(testNameRef: string) {
        this.isButtonClicked = true;
        this.test.testName = testNameRef;
        testNameRef = testNameRef.trim();
        if (testNameRef) {
            this.testService.IsTestNameUnique(testNameRef, this.test.id).subscribe((isTestNameUnique) => {
                this.isButtonClicked = false;
                if (isTestNameUnique) {
                    this.testService.addTests(this.test).subscribe((responses) => {
                        this.dialogRef.close(responses);
                        this.route.navigate(['tests/' + responses.id + '/sections']);
                    },
                        errorhandling => {
                            this.openSnackbar('Something went wrong');
                        });
                }
                else
                    this.errorMessage = true;
            },
                errorHandling => {
                    this.openSnackbar('Something went wrong');
                });
        }
        else
            this.isWhiteSpaceError = true;
    }
    /**
    this method is used to disable the errorMessage
    */
    ChangeError() {
        this.errorMessage = false;
        this.isWhiteSpaceError = false;
    }
    /**
    to display error message in snackbar when any  error is caught from server
    */
    openSnackbar(message: string) {
        let config = this.snackbar.open(message, 'Dismiss', {
            duration: 4000,
        });
    }
}
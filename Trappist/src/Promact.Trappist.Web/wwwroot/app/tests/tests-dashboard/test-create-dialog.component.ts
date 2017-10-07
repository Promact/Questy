import { Component, OnInit } from '@angular/core';
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

export class TestCreateDialogComponent implements OnInit {
    errorMessage: boolean;
    test: Test;
    testNameReference: string;
    isWhiteSpaceError: boolean;
    isButtonClicked: boolean;

    constructor(public dialogRef: MdDialogRef<TestCreateDialogComponent>, private testService: TestService, private snackbar: MdSnackBar, public route: Router) {
        this.test = new Test();
        this.isButtonClicked = false;
    }

    ngOnInit() {
        this.selectTextArea();
    }

    /**
     * this method is used to add a new test
     * @param testNameRef is name of the test
     */
    addTest(testNameRef: string) {
        this.isButtonClicked = true;
        this.test.testName = testNameRef;
        testNameRef = testNameRef.trim();
        if (testNameRef) {
            this.testService.IsTestNameUnique(testNameRef, this.test.id).subscribe((isTestNameUnique) => {
                this.isButtonClicked = false;
                if (isTestNameUnique) {
                    this.isButtonClicked = true;
                    this.testService.addTests(this.test).subscribe((responses) => {
                        this.isButtonClicked = false;
                        this.dialogRef.close(responses);
                        this.route.navigate(['tests/' + responses.id + '/sections']);
                    },
                        errorhandling => {
                            this.isButtonClicked = false;
                            this.openSnackbar('Something went wrong.Please try again later.');
                        });
                }
                else
                    this.errorMessage = true;
            },
                errorHandling => {
                    this.isButtonClicked = false;
                    this.openSnackbar('Something went wrong.Please try again later.');
                });
        }
        else {
            this.isWhiteSpaceError = true;
            this.testNameReference = this.testNameReference.trim();
        }
    }
    /**
    this method is used to disable the errorMessage
    */
    changeError() {
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

    /**
     * When user press enter key, it will be checked first isButtonClicked is false and then test will be created so that multiple time pressing enter key will not create test with same name.
     * @param testName
     */
    onEnter(testName: string) {
        if (!this.isButtonClicked && testName)
            this.addTest(testName);
    }

    /**
     * Selects text area present in the dialog when the dialog gets opened
     */
    selectTextArea() {
        let textArea: any = document.getElementById('name');
        setTimeout(() => {
            textArea.select();
        }, 500);
    }
}
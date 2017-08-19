import { Component } from '@angular/core';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { MdSnackBar, MdDialogRef } from '@angular/material';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'duplicate-test-dialog',
    templateUrl: 'duplicate-test-dialog.html'
})
export class DuplicateTestDialogComponent {
    testName: string;
    testArray: Test[];
    testToDuplicate: Test;
    duplicatedTest: Test;
    error: boolean;
    successMessage: string;
    id: number;
    loader: boolean;

    constructor(public testService: TestService, public snackBar: MdSnackBar, public dialog: MdDialogRef<any>, private route: Router) {
        this.successMessage = 'The selected test has been duplicated successfully.';
        this.testArray = new Array<Test>();
    }

    /**
     * duplicates the selected test
     */
    duplicateTest() {
        this.id = this.testToDuplicate.id;
        this.duplicatedTest = JSON.parse(JSON.stringify(this.testToDuplicate));
        this.duplicatedTest.id = 0;
        this.duplicatedTest.testName = this.testName;
        this.loader = true;
        //Verifies that the test name is unique
        this.testService.IsTestNameUnique(this.duplicatedTest.testName, this.duplicatedTest.id).subscribe((isTestNameUnique) => {
            if (isTestNameUnique) {
                this.testService.duplicateTest(this.id, this.duplicatedTest).subscribe((response) => {
                    this.loader = false;
                    this.testArray.unshift(response);
                    this.snackBar.open(this.successMessage, 'Dismiss', {
                        duration: 3000,
                    });
                    this.dialog.close();
                    this.route.navigate(['tests/' + response.id + '/sections']);
                },
                    err => {
                        this.loader = false;
                    });
            }
            else {
                this.loader = false;
                this.error = true;
            }
        },
            err => {
                this.loader = false;
            });
    }

    // this method is used to disable the errorMessage
    onErrorChange() {
        this.error = false;
    }
}

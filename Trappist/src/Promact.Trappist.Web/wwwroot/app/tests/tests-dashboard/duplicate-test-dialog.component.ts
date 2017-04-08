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
    errorMessage: string;
    successMessage: string;
    id: number;


    constructor(public testService: TestService, public snackBar: MdSnackBar, public dialog: MdDialogRef<any>, private route:Router) {
        this.errorMessage = 'Something went worng.Please try agiain later';
        this.successMessage = 'The selected test has been duplicated successfully';
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
        //Verifies that the test mame is unique
        this.testService.IsTestNameUnique(this.duplicatedTest.testName,this.duplicatedTest.id).subscribe((isTestNameUnique) => {
            if (isTestNameUnique) {
                this.testService.duplicateTest(this.id, this.duplicatedTest).subscribe((response) => {
                    this.testArray.unshift(response);
                    this.dialog.close();
                    this.snackBar.open(this.successMessage, 'Dismiss', {
                        duration: 3000,
                    });
                    this.route.navigate(['tests/' + response.id + '/sections']);
                });
            }
            else {
                this.error = true;
            }
        },
            err => {
                this.snackBar.open(this.errorMessage, 'Dismiss', {
                    duration: 3000,
                });
            });
    }

    // this method is used to disable the errorMessage
    onErrorChange() {
        this.error = false;
    }
}

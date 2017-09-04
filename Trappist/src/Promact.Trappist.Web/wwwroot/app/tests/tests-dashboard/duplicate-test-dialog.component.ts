import { Component, OnInit, Inject } from '@angular/core';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { MdSnackBar, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'duplicate-test-dialog',
    templateUrl: 'duplicate-test-dialog.html'
})
export class DuplicateTestDialogComponent implements OnInit {
    testName: string;
    testArray: Test[];
    testToDuplicate: Test;
    duplicatedTest: Test;
    error: boolean;
    successMessage: string;
    id: number;
    loader: boolean;
    count: number;
    testId: number;

    constructor(public testService: TestService, public snackBar: MdSnackBar, public dialog: MdDialogRef<any>, @Inject(MD_DIALOG_DATA) public data: any, private route: Router) {
        this.successMessage = 'The selected test has been duplicated successfully.';
        this.testArray = new Array<Test>();
    }

    ngOnInit() {
        let test = new Test();
        test = this.data;
        this.testId = test.id;
        this.count = test.testCopiedNumber;
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
                this.count = this.count === 0 ? 1 : this.count;
                this.duplicatedTest.testName = this.testName + '_' + this.count;
                this.count = this.count + 1;
                this.testService.duplicateTest(this.id, this.duplicatedTest).subscribe((response) => {
                    this.loader = false;
                    this.testArray.unshift(response);
                    this.snackBar.open(this.successMessage, 'Dismiss', {
                        duration: 3000,
                    });
                    this.testService.setTestCopiedNumber(this.testId, this.count).subscribe((response) => {
                        this.count = response;
                    });
                    this.dialog.close();
                    this.route.navigate(['tests/' + response.id + '/sections']);
                });
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

import { Component, OnInit } from '@angular/core';
import { Test } from '../tests.model';
import { MdDialogRef, MdSnackBar } from '@angular/material';
import { TestService } from '../tests.service';

@Component({
    moduleId: module.id,
    selector: 'delete-test-dialog',
    templateUrl: 'delete-test-dialog.html'
})
export class DeleteTestDialogComponent {
    testToDelete: Test;
    testArray: Test[] = new Array<Test>();
    response: any;
    isDeleteAllowed: boolean;
    errorMessage: string;
    successMessage: string;

    constructor(private testService: TestService, public dialog: MdDialogRef<any>, public snackBar: MdSnackBar) {
        this.errorMessage = 'Something went wrong.Please try again later';
        this.successMessage = 'The selected test is deleted';
    }

    /**
     * Delete the test from the test dashboard page
     */
    deleteTest() {
        this.testService.deleteTest(this.testToDelete.id).subscribe((response) => {
            this.testArray.splice(this.testArray.indexOf(this.testToDelete), 1);
            this.dialog.close();
            this.snackBar.open(this.successMessage, 'Dismiss', {
                duration: 3000,
            });
        },
            err => {
                this.snackBar.open(this.errorMessage, 'Dismiss', {
                    duration: 3000,
                });
            });
    }

}

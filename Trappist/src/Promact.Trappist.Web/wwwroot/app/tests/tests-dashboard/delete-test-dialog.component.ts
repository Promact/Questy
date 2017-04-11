import { Component } from '@angular/core';
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
    errorMesseage: any;
    errorCorrection: boolean = true;
    response: any;
    isDeleteAllowed: boolean;
    constructor(private testService: TestService, public dialog: MdDialogRef<any>, public snackBar: MdSnackBar) { }

    /**
     * Delete the test from the test dashboard page
     */
    deleteTest() {
        this.testService.deleteTest(this.testToDelete.id).subscribe((response) => {
            this.testArray.splice(this.testArray.indexOf(this.testToDelete), 1);
            this.dialog.close();
            let snackBarRef = this.snackBar.open('The selected test is deleted', 'Dismiss', {
                duration: 3000,
            });
        },
            err => {
                this.errorCorrection = true;
                this.response = (err.json());
                this.errorMesseage = this.response['error'][0];
                let snackBarRef = this.snackBar.open(this.errorMesseage, 'Dismiss', {
                    duration: 3000,
                });
            });
    }

}

import { Component, Output, EventEmitter, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA, MdSnackBar } from '@angular/material';
import { TestSectionsComponent } from '../test-sections/test-sections.component';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestService } from '../tests.service';
import { Category } from '../../questions/category.model';
import { TestDetails } from '../test-details';

@Component({
    moduleId: module.id.toString(),
    selector: 'deselect-category',
    templateUrl: 'deselect-category.html'
})
export class DeselectCategoryComponent {

    constructor(public testService: TestService, public dialogRef: MdDialogRef<DeselectCategoryComponent>, @Inject(MD_DIALOG_DATA) public data: any, public snackbarRef: MdSnackBar) {
    }

    /**
     * When user selects 'Yes' to deselect the category, category is deselected
     */
    yesDeselectCategory() {
        this.testService.removeDeselectedCategory(this.data).subscribe((response) => {
            this.dialogRef.close(response);
        },
            err => {
                this.openSnackbar('Something went wrong');
                this.dialogRef.close();
            });
    }

    /**
     *To display message in snackbar whenever required
     */
    openSnackbar(message: string) {
        return this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
    }
}

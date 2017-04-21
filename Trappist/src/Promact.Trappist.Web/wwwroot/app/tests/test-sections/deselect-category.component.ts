import { Component, Output, EventEmitter, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA, MdSnackBar } from '@angular/material';
import { TestSectionsComponent } from '../test-sections/test-sections.component';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestService } from '../tests.service';
import { Category } from '../../questions/category.model';
import { TestDetails } from '../test-details';

@Component({
    moduleId: module.id,
    selector: 'deselect-category',
    templateUrl: 'deselect-category.html'
})
export class DeselectCategoryComponent {
    testAC: TestDetails;
    category: Category;
    test: Test;
    categoryId: number;

    constructor(public testService: TestService, public dialogRef: MdDialogRef<DeselectCategoryComponent>, @Inject(MD_DIALOG_DATA) public data: any, public snackbar: MdSnackBar) {
        this.testAC = new TestDetails();
        this.testAC = this.data;
    }

    /**
     * When user selects 'Yes' to deselect the category
     */
    YesDeselectCategory() {
        this.testService.removeDeselectedCategory(this.data).subscribe((response) => {
            this.dialogRef.close(response);
        });
    }
}

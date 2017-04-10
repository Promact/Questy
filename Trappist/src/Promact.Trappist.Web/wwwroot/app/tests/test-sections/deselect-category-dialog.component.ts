import { Component } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { TestSectionsComponent } from '../test-sections/test-sections.component';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestService } from '../tests.service';

@Component({
    moduleId: module.id,
    selector: 'deselect-category-dialog',
    templateUrl: 'deselect-category.html'
})
export class DeselectCategoryDialogComponent {
    testCategory: TestCategory;

    constructor(public dialogRef: MdDialogRef<DeselectCategoryDialogComponent>, public testService: TestService) {
        this.testCategory = new TestCategory();
    }
    Yes(categoryId: number) {
        this.testService.removeDeselectedCategory(categoryId).subscribe();
        this.dialogRef.close();
    }
}

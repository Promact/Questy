import { Component } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { TestSectionsComponent } from '../test-sections/test-sections.component';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestService } from '../tests.service';
import { Category } from '../../questions/category.model';

@Component({
    moduleId: module.id,
    selector: 'deselect-category-dialog',
    templateUrl: 'deselect-category.html'
})
export class DeselectCategoryDialogComponent {
    testCategory: TestCategory;
    category: Category;
    constructor(public dialogRef: MdDialogRef<DeselectCategoryDialogComponent>, public testService: TestService) {
        this.testCategory = new TestCategory();
        this.category = new Category();
    }
    Yes(categoryId: number) {
        this.category.id = categoryId;
        this.testService.removeDeselectedCategory(categoryId).subscribe();       
        this.category.isSelect = false;
        this.dialogRef.close();
    }
}

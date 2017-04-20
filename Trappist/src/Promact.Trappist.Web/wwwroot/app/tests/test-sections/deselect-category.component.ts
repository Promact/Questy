import { Component } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { TestSectionsComponent } from '../test-sections/test-sections.component';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestService } from '../tests.service';
import { Category } from '../../questions/category.model';

@Component({
    moduleId: module.id,
    selector: 'deselect-category',
    templateUrl: 'deselect-category.html'
})
export class DeselectCategoryComponent {
    testCategory: TestCategory;
    category: Category;
    test: Test;
    constructor(public dialogRef: MdDialogRef<DeselectCategoryComponent>, public testService: TestService) {
        this.testCategory = new TestCategory();
        this.test = new Test();
        this.category = new Category();
    }
    Yes() {
        this.testCategory.categoryId = this.category.id;
        this.testCategory.testId = this.test.id;
        this.testService.removeDeselectedCategory(this.testCategory.id).subscribe();
        this.category.isSelect = false;
        this.dialogRef.close();
    }
}

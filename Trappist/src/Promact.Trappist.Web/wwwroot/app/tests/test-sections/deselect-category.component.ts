import { Component, Output, EventEmitter } from '@angular/core';
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
    categoryId: number;
    @Output() SelectedCategoryId: any;
    dialogRef: MdDialogRef<DeselectCategoryComponent>;

    constructor( public testService: TestService) {
        this.testCategory = new TestCategory();
        this.test = new Test();
        this.category = new Category();
        this.SelectedCategoryId = new EventEmitter();
    }

    /**
     * To get the id of category to be deselected
     */
    CategoryId() {
        this.categoryId = this.SelectedCategoryId();
    }

    /**
     * When user chooses to deselect the category
     */
    Yes() {
        this.testCategory.id = this.categoryId;
        this.testService.removeDeselectedCategory(this.testCategory.id).subscribe();
        this.category.isSelect = false;
        this.dialogRef.close();
    }
}

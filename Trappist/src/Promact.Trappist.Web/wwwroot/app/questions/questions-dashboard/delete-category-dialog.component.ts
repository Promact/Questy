import { Component } from '@angular/core';
import { CategoryService } from '../category.service';
import { MdDialog } from '@angular/material';
import { Category } from '../../questions/category.model';

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: 'delete-category-dialog.html'
})

export class DeleteCategoryDialogComponent {
    categoryToDelete: Category;
    categoryArray: Category[] = new Array<Category>();

    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    // A method to remove a category from dashboard
    removeCategoryOperation() {
        this.categoryService.removeCategory(this.categoryToDelete.id).subscribe((response: any) => {
            if (response.status === 204) {
                this.categoryArray.splice(this.categoryArray.indexOf(this.categoryToDelete), 1);
            }
        });
        this.dialog.closeAll();
    }
}
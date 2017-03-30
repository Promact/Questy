import { Component } from '@angular/core';
import { CategoryService } from '../category.service';
import { MdDialog } from '@angular/material';
import { Category } from '../../questions/category.model';
import { MdSnackBar } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: 'delete-category-dialog.html'
})

export class DeleteCategoryDialogComponent {
    categoryToDelete: Category;
    categoryArray: Category[] = new Array<Category>();
    constructor(private categoryService: CategoryService, private dialog: MdDialog, private snackBar: MdSnackBar) {
    }

    // A method to remove a category from dashboard
    removeCategoryOperation() {
        this.categoryService.removeCategory(this.categoryToDelete.id).subscribe(
            result => {
                if (result.status === 204) {
                    this.categoryArray.splice(this.categoryArray.indexOf(this.categoryToDelete), 1);
                    this.snackBar.open('Category Successfully Removed', 'Dismiss', {
                        duration: 3000,
                    })
                }
            },
            err => {
                if (err.status === 404) {
                    this.snackBar.open('Category is not available', 'Dismiss', {
                        duration: 3000,
                    });
                }
            });
        this.dialog.closeAll();
    }
}
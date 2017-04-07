import { Component } from '@angular/core';
import { CategoryService } from '../categories.service';
import { MdDialog } from '@angular/material';
import { Category } from '../../questions/category.model';
import { MdSnackBar } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: 'delete-category-dialog.html'
})

export class DeleteCategoryDialogComponent {
    category: Category;
    categoryArray: Category[] = new Array<Category>();
    constructor(private categoryService: CategoryService, private dialog: MdDialog, private snackBar: MdSnackBar) {
    }

    // A method to remove a category from dashboard
    removeCategoryOperation() {
        this.categoryService.removeCategory(this.category.id).subscribe(
            result => {
                if (result.status === 204) {
                    this.categoryArray.splice(this.categoryArray.indexOf(this.category), 1);
                    this.openSnackBar('Category successfully removed');
                }
            },
            err => {
                this.openSnackBar('Something went wrong. Please try again later');
                this.openSnackBar(err.json()['Error'][0]);
            });
        this.dialog.closeAll();
    }

    /**
     * Open snackbar
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackBar.open(message, 'Dismiss', {
            duration: 3000,
        });
    }
}
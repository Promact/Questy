import { Component } from '@angular/core';
import { CategoryService } from '../categories.service';
import { Category } from '../../questions/category.model';
import { MdSnackBar, MdDialogRef } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: 'delete-category-dialog.html'
})
export class DeleteCategoryDialogComponent {
    private response: JSON;

    successMessage: string;
    errorMessage: string;
    category: Category;
    categoryArray: Category[] = new Array<Category>();

    constructor(private categoryService: CategoryService, private dialog: MdDialogRef<DeleteCategoryDialogComponent>, private snackBar: MdSnackBar) {
        this.successMessage = 'Category deleted successfully';
        this.errorMessage = 'Something went wrong. Please try again later';
    }

    /**
     * Open a Snackbar
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackBar.open(message, 'Dismiss', {
            duration: 3000,
        });
    }

    /**
     * Method to delete Category
     */
    deleteCategory(category:Category) {
        this.categoryService.deleteCategory(category.id).subscribe(
            response => {
                this.dialog.close(category);
                this.openSnackBar(this.successMessage);
            },
            err => {
                if (err.status === 400) {
                    this.response = err.json();
                    this.errorMessage = this.response['error'][0];
                    this.dialog.close(null);
                    this.openSnackBar(this.errorMessage);
                }
                else {
                    this.dialog.close(null);
                    this.openSnackBar(this.errorMessage);
                }
            });
    }
}
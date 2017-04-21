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
    private response: JSON;

    successMessage: string;
    errorMessage: string;
    category: Category;
    categoryArray: Category[] = new Array<Category>();

    constructor(private categoryService: CategoryService, private dialog: MdDialog, private snackBar: MdSnackBar) {
        this.successMessage = 'Category deleted successfully';
        this.errorMessage = 'Something went wrong. Please try again later';
    }

    /**
     * Open snackbar
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackBar.open(message, 'Dismiss', {
            duration: 3000,
        });
    }

    /**
     * Method to delete Category
     */
    deleteCategory() {
        this.categoryService.removeCategory(this.category.id).subscribe(
            result => {
                this.categoryArray.splice(this.categoryArray.indexOf(this.category), 1);
                this.openSnackBar(this.successMessage);
            },
            err => {
                if (err.status === 400) {
                    this.response = err.json();
                    this.errorMessage = this.response['error'][0];
                    this.openSnackBar(this.errorMessage);
                }
                else
                    this.openSnackBar(this.errorMessage);
            });
        this.dialog.closeAll();
    }
}
import { Component, Injectable } from '@angular/core';
import { CategoryService } from '../categories.service';
import { MdDialogRef, MdSnackBar } from '@angular/material';
import { Category } from '../category.model';

@Injectable()
@Component({
    moduleId: module.id,
    selector: 'rename-category-dialog',
    templateUrl: 'rename-category-dialog.html'
})

export class RenameCategoryDialogComponent {
    private response: any;
    private successMessage: string;

    isCategoryNameExist: boolean;
    errorMessage: string;
    category: Category;
    responseObject: Category;

    constructor(private categoryService: CategoryService, private dialogRef: MdDialogRef<RenameCategoryDialogComponent>, public snackBar: MdSnackBar) {
        this.isCategoryNameExist = false;
        this.successMessage = 'Category Name Updated Successfully';
    }

    /**
     * Open snackbar
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackBar.open(message, '', {
            duration: 3000,
        });
    }

    /**
     *Method to update Category 
     * @param category: Category object
     */
    updateCategory(category: Category) {
        if (category.categoryName) {
            this.categoryService.updateCategory(category.id, category).subscribe(
                result => {
                    this.responseObject = result;
                    this.dialogRef.close(this.responseObject);
                    this.openSnackBar(this.successMessage);
                },
                err => {
                    this.isCategoryNameExist = true;
                    this.response = (err.json());
                    this.errorMessage = this.response['error'][0];
                });
        }
    }

    /**
     *Method to toggle error message
     */
    changeErrorMessage() {
        this.isCategoryNameExist = false;
    }
}
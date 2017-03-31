import { Component, Injectable } from '@angular/core';
import { CategoryService } from '../categories.service';
import { MdDialogRef } from '@angular/material';
import { Category } from '../category.model';
import { MdSnackBar } from '@angular/material';

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
    errormessage: string;
    category: Category;
    responseobject: Category;

    constructor(private categoryService: CategoryService, private dialogRef: MdDialogRef<RenameCategoryDialogComponent>, public snackBar: MdSnackBar) {
        this.isCategoryNameExist = false;
        this.successMessage = 'Category Name Updated Sucessfully';
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
     * @param category  object contains Category details
     */
    updateCategory(category: Category) {
        if (typeof category.categoryName !== 'undefined') {
            this.categoryService.updateCategory(category.id, category).subscribe(
                result => {
                    this.responseobject = result;
                    this.dialogRef.close(this.responseobject);
                    this.openSnackBar(this.successMessage);
                },
                err => {
                    this.isCategoryNameExist = true;
                    this.response = (err.json());
                    this.errormessage = this.response['error'][0];
                });
        }
    }

    /**
     *Method to change error message when change will made in text box  
     */
    changeErrorMessage() {
        this.isCategoryNameExist = false;
    }
}
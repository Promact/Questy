import { Component } from '@angular/core';
import { Category } from '../category.model';
import { CategoryService } from '../categories.service';
import { MdDialogRef } from '@angular/material';
import { MdSnackBar } from '@angular/material';
import { QuestionsDashboardComponent } from './questions-dashboard.component'

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: 'add-category-dialog.html'
})
export class AddCategoryDialogComponent {
    private response: any;
    private successMessage: string;

    isCategoryNameExist: boolean;
    errormessage: string;
    category: Category;
    responseobject: Category;

    constructor(private categoryService: CategoryService, private dialogRef: MdDialogRef<AddCategoryDialogComponent>, public snackBar: MdSnackBar) {
        this.isCategoryNameExist = false;
        this.category = new Category();
        this.successMessage = 'Category Name Added Sucessfully';
    }

    /**
     * Open snackBar
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackBar.open(message, '', {
            duration: 3000,
        });
    }

    /**
     *Method to add Category 
     * @param category object contains Category details
     */
    addCategory(category: Category) {
        if (typeof category.categoryName !== 'undefined') {
            this.categoryService.addCategory(category).subscribe(
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

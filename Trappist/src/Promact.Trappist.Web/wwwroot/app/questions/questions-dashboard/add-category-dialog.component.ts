import { Component } from '@angular/core';
import { Category } from '../category.model';
import { CategoryService } from '../categories.service';
import { MdDialogRef, MdSnackBar } from '@angular/material';
import { QuestionsDashboardComponent } from './questions-dashboard.component';

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: 'add-category-dialog.html'
})

export class AddCategoryDialogComponent {
    private response: any;
    private successMessage: string;

    isCategoryNameExist: boolean;
    errorMessage: string;
    category: Category;
    responseObject: Category;

    constructor(private categoryService: CategoryService, private dialogRef: MdDialogRef<AddCategoryDialogComponent>, public snackBar: MdSnackBar) {
        this.isCategoryNameExist = false;
        this.category = new Category();
        this.successMessage = 'Category Name Added Successfully';
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
     * @param category:Category object
     */
    addCategory(category: Category) {
        category.categoryName = category.categoryName.trim();
        if (category.categoryName) {
            this.categoryService.addCategory(category).subscribe(
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

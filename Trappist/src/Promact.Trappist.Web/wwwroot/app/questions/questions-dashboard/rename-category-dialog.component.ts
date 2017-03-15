import { Component, Injectable } from '@angular/core';
import { CategoryService } from '../categories.service';
import { MdDialog } from '@angular/material';
import { Category } from '../category.model';
@Injectable()

@Component({
    moduleId: module.id,
    selector: 'rename-category-dialog',
    templateUrl: 'rename-category-dialog.html'
})

export class RenameCategoryDialogComponent {
    private response: any;

    showButton: boolean;
    category: Category = new Category();
    isCategoryNameExist: boolean = false;
    errormessage: any;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
        this.showButton = false;
    }

    /**
     *Method to Add Category 
     * @param category category object contains Category details
     */
    updateCategory(category: Category) {
        if (category.categoryName !== '') {
            this.categoryService.updateCategory(category.id, category).subscribe(
                result => {
                    this.dialog.closeAll();
                },
                err => {
                    this.isCategoryNameExist = true;
                    this.response = (err.json());
                    this.errormessage = this.response['error'][0];
                    this.showButton = false;
                });
        }
    }

    /**
     *Method to change Error Message when change will made in text box  
     */
    changeErrorMessage() {
        this.isCategoryNameExist = false;
    }
}
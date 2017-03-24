import { Component } from "@angular/core";
import { Category } from "../category.model";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent {
    private response: any;

    category: Category = new Category();
    isCategoryNameExist: boolean = false;
    errormesseage: any;
    showButton: boolean;

    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
        this.showButton = false;
    }

    /**
     * Method to Add Category
     * @param category Category object Contains Category Details
     */
    addCategory(category: Category) {
        if (category.categoryName !== "" && category.categoryName !== null && category.categoryName !== undefined) {
            this.categoryService.addCategory(category).subscribe(
                result => {
                    this.dialog.closeAll();
                },
                err => {
                    this.isCategoryNameExist = true;
                    this.response = (err.json());
                    this.errormesseage = this.response["error"][0];
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

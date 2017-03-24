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
    private category: Category = new Category();
    isCategoryNameExist: boolean = false;
    lengthOfCategoryName: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    /**
     * Method to Add Category
     * @param category Category object Contains Category Details
     */
    addCategory(category: Category) {
        if (category.categoryName !== "" && category.categoryName !== null && category.categoryName !== undefined) {
            this.categoryService.addCategory(category).subscribe((response) => {
            });
            this.dialog.closeAll();
        }
    }

    /**
     * Method to Check Duplicate Category Name and Character Length
     * @param categoryName:categoryName
     */
    checkDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isCategoryNameExist = result;
        });
        //check for Character Length
        if (categoryName.length > 150) {
            this.lengthOfCategoryName = true;
        }
        else {
            this.lengthOfCategoryName = false;
        }
    }
}

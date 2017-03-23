import { Component } from "@angular/core";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Category } from "../category.model";

@Component({
    moduleId: module.id,
    selector: 'rename-category-dialog',
    templateUrl: "rename-category-dialog.html"
})

export class RenameCategoryDialogComponent {
    category: Category = new Category();
    isCategoryNameExist: boolean = false;
    lengthOfCategoryName: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    /**
     *Method to Add Category 
     * @param category category object contains Category details
     */
    updateCategory(category: Category) {
        if (category.categoryName !== "") {
            this.categoryService.updateCategory(category.id, category).subscribe((response) => {
            });
            this.dialog.closeAll();
        }
    }

    /**
     * Method to Check Duplicate Category Name and Character length
     * @param categoryName:Category Name
     */
    checkDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isCategoryNameExist = result;
        });
        //check for Charcter length
        if (categoryName.length > 150) {
            this.lengthOfCategoryName = true;
        }
        else {
            this.lengthOfCategoryName = false;
        }
    }
}
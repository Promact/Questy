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
    isNameExist: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }
    /*
    * method to add Category
    *<param name="category">category object contains Category details</param>
    *After Sucessful Addition it will Close add-category-dialog
    */
    addCategory(category: Category) {
        this.categoryService.addCategory(category).subscribe((response) => {
            this.dialog.closeAll();
        });
    }
    /* method to check Whether same CategoryName Exists or not
    *<param name="categoryName">categoryname to check it Exists or not</param>
    * if categoryName Exists it will return true and button will be disabled
    */
    checkDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isNameExist = result;
        });
    }
}

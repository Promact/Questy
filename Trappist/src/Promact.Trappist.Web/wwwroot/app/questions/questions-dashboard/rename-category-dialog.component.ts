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
    isNameExist: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    /*
   * Method to Update Category
   *<param name="category">category object cobtains Category details </param>
   *after sucessful adding rename-category-diaalog will be closed
   */
    updateCategory(category: Category) {
        this.categoryService.updateCategory(category.id, category).subscribe((response) => {
        });
        this.dialog.closeAll();
    }

    /* to check Whether same CategoryName Exists or not
    *<param name="categoryName">categoryName </param>
    * if categoryName Exists it will return true and button will be disabled
    */
    checkDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isNameExist = result;
        });
    }
}

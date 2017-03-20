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
   * edit category from Cateogry Model
   */
    categoryedit(category: Category) {
        this.categoryService.editCategory(category.id, category).subscribe((response) => {
            this.dialog.closeAll();
        });
    }

    /* to check Whether CategoryName Exists or not
    * if categoryName Exists it will return true and button will be disabled
    */
    CheckDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isNameExist = result;
        });
    }
}

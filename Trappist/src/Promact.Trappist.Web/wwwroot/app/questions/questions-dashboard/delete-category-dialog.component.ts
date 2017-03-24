import { Component } from "@angular/core";
import { CategoryService } from "../category.service";
import { MdDialog } from '@angular/material';
import { Category } from "../../questions/category.model"

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: "delete-category-dialog.html"
})

export class DeleteCategoryDialogComponent {
    categoryIdToDelete: number;
    categoryArray: Category[] = new Array<Category>();
    msg: any;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    // call removeCategory() method of categoryService class 
    removeCategoryData(deleteCategory: number) {
        this.categoryService.removeCategory(deleteCategory).subscribe((response: any) => {
            if (response.status >= 200 && response.status <= 299) {
                for (var i = 0; i < this.categoryArray.length; i++) {
                    if (deleteCategory == this.categoryArray[i].id) {
                        this.categoryArray.splice(this.categoryArray.indexOf(this.categoryArray[i]), 1);
                    }
                }
            }
        });
        this.dialog.closeAll();
    }

}
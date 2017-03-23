import { Component } from "@angular/core";
import { CategoryService } from "../category.service";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: "delete-category-dialog.html"
})

export class DeleteCategoryDialogComponent {
    categoryIdToDelete: number;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }
    // call removeCategory() method of categoryService class 
    removeCategoryData(deleteCategory: number) {
        this.categoryService.removeCategory(deleteCategory).subscribe((response: any) => response);
        this.dialog.closeAll();
    }
}

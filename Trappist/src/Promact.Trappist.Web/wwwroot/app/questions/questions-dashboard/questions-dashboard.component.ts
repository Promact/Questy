import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {
    categoryName: string[] = new Array<string>();
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {
        this.getAllQuestions();
        this.getAllCategories();
    }
    //To Get All The categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryName = CategoriesList;
        });
    }
    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            console.log(questionsList);
        });
    }
    // Open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }
    // Open Delete Category Dialog
    deleteCategoryDialog(categoryNameToDelete: string) {
        var property = this.dialog.open(DeleteCategoryDialogComponent).componentInstance;
        property.categoryName = this.categoryName;
        property.categoryNameToDelete = categoryNameToDelete;
    }
}

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent { }

export class Category {
    CategoryName: string;
}

@Component({
    moduleId: module.id,
    selector: 'delete-category-dialog',
    templateUrl: "delete-category-dialog.html"
})
export class DeleteCategoryDialogComponent {
    categoryName: string[] = new Array<string>();
    categoryNameToDelete: string;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }
    // send request for Remove a Category from database
    public removeCategory(deleteCategory: string) {
        this.categoryService.removeCategory(deleteCategory).subscribe(response => { this.categoryName.splice(this.categoryName.indexOf(response.categoryName), 1) });        
        this.dialog.closeAll();
    }
}


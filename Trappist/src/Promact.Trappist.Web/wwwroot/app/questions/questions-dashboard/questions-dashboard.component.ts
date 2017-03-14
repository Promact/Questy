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

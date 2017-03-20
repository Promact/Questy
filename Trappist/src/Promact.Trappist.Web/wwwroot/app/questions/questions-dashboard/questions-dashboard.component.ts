import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { AddCategoryDialogComponent } from "./add-category-dialog.component";
import { DeleteCategoryDialogComponent } from "./delete-category-dialog.component";
import { DeleteQuestionDialogComponent } from "./delete-question-dialog.component";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {

    categoryName: string[] = new Array<string>();
    constructor(private questionsService: QuestionsService, public dialog: MdDialog, private categoryService: CategoryService) {
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
    deleteCategoryDialog() {
      this.dialog.open(DeleteCategoryDialogComponent);
    }

    // Open Delete Question Dialog
    deleteQuestionDialog() {
      this.dialog.open(DeleteQuestionDialogComponent);
    }

}

export class Category {
    CategoryName: string;
}

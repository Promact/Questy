import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { AddCategoryDialogComponent } from "./add-category-dialog.component";
import { DeleteCategoryDialogComponent } from "./delete-category-dialog.component";
import { DeleteQuestionDialogComponent } from "./delete-question-dialog.component";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../category.service";
import { Question } from "../../questions/question.model"
import { DifficultyLevel } from "../../questions/enum-difficultylevel"
import { QuestionType } from "../../questions/enum-questiontype"
import { Category } from "../../questions/category.model"
@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})
export class QuestionsDashboardComponent {
    questionDisplay: Question[] = new Array<Question>();
    categoryArray: Category[] = new Array<Category>();
    //To enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    //To enable enum questiontype in template 
    QuestionType = QuestionType;
    alpha: string[] = ["a", "b", "c", "d", "e", "..."];
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {
        this.getAllQuestions();
        this.getAllCategories();
    }
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return "correct";
        }
    }
    //To Get All The categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        });
    }
    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            this.questionDisplay = questionsList;
        });
    }
    // Open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }
    // Open Delete Category Dialog and set the propert of DeleteCategoryDialogComponent class
    deleteCategoryDialog(categoryIdToDelete: number) {
        var property = this.dialog.open(DeleteCategoryDialogComponent).componentInstance;
        property.categoryIdToDelete = categoryIdToDelete;
    }

    // Open Delete Question Dialog
    deleteQuestionDialog() {
        this.dialog.open(DeleteQuestionDialogComponent);
    }
}



import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { AddCategoryDialogComponent } from "./add-category-dialog.component";
import { DeleteCategoryDialogComponent } from "./delete-category-dialog.component";
import { DeleteQuestionDialogComponent } from "./delete-question-dialog.component";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Category } from "../category.model";
import { Location } from '@angular/common';

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent implements OnInit {
    private category: Category = new Category();
    categoryArray: string[] = new Array<string>();

export class QuestionsDashboardComponent implements OnInit {
    private category: Category = new Category();
    categoryArray: string[] = new Array<string>();
    //To enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    //To enable enum questiontype in template 
    QuestionType = QuestionType;
    alpha: string[] = ["a", "b", "c", "d", "e", "..."];
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {

    }

    ngOnInit() {
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
    // Open Delete Category Dialog
    deleteCategoryDialog() {
        this.dialog.open(DeleteCategoryDialogComponent);
    }

    // Open Delete Question Dialog
    deleteQuestionDialog() {
        this.dialog.open(DeleteQuestionDialogComponent);
    }
}
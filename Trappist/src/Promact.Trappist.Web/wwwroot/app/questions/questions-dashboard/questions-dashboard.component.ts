import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog } from '@angular/material';
import { AddCategoryDialogComponent } from './add-category-dialog.component';
import { DeleteCategoryDialogComponent } from './delete-category-dialog.component';
import { DeleteQuestionDialogComponent } from './delete-question-dialog.component';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Question } from '../../questions/question.model';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { QuestionType } from '../../questions/enum-questiontype';
import { Category } from '../../questions/category.model';
import { RenameCategoryDialogComponent } from './rename-category-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'questions-dashboard',
    templateUrl: 'questions-dashboard.html'
})

export class QuestionsDashboardComponent implements OnInit {

    questionDisplay: Question[] = new Array<Question>();
    categoryArray: Category[] = new Array<Category>();
    private category: Category = new Category();
    // to enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    // to enable enum questiontype in template 
    QuestionType = QuestionType;
    alpha: string[] = ['a', 'b', 'c', 'd', 'e', '...'];
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {
    }
    ngOnInit() {
        this.getAllQuestions();
        this.getAllCategories();
    }
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }
    // to Get All The categories
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
    // open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }
    // open Rename Category Dialog
    renameCategoryDialog(category: any) {
        var prop = this.dialog.open(RenameCategoryDialogComponent).componentInstance;
        prop.category = JSON.parse(JSON.stringify(category));
    }
    // open Delete Category Dialog
    deleteCategoryDialog() {
        this.dialog.open(DeleteCategoryDialogComponent);
    }

    // open Delete Question Dialog
    deleteQuestionDialog() {
        this.dialog.open(DeleteQuestionDialogComponent);
    }
}
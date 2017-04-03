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
@Component({
    moduleId: module.id,
    selector: 'questions-dashboard',
    templateUrl: 'questions-dashboard.html'
})
export class QuestionsDashboardComponent {
    showSearchInput: boolean;
    questionDisplay: Question[] = new Array<Question>();
    categoryArray: Category[] = new Array<Category>();
    //To enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    //To enable enum questiontype in template 
    QuestionType = QuestionType;
    optionName: string[] = ['a', 'b', 'c', 'd', 'e', '...'];
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {
        this.getAllQuestions();
        this.getAllCategories();
    }
    //To check whether the option is correct or not
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }
    //To get all the Categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        });
    }
    //To get all the Questions
    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            this.questionDisplay = questionsList;
        });
    }
    // Open add category dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }
    // Open delete category dialog
    deleteCategoryDialog() {
        this.dialog.open(DeleteCategoryDialogComponent);
    }
    // Open delete question dialog
    deleteQuestionDialog() {
        this.dialog.open(DeleteQuestionDialogComponent);
    }
}
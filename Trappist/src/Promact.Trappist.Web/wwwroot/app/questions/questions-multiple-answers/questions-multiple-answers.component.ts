import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { Router } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';
import { Observable } from 'rxjs';
@Component({
    moduleId: module.id,
    selector: 'questions-multiple-answers',
    templateUrl: 'questions-multiple-answers.html'
})

export class QuestionsMultipleAnswersComponent {
    isOptionSelected: boolean;
    noOfOptionShown: number;
    categoryName: string;
    isClose: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    multipleAnswerQuestion: QuestionBase;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {
        this.getAllCategories();
        this.noOfOptionShown = 4;
        this.isOptionSelected = false;
        this.isClose = false;
        this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
        this.categoryArray = new Array<Category>();
        this.multipleAnswerQuestion = new QuestionBase();
        this.multipleAnswerQuestion.question.difficultyLevel = 0;
        this.multipleAnswerQuestion.question.questionType = 1;
        for (let i = 0; i < this.noOfOptionShown; i++) {
            this.multipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
            this.multipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
        }
    }

    /**
     * Return category list
     */
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        });
    }

    /**
     * Redirect to question dashboard page
     */
    cancelButtonClicked() {
        this.router.navigate(['/questions']);
    }

    /**
     * Remove extra option 
     */
    removeOption(optionIndex: number) {
        this.multipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.splice(optionIndex,1);
        this.noOfOptionShown--;
        if (this.noOfOptionShown === 2) {
            this.isClose = true;
        }
    }

    /**
     * Add multiple answer question and redirect to question dashboard page
     * @param multipleAnswerQuestion
     */
    multipleQuestionAnswerAdd(multipleAnswerQuestion: QuestionBase) {
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).subscribe((response) => {
            if (response.ok) {

            }
        });
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).catch((error: any) => {
                return Observable.throw(new Error(error.status));
        });
        let snackBarRef = this.snackBar.open('Saved Changes Successfully', 'Dismiss', {
            duration: 3000,
        });
        snackBarRef.afterDismissed().subscribe(() => {
            this.router.navigate(['/questions']);
        });     
    }

    /**
    * Get category id base on category name
    * @param category
    */
    getCategoryId() {
        this.multipleAnswerQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName === this.categoryName && x.categoryName !== null).id;
    }
}

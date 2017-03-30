import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { Router } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';
@Component({
    moduleId: module.id,
    selector: 'questions-single-answer',
    templateUrl: 'questions-single-answer.html'
})

export class QuestionsSingleAnswerComponent {
    value: number;
    categoryName: string;
    noOfOptionShown: number;
    isClose: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    singleAnswerQuestion: QuestionBase;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {          
            this.getAllCategories();
            this.noOfOptionShown = 4;
            this.isClose = false;
            this.categoryArray = new Array<Category>();
            this.singleAnswerQuestion = new QuestionBase();
            this.singleAnswerQuestion.question.questionType = 0;
            this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
            this.singleAnswerQuestion.question.difficultyLevel = 0;
            for (let i = 0; i < this.noOfOptionShown; i++) {
                this.singleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption());
                this.singleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
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
     * Remove option from display page
     */
    removeOption(optionIndex: number) {
        this.singleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.splice(optionIndex,1);
        this.noOfOptionShown = this.noOfOptionShown - 1;
        if (this.noOfOptionShown === 2) {
            this.isClose= true;
        }
    } 

    /**
     * Added single answer question and redirect to question dashboard page
     * @param singleAnswerQuestion
     */
    singleQuestionAnswerAdd(singleAnswerQuestion: QuestionBase) {
            singleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
            this.questionService.addSingleAnswerQuestion(singleAnswerQuestion).subscribe((response) => {
                if (response.ok) {

                }
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
        this.singleAnswerQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName === this.categoryName).id;
    }
 }
import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionAC } from '../question';
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
    showFirstOption: boolean = true;
    showSecondOption: boolean = true;
    showThirdOption: boolean = true;
    showFourthOption: boolean = true;
    noOfOptionShown: number = 4;
    isClose: boolean = false;
    isSelected: string;
    categoryArray: Category[] = new Array<Category>();
    difficultyLevel: string[] = new Array<string>();
    singleAnswerQuestion: QuestionAC;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {          
            this.getAllCategories();        
            this.singleAnswerQuestion = new QuestionAC();
            this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
            this.singleAnswerQuestion.question.questionType = 0;
            this.singleAnswerQuestion.question.difficultyLevel = 0;
            this.singleAnswerQuestion.question.createdBy = 'Admin';
            for (let i = 0; i < this.noOfOptionShown; i++) {
                this.singleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
                this.singleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption[i].isAnswer = false;
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
    removeOption() {
        this.noOfOptionShown = this.noOfOptionShown - 1;
        if (this.noOfOptionShown === 2) {
            this.isClose = true;
        }
    } 

    /**
     * Added single answer question and redirect to question dashboard page
     * @param singleAnswerQuestion
     */
    singleQuestionAnswerAdd(singleAnswerQuestion: QuestionAC) {
        singleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
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
 }
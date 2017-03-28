import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { Questions } from '../question';
import { Router } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';
@Component({
    moduleId: module.id,
    selector: 'questions-multiple-answers',
    templateUrl: 'questions-multiple-answers.html'
})

export class QuestionsMultipleAnswersComponent {
    isOptionSelected:boolean=false;
    noOfOptionShown: number = 4;
    isClose: boolean = false;
    categoryArray: Category[] = new Array<Category>();
    difficultyLevel: string[] = ['Easy', 'Medium', 'Hard'];
    multipleAnswerQuestion: Questions;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {
        this.getAllCategories();
        this.multipleAnswerQuestion = new Questions();
        this.multipleAnswerQuestion.question.difficultyLevel = 0;
        this.multipleAnswerQuestion.question.questionType = 1;
        for (let i = 0; i < this.noOfOptionShown; i++) {
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption[i].isAnswer = false;
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
        this.multipleAnswerQuestion.singleMultipleAnswerQuestionAC.singleMultipleAnswerQuestionOption.splice(optionIndex,1);
        this.noOfOptionShown = this.noOfOptionShown - 1;
        if (this.noOfOptionShown === 2) {
            this.isClose = true;
        }
    }

    /**
     * Add multiple answer question and redirect to question dashboard page
     * @param multipleAnswerQuestion
     */
    multipleQuestionAnswerAdd(multipleAnswerQuestion: Questions) {
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).subscribe((response) => {
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

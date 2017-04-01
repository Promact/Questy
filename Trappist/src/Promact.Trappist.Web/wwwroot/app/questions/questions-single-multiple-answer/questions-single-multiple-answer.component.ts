import { Component, OnInit, ViewChild,Input } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { Router,ActivatedRoute } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';
@Component({
    moduleId: module.id,
    selector: 'questions-single-multiple-answer',
    templateUrl: 'questions-single-multiple-answer.html'
})

export class QuestionsSingleMultipleAnswerComponent {
    value: number;
    categoryName: string;
    noOfOptionShown: number;
    isClose: boolean;
    isOptionSelected:boolean;
    isSingleAnswerQuestion: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    singleMultipleQuestion: QuestionBase;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {          
            this.getAllCategories();
            this.noOfOptionShown = 4;
            this.isClose = false;
            this.categoryArray = new Array<Category>();
            this.singleMultipleQuestion = new QuestionBase();
            this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
            this.getQuestionType();
            this.singleMultipleQuestion.question.difficultyLevel = 0;
            for (let i = 0; i < this.noOfOptionShown; i++) {
                this.singleMultipleQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption());
                this.singleMultipleQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
            }         
    }

    /**
     * Return category list
     */
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
            },
            err => {
                this.snackBar.open('Failed to Load Category');
            }
        );
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
        this.singleMultipleQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.splice(optionIndex,1);
        this.noOfOptionShown-- ;
        if (this.noOfOptionShown === 2) {
            this.isClose= true;
        }
    } 

    /**
     * Get question type of question.
     */
    getQuestionType() {
        if (this.router.url === '/questions/single-answer') {
            this.singleMultipleQuestion.question.questionType = 0;
            this.isSingleAnswerQuestion = true;
        }
        if (this.router.url === '/questions/multiple-answers') {
            this.singleMultipleQuestion.question.questionType = 1;
            this.isSingleAnswerQuestion = false;
        }
    }

    /**
     * Added single/multiple answer question and redirect to question dashboard page
     * @param singleAnswerQuestion
     */
    singleMultipleQuestionAnswerAdd(singleMultipleQuestion: QuestionBase) {
        if (this.singleMultipleQuestion.question.questionType === 0) {
            singleMultipleQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
        }
        this.questionService.addSingleMultipleAnswerQuestion(singleMultipleQuestion).subscribe(
            (response) => {
                this.snackBar.open('Save Changes Successfully', 'Dismiss');
                this.router.navigate(['/questions']);
            },
            err => {
                this.snackBar.open('Failed to Save', 'Dismiss');
            }
        );
    }

    /**
     * Get category id base on category name
     */
    getCategoryId() {
        this.singleMultipleQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName === this.categoryName && this.categoryName !== null).id;
    }
 }
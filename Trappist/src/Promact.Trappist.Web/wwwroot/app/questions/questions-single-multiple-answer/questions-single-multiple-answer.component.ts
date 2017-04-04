import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';

@Component({
    moduleId: module.id,
    selector: 'questions-single-multiple-answer',
    templateUrl: 'questions-single-multiple-answer.html'
})

export class SingleMultipleAnswerQuestionComponent {
    value: number;
    categoryName: string;
    noOfOptionShown: number;
    isClose: boolean;
    isOptionSelected: boolean;
    isSingleAnswerQuestion: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    singleMultipleAnswerQuestion: QuestionBase;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {
        this.noOfOptionShown = 4;
        this.isClose = false;
        this.categoryArray = new Array<Category>();
        this.singleMultipleAnswerQuestion = new QuestionBase();
        this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
        this.singleMultipleAnswerQuestion.question.difficultyLevel = 0;
        for (let i = 0; i < this.noOfOptionShown; i++) {
            this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption());
        }
    }

    ngOnInit() {
        this.getAllCategories();
        this.getQuestionType();
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
        this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.splice(optionIndex, 1);
        this.noOfOptionShown--;
        if (this.noOfOptionShown === 2) {
            this.isClose = true;
        }
    }

    /**
     * Get question type of question based on current url
     */
    getQuestionType() {
        if (this.router.url === '/questions/single-answer') {
            this.singleMultipleAnswerQuestion.question.questionType = 0;
            this.isSingleAnswerQuestion = true;
        }
        if (this.router.url === '/questions/multiple-answers') {
            this.singleMultipleAnswerQuestion.question.questionType = 1;
            this.isSingleAnswerQuestion = false;
        }
    }

    /**
     * Add single/multiple answer question and redirect to question dashboard page
     * @param singleAnswerQuestion
     */
    saveSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion: QuestionBase) {
        if (singleMultipleAnswerQuestion.question.questionType === 0) {
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
        }
        this.questionService.addSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion).subscribe(
            (response) => {
                this.snackBar.open('Question added successfully', 'Dismiss');
                this.router.navigate(['/questions']);
            },
            err => {
                this.snackBar.open('There is some eror. Please try again.', 'Dismiss');
            }
        );
    }

    /**
     * Get category id based on category name
     */
    getCategoryId() {
        this.singleMultipleAnswerQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName === this.categoryName) !.id;
    }
}
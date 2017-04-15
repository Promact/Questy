import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionsService } from '../questions.service';
import { DifficultyLevel } from '../enum-difficultylevel';
import { Question } from '../../questions/question.model';
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';

@Component({
    moduleId: module.id,
    selector: 'questions-single-multiple-answer',
    templateUrl: 'questions-single-multiple-answer.html'
})

export class SingleMultipleAnswerQuestionComponent implements OnInit {
    value: number;
    categoryName: string;
    questionId: number;
    isRadioButtonSelected: boolean;
    difficultyLevelSelected: string;
    noOfOptionShown: number;
    isClose: boolean;
    isSingleAnswerQuestion: boolean;
    isEditQuestion: boolean;
    isNoOfOptionOverLimit: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    singleMultipleAnswerQuestion: QuestionBase;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar, private route: ActivatedRoute) {
        this.noOfOptionShown = 4;
        this.isClose = false;
        this.difficultyLevelSelected = 'default';
        this.categoryArray = new Array<Category>();
        this.singleMultipleAnswerQuestion = new QuestionBase();
        this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
        for (let i = 0; i < this.noOfOptionShown; i++) {
            this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption());
        }
    }

    ngOnInit() {
        this.questionId = +this.route.snapshot.params['id'];
        this.getQuestionType();
        this.getAllCategories();
        if (this.questionId > 0) {
            this.isEditQuestion = true;
            this.getQuestionById(this.questionId);
        }
    }

    /**
    * Gets Question of specific Id
    * @param id: Id of the Question
    */
    getQuestionById(id: number) {
        this.questionService.getQuestionById(id).subscribe((response) => {
            this.singleMultipleAnswerQuestion = response;
            this.getCategoryName();
            this.difficultyLevelSelected = DifficultyLevel[this.singleMultipleAnswerQuestion.question.difficultyLevel];
            this.value = this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.findIndex(x => x.isAnswer === true);
            this.noOfOptionShown = this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.length;
            if (this.noOfOptionShown === 2) {
                this.isClose = true;
            }
            if (this.noOfOptionShown === 10) {
                this.isNoOfOptionOverLimit = true;
            }
        });
    }

    /**
     * Return category list
     */
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        },
            err => {
                this.snackBar.open('Failed to Load Category', 'Dismiss', { duration: 3000 });
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
        if (+this.value === optionIndex) {
            this.value = null;
        }
        if (this.noOfOptionShown === 9) {
            this.isNoOfOptionOverLimit = false;
        }
    }

    /**
     * Add option on display page
     */
    addOption(optionIndex: number) {
        if (this.noOfOptionShown === 2) {
            this.isClose = false;
        }
        this.noOfOptionShown++;
        this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption());
        if (this.noOfOptionShown === 10) {
            this.isNoOfOptionOverLimit = true;
        }
    }

    /**
     * Get question type of question based on current url
     */
    getQuestionType() {
        if (this.router.url === '/questions/single-answer') {
            this.singleMultipleAnswerQuestion.question.questionType = 0;
            this.isSingleAnswerQuestion = true;
            this.isEditQuestion = false;
        }
        if (this.router.url === '/questions/multiple-answers') {
            this.singleMultipleAnswerQuestion.question.questionType = 1;
            this.isSingleAnswerQuestion = false;
            this.isEditQuestion = false;
        }
        if (this.router.url.includes('edit-single')) {
            this.isSingleAnswerQuestion = true;
        }
        if (this.router.url.includes('edit-multiple')) {
            this.isSingleAnswerQuestion = false;
        }
    }

    /**
     * Get category id based on category name
     */
    getCategoryId() {
        this.singleMultipleAnswerQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName.trim() === this.categoryName) !.id;
    }

    /**
     * Get category name based on category Id
     */
    getCategoryName() {
        this.categoryName = this.categoryArray.find(x => x.id === this.singleMultipleAnswerQuestion.question.categoryID) !.categoryName;
    }

    /**
     * Check at least one option is selected or not
     */
    isOptionSelected() {
        if (this.singleMultipleAnswerQuestion.question.questionType === 0 && this.isEditQuestion === false) {
            return this.isRadioButtonSelected;
        }
        if (this.singleMultipleAnswerQuestion.question.questionType === 0 && this.isEditQuestion === true) {
            return true;
        }
        if (this.singleMultipleAnswerQuestion.question.questionType === 1) {
            return this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.some(x => x.isAnswer === true);
        }
    }

    /**
     * Add or update single/multiple answer question and redirect to question dashboard page
     * @param singleAnswerQuestion
     */
    saveSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion: QuestionBase) {
        this.singleMultipleAnswerQuestion.question.difficultyLevel = DifficultyLevel[this.difficultyLevelSelected];
        if (singleMultipleAnswerQuestion.question.questionType === 0) {
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.isAnswer = false);
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
        }
        if (!this.isEditQuestion) {
            this.questionService.addSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion).subscribe(
                (response) => {
                    this.snackBar.open('Question added successfully', 'Dismiss', { duration: 3000 });
                    this.router.navigate(['/questions']);
                },
                err => {
                    this.snackBar.open('There is some eror. Please try again.', 'Dismiss', { duration: 3000 });
                }
            );
        }
        else {
            this.questionService.updateSingleMultipleAnswerQuestion(this.questionId, singleMultipleAnswerQuestion).subscribe(
                (response) => {
                    this.snackBar.open('Question updated successfully', 'Dismiss', { duration: 3000 });
                    this.router.navigate(['/questions']);
                },
                err => {
                    this.snackBar.open('There is some eror. Please try agai.', 'Dismiss', { duration: 3000 });
                }
            );
        }
    }
}
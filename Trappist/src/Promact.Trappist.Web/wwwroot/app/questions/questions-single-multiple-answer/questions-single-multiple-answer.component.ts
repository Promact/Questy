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

    indexOfOptionSelected: number;
    categoryName: string;
    optionIndex: number;
    questionId: number;
    difficultyLevelSelected: string;
    noOfOptionShown: number;
    isClose: boolean;
    isSingleAnswerQuestion: boolean;
    isEditQuestion: boolean;
    isdulicateQuestion: boolean;
    isNoOfOptionOverLimit: boolean;
    categoryArray: Category[];
    difficultyLevel: string[];
    singleMultipleAnswerQuestion: QuestionBase;
    isTwoOptionSame: boolean;
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar, private route: ActivatedRoute) {
        this.noOfOptionShown = 2;
        this.indexOfOptionSelected = null;
        this.isClose = true;
        this.isdulicateQuestion = false;
        this.isTwoOptionSame = false;
        this.difficultyLevelSelected = 'default';
        this.categoryName = 'default';
        this.categoryArray = new Array<Category>();
        this.singleMultipleAnswerQuestion = new QuestionBase();
        this.difficultyLevel = ['Easy', 'Medium', 'Hard'];
        for (let i = 0; i < this.noOfOptionShown; i++) {
            let option = new SingleMultipleAnswerQuestionOption();
            option.id = this.findMaxId() + 1;
            this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(option);
        }
    }

    ngOnInit() {
        let currentUrl = this.router.url;
        this.questionId = +this.route.snapshot.params['id'];
        this.getQuestionType();
        this.getAllCategories();
        if (this.questionId > 0 && !currentUrl.includes('duplicate')) {
            this.isEditQuestion = true;
            this.getQuestionById(this.questionId);
        }
        else if (currentUrl.includes('duplicate')) {
            this.getQuestionById(this.questionId);
            this.isdulicateQuestion = true;
        }
    }



    /**
    * Gets Question of specific Id
    * @param id: Id of the Question
    */
    getQuestionById(id: number) {
        this.questionService.getQuestionById(id).subscribe((response: QuestionBase) => {
            this.singleMultipleAnswerQuestion = response;
            this.getCategoryName();
            this.difficultyLevelSelected = DifficultyLevel[this.singleMultipleAnswerQuestion.question.difficultyLevel];
            this.indexOfOptionSelected = this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.findIndex(x => x.isAnswer === true);
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
     * Finds the greatest Id of option and increments it
     */
    private findMaxId() {
        return (this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.length === 0 ? 0 : Math.max.apply(Math, this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.map(function (o) { return o.id; })));
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
        this.indexOfOptionSelected = null;
        if (this.noOfOptionShown === 2) {
            this.isClose = true;
        }
        if (+this.indexOfOptionSelected === optionIndex) {
            this.indexOfOptionSelected = null;
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
        let newOption = new SingleMultipleAnswerQuestionOption();
        newOption.id = this.findMaxId() + 1;
        this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(newOption);
        if (this.noOfOptionShown === 10) {
            this.isNoOfOptionOverLimit = true;
        }
    }


    isTwoOptionsSame(optionName: string, optionIndex: number) {
        this.isTwoOptionSame = false;
        for (let i = 0; i < this.noOfOptionShown; i++) {
            if (i !== optionIndex)
                this.isTwoOptionSame = this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].option === optionName.trim();
            this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isTwoOptionsSame = this.isTwoOptionSame;
            if (this.isTwoOptionSame) {
                break;
            }
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
        else if (this.router.url === '/questions/multiple-answers') {
            this.singleMultipleAnswerQuestion.question.questionType = 1;
            this.isSingleAnswerQuestion = false;
            this.isEditQuestion = false;
        }
        else if (this.router.url.includes('edit-single')) {
            this.isSingleAnswerQuestion = true;
        }
        else if (this.router.url.includes('single-answer/duplicate')) {
            this.isSingleAnswerQuestion = true;
            this.isEditQuestion = false;
            this.isdulicateQuestion = true;
        }
        else if (this.router.url.includes('edit-multiple')) {
            this.isSingleAnswerQuestion = false;
        }
        else if (this.router.url.includes('multiple-answers/duplicate')) {
            this.isSingleAnswerQuestion = false;
            this.isEditQuestion = false;
            this.isdulicateQuestion = true;
        }
    }

    /**
     * Get category id based on category name
     * @param category: Category selected by the user
     */
    getCategoryId(category: string) {
        this.categoryName = category;
        this.singleMultipleAnswerQuestion.question.categoryID = this.categoryArray.find(x => x.categoryName === this.categoryName)!.id;
    }

    /**
     * Get category name based on category Id
     */
    getCategoryName() {
        this.categoryName = this.categoryArray.find(x => x.id === this.singleMultipleAnswerQuestion.question.categoryID)!.categoryName;
    }

    /**
     * Sets the difficulty level
     * @param difficulty: Difficulty level selected by the user
     */
    setDifficultyLevel(difficulty: string) {
        this.difficultyLevelSelected = difficulty;
    }

    /**
     * Check at least one option is selected or not
     */
    isOptionSelected() {
        if (this.singleMultipleAnswerQuestion.question.questionType === 0) {
            return (this.indexOfOptionSelected === null ? false : true);
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
        this.singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.id = 0);
        if (singleMultipleAnswerQuestion.question.questionType === 0) {
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.isAnswer = false);
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[this.indexOfOptionSelected].isAnswer = true;
        }
        if (this.isdulicateQuestion) {
            singleMultipleAnswerQuestion.question.id = 0;
            singleMultipleAnswerQuestion.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.id = 0);
        }
        ((this.isEditQuestion) ?
            (this.questionService.updateSingleMultipleAnswerQuestion(this.questionId, singleMultipleAnswerQuestion)) :
            (this.questionService.addSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion))).subscribe(
            (response) => {
                if (this.isEditQuestion) {
                    this.snackBar.open('Question updated successfully', 'Dismiss', { duration: 3000 });
                }
                else {
                    this.snackBar.open('Question added successfully', 'Dismiss', { duration: 3000 });
                }
                this.router.navigate(['/questions']);
            },
            err => {
                this.snackBar.open('There is some error. Please try again.', 'Dismiss', { duration: 3000 });
            }
            );
    }
}
﻿import { Component, OnInit } from '@angular/core';
import { Question } from '../../questions/question.model';
import { Category } from '../../questions/category.model';
import { TestService } from '../tests.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig, MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { Test, TestQuestionAC } from '../tests.model';
import { QuestionBase } from '../../questions/question';
import { QuestionType } from '../../questions/enum-questiontype';
import { RandomQuestionSelectionDialogComponent } from './random-question-selection-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'test-questions',
    templateUrl: 'test-questions.html'
})

export class TestQuestionsComponent implements OnInit {
    editName: boolean;
    DifficultyLevel = DifficultyLevel;
    QuestionType = QuestionType;
    questionsToAdd: TestQuestionAC[];
    testId: number;
    isSaveExit: boolean;
    testDetails: Test;
    loader: boolean = false;
    loader_question: boolean = false;
    optionName: string[];
    testNameReference: string;
    isAnyCategorySelectedForTest: boolean;
    isEditTestEnabled: boolean;
    disablePreview: boolean;
    array: QuestionBase[];
    randomQuestionsArray: QuestionBase[];
    questionList: QuestionBase[];

    constructor(private testService: TestService, public snackBar: MdSnackBar, public router: ActivatedRoute, public route: Router, public dialog: MdDialog) {
        this.testDetails = new Test();
        this.isSaveExit = false;
        this.questionsToAdd = [];
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
        this.disablePreview = false;
        this.array = [];
        this.questionList = [];  
        this.router.params.subscribe(params => {
            this.testId = params['id'];
        });
    }

    ngOnInit() {
        this.getTestDetails();
        this.isTestAttendeeExist();
    }

    openSnackBar(text: string) {
        this.snackBar.open(text, 'Dismiss', {
            duration: 3000
        });
    }

    /**
     * Gets all the questions of particular category by passing its Id
     * @param category
     * @param i is index of category
     */
    getAllquestions(category: Category, i: number) {
        this.loader_question = true;
        if (!category.isAccordionOpen) {
            category.isAccordionOpen = true;
            if (!category.isAlreadyClicked) {//If Accordion is already clicked then it wont call the server next time it is clicked,so that user can not lose its selected questions
                category.isAlreadyClicked = true;
                this.testService.getQuestions(this.testDetails.id, category.id).subscribe(response => {
                    this.testDetails.categoryAcList[i].questionList = response;//gets the total number of questions of particular category
                    this.array = this.testDetails.categoryAcList[i].questionList.slice();
                    this.questionList = this.testDetails.categoryAcList[i].questionList;
                    this.testDetails.categoryAcList[i].numberOfSelectedQuestion = this.testDetails.categoryAcList[i].questionList.filter(question => {
                        return question.question.isSelect;
                    }).length;
                    category.selectAll = category.questionList.every(question => {
                        return question.question.isSelect;
                    });
                    this.testDetails.categoryAcList[i].numberOfRandomQuestionsSelected = this.testDetails.categoryAcList[i].numberOfSelectedQuestion;
                    this.loader_question = false;
                });
            } else {
                category.isAlreadyClicked = true;
                this.loader_question = false;
            }
        } else {
            category.isAccordionOpen = false;
            this.loader_question = false;
        }
    }

    /**
     * returns 'correct' class for correct option
     * @param isAnswer
     */
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }

    /**
     * Gets the details of a test by passing its Id
     */
    getTestDetails() {
        this.loader = true;
        this.testService.getTestById(this.testId).subscribe(response => {
            this.testDetails = response;
            this.disablePreview = this.testDetails.categoryAcList === null || this.testDetails.categoryAcList.every(x => !x.isSelect) || this.testDetails.categoryAcList.every(x => x.numberOfSelectedQuestion === 0);
            this.testNameReference = this.testDetails.testName;
            this.isAnyCategorySelectedForTest = this.testDetails.categoryAcList.some(function (category) {
                return category.isSelect;
            });
            this.loader = false;
        }, err => {
            this.loader = false;
            this.openSnackBar('No test found for this id.');
            this.route.navigate(['/tests']);
        });
    }

    /**
     * Selects questions from the list of questions of a particular category
     * @param question is an object of QuestionBase
     * @param category is an object of Category
     */
    selectQuestion(questionToSelect: QuestionBase, category: Category) {
        if (questionToSelect.question.isSelect) {//If all questions are selected except one,and If user selects that question, then selectAll checkbox will be selected
            let isAllSelected = category.questionList.every(function (question) {
                return question.question.isSelect;
            });
            if (isAllSelected)
                category.selectAll = true;
            category.numberOfSelectedQuestion++;
        } else {
            category.selectAll = false;
            questionToSelect.question.isSelect = false;
            category.numberOfSelectedQuestion--;
        }
    }

    /**
     * Adds all the questions to to database and navigate to test-settings.component
     */
    saveNext() {
        this.loader = true;
        this.questionsToAdd = new Array<TestQuestionAC>();
        //It checks for every category of a test
        for (let category of this.testDetails.categoryAcList) {
            //If question list of a category is not null
            if (category.isSelect && category.questionList !== null)
                //Every question from category are concatenated to single array which will be sent to add to test
                category.questionList.forEach(question => {
                    let questionToAdd = new TestQuestionAC();
                    questionToAdd.categoryID = question.question.categoryID;
                    questionToAdd.id = question.question.id;
                    questionToAdd.isSelect = question.question.isSelect;
                    this.questionsToAdd.push(questionToAdd);
                });
        }
        if (this.isEditTestEnabled) {
            this.testService.addTestQuestions(this.questionsToAdd, this.testId).subscribe(response => {
                if (response) {
                    this.openSnackBar(response.message);
                    if (this.isSaveExit) {
                        this.loader = false;
                        this.route.navigate(['/tests']);
                    }
                    else {
                        this.loader = false;
                        this.route.navigate(['tests/' + this.testId + '/settings']);
                    }
                }
            },
                error => {
                    this.loader = false;
                    this.openSnackBar('Something went wrong. Please try again later.');
                });
        }
        else
            this.route.navigate(['tests/' + this.testId + '/settings']);
    }

    /**
     * Selects and deselect all the questions of a category
     * @param category object
     * @param questionCount number of all the questions of a category
     */
    selectAll(category: Category) {
        category.questionList.map(function (questionList) {
            //If selectAll checkbox is selected
            if (category.selectAll) {
                //every question is selected
                questionList.question.isSelect = true;
                category.numberOfSelectedQuestion = category.questionCount;
            }
            else {
                //If selectAll checkbox is unselected ,then every question is deselected
                questionList.question.isSelect = false;
                category.numberOfSelectedQuestion = 0;
            }
        });
    }

    /**
     * Adds the questions to question table and redirect to test dashboard
     */
    saveExit() {
        this.isSaveExit = true;
        if (this.isEditTestEnabled)
            this.saveNext();
        else
            this.route.navigate(['/tests']);
    }

    /**
    * Checks if any candidate has taken the test
    */
    isTestAttendeeExist() {
        this.testService.isTestAttendeeExist(this.testId).subscribe(res => {
            this.isEditTestEnabled = !res;
        });
    }

    /**
     * Gets the shuffled array for the question array of a particular category
     * @param k contains the index number of the category which is being selected
     */
    GetShuffledQuestionArray(k: number) {
        this.testDetails.categoryAcList[k].numberOfRandomQuestionsSelected = this.testDetails.categoryAcList[k].numberOfSelectedQuestion;
        this.randomQuestionsArray = this.shuffleArray(this.array);
    }

    /**
     * Selects the questions in a random order in the select questions page
     * @param n contains the number of questions to be selected in random order
     * @param k contains the index of the category which is being selected
     */
    selectRandomQuestions(n: number, k: number) {
        this.questionList.forEach(x => x.question.isSelect = false);
        for (let i = 0; i < n; i++) {
            for (let j = 0; j < this.testDetails.categoryAcList[k].questionList.length; j++) {
                if (this.testDetails.categoryAcList[k].questionList[j].question.id === this.randomQuestionsArray[i].question.id)
                    this.testDetails.categoryAcList[k].questionList[j].question.isSelect = true;
            }
        }
        this.testDetails.categoryAcList[k].numberOfSelectedQuestion = this.testDetails.categoryAcList[k].questionList.filter(question => {
            return question.question.isSelect;
        }).length;
        this.testDetails.categoryAcList[k].selectAll = this.testDetails.categoryAcList[k].questionList.every(x => x.question.isSelect);
    }

    /**
     * Shuffles the contents of the array
     * @param arrayToShuffle contains the array whose contents are to be shuffled
     */
    private shuffleArray(arrayToShuffle: any[]) {
        let max = arrayToShuffle.length - 1;
        for (let i = 0; i < max; i++) {
            let pickIndex = Math.floor(Math.random() * max);
            [arrayToShuffle[i], arrayToShuffle[pickIndex]] = [arrayToShuffle[pickIndex], arrayToShuffle[i]];
        }
        return arrayToShuffle;
    }

    /**
     * Opens the dialog box where the number of questions to be selected randomly is to be entered
     * @param category is the object of Category
     * @param k contains the index of the category which is being selected
     */
    openDialog(category: Category, k: number): void {
        let dialogRef = this.dialog.open(RandomQuestionSelectionDialogComponent, {
            data: { numberOfQuestions: category.numberOfRandomQuestionsSelected, numberOfQuestionsInSelectedCategory: category.questionList.length },
            disableClose: true, hasBackdrop: true
        });

        dialogRef.afterClosed().subscribe(result => {
            category.numberOfRandomQuestionsSelected = result;
            if (result !== '')
                this.selectRandomQuestions(category.numberOfRandomQuestionsSelected, k);
            else
                category.numberOfRandomQuestionsSelected = this.testDetails.categoryAcList[k].numberOfSelectedQuestion = this.testDetails.categoryAcList[k].questionList.filter(question => {
                    return question.question.isSelect;
                }).length;
        });
    }
}


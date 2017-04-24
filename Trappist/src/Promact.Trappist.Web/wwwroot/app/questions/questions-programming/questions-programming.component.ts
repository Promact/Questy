﻿import { Component, OnInit, ViewChild } from '@angular/core';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { DifficultyLevel } from '../enum-difficultylevel';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { CodeSnippetQuestionsTestCases } from '../../questions/code-snippet-questions-test-cases.model';
import { TestCaseType } from '../enum-test-case-type';

@Component({
    moduleId: module.id,
    selector: 'questions-programming',
    templateUrl: 'questions-programming.html'
})

export class QuestionsProgrammingComponent implements OnInit {

    selectedLanguageList: string[];
    codingLanguageList: string[];
    selectedCategory: string;
    selectedDifficulty: string;
    categoryList: Category[];
    questionModel: QuestionBase;
    formControlModel: FormControlModel;

    nolanguageSelected: boolean;
    isCategoryReady: boolean;
    isLanguageReady: boolean;
    isFormSubmitted: boolean;
    code: any;
    testCases: CodeSnippetQuestionsTestCases[];
    //To enable enum testCaseType in template
    testCaseType: TestCaseType;
    questionId: number;

    private successMessage: string = 'Question saved successfully';
    private failedMessage: string = 'Question failed to save';
    private routeToDashboard = ['questions'];

    constructor(private questionsService: QuestionsService,
        private categoryService: CategoryService,
        private snackBar: MdSnackBar,
        private router: Router,
        private route: ActivatedRoute) {

        this.nolanguageSelected = true;
        this.isCategoryReady = false;
        this.isLanguageReady = false;
        this.selectedLanguageList = new Array<string>();
        this.codingLanguageList = new Array<string>();
        this.categoryList = new Array<Category>();
        this.questionModel = new QuestionBase();
        this.selectedCategory = 'Please select a category';
        this.selectedDifficulty = 'Easy';
        this.formControlModel = new FormControlModel();
        this.testCases = new Array<CodeSnippetQuestionsTestCases>();
    }

    ngOnInit() {
        this.questionId = +this.route.snapshot.params['id'];
        if (this.questionId === undefined
            || this.questionId === null
            || isNaN(this.questionId)) {
            this.getCodingLanguage();
            this.getCategory();
        }
        else {
            this.isQuestionEditted = true;
            this.prepareToEdit(this.questionId);
        }
    }

    /**
     * Prepares the form for editting
     */
    prepareToEdit(id: number) {
        this.getQuestionById(id);
    }

    /**
     * Gets Question of specific Id
     * @param id: Id of the Question
     */
    getQuestionById(id: number) {
        this.questionsService.getQuestionById(id).subscribe(
            (response) => {
                this.questionModel = response;
                this.selectedDifficulty = DifficultyLevel[this.questionModel.question.difficultyLevel];
                this.testCases = this.questionModel.codeSnippetQuestion.testCases;

                //If Question has no test case show the button to add new test case
                if (this.testCases.length > 0)
                    this.formControlModel.showTestCase = true;

                this.getCodingLanguage();
                this.getCategory();
            },
            err => {
                this.openSnackBar('Question not found', true, this.routeToDashboard);
            });
    }

    /**
     *  Adds test cases of code snippet question
     */
    addTestCases() {
        let testCase = new CodeSnippetQuestionsTestCases();
        testCase.id = this.findMaxId() + 1;
        this.testCases.push(testCase);
    }

    /**
     * Finds the greatest Id of testcases and increments it
     */
    private findMaxId() {
        return (this.testCases.length === 0 ? 0 : Math.max.apply(Math, this.testCases.map(function (o) { return o.id; })));
    }

    /**
     * Removes the test cases of code snippet question 
     * @param testCaseIndex
     */
    removeTestCases(testCaseIndex: number) {
        this.testCases.splice(testCaseIndex, 1);
    }

    /**
     * Gets all the coding languages
     */
    private getCodingLanguage() {
        this.questionsService.getCodingLanguage().subscribe((response) => {
            this.codingLanguageList = response;
            this.codingLanguageList.sort();

            if (this.isQuestionEditted) {
                this.questionModel.codeSnippetQuestion.languageList.forEach(x => {
                    this.selectLanguage(x);
                });
            }

            this.isLanguageReady = true;
        });
    }

    /**
     * Gets all the categories
     */
    private getCategory() {
        this.categoryService.getAllCategories().subscribe((response) => {
            this.categoryList = response;

            //If question is being editted then set the category
            if (this.isQuestionEditted)
                this.selectedCategory = this.categoryList.find(x => x.id === this.questionModel.question.categoryID).categoryName;

            this.isCategoryReady = true;
        });
    }

    /**
     * Adds language to the selectedLanguageList
     * @param language : language to add
     */
    selectLanguage(language: string) {
        let index = this.selectedLanguageList.indexOf(language);
        if (index === -1) {
            this.selectedLanguageList.push(language);
            this.codingLanguageList.splice(this.codingLanguageList.indexOf(language), 1);
        }
        this.checkIfNoLanguageSelected();
    }

    /**
     * Removes language from selectedLanguageList 
     * @param language : Language to remove
     */
    removeLanguage(language: string) {
        this.selectedLanguageList.splice(this.selectedLanguageList.indexOf(language), 1);
        this.codingLanguageList.push(language);
        this.checkIfNoLanguageSelected();
        this.codingLanguageList.sort();
    }

    /**
     * Toggles nolanguageSelected according to selectedLanguageList 
     */
    private checkIfNoLanguageSelected() {
        this.nolanguageSelected = this.selectedLanguageList.length === 0 ? true : false;
    }

    /**
     * Adds category to the Question 
     * @param category : category to be added
     */
    selectCategory(category: string) {
        this.questionModel.question.categoryID = this.categoryList.find(x => x.categoryName === category).id;
    }

    /**
     * Adds difficulty to the Question
     * @param difficulty : Difficulty level to select
     */
    selectDifficulty(difficulty: string) {
        this.questionModel.question.difficultyLevel = DifficultyLevel[difficulty];
    }

    //Converts enum of type TestCaseType to string
    getTestCaseString(testCase: TestCaseType) {
        return TestCaseType[testCase];
    }

    /**
     * Opens snack bar
     * @param message: message to display
     * @param enableRouting: enable routing after snack bar dismissed
     * @param routeTo: routing path 
     */
    private openSnackBar(message: string, enableRouting: boolean = false, routeTo: any[] = ['']) {
        let snackBarAction = this.snackBar.open(message, 'Dismiss', {
            duration: 3000
        });
        if (enableRouting) {
            this.router.navigate(routeTo);
        }
    }

    /**
     * Sends post request to add code snippet question
     * @param isCodeSnippetFormValid : Validation status of code snippet form
     */
    addCodingQuestion(isCodeSnippetFormValid: boolean) {
        if (isCodeSnippetFormValid && !this.nolanguageSelected) {
            //Lock the form. Load spinner.
            this.isFormSubmitted = true;

            this.questionModel.question.questionType = 2; // QuestionType 2 for programming question
            this.testCases.forEach(x => x.id = 0);//Explicitly converting the id of the testcases to zero
            this.questionModel.codeSnippetQuestion.testCases = this.testCases;
            this.questionModel.codeSnippetQuestion.languageList = [];
            this.selectedLanguageList.forEach(language => {
                this.questionModel.codeSnippetQuestion.languageList.push(language);
            });
            if (!this.isQuestionEditted) {
                this.questionsService.addCodingQuestion(this.questionModel).subscribe(
                    (response) => {
                        this.openSnackBar(this.successMessage, true, this.routeToDashboard);
                    },
                    err => {
                        this.openSnackBar(this.failedMessage);
                        //Release the form for user to retry
                        this.isFormSubmitted = false;
                    }
                );
            } else {
                this.questionsService.updateQuestionById(this.questionId, this.questionModel).subscribe(
                    (response) => {
                        this.openSnackBar(this.successMessage, true, this.routeToDashboard);
                    },
                    err => {
                        this.openSnackBar(this.failedMessage);
                        //Release the form for user to retry
                        this.isFormSubmitted = false;
                    }
                );
            }
        }
    }
}

class FormControlModel {
    showTestCase: boolean;
    collapseTestCase: boolean;
    showNewTestCase: boolean;
}

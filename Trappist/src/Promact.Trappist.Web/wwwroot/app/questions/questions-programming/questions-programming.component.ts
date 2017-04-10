import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { DifficultyLevel } from '../enum-difficultylevel';
import { MdSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { CodeSnippetQuestionsTestCases } from '../../questions/code-snippet-questions-test-cases.model';

@Component({
    moduleId: module.id,
    selector: 'questions-programming',
    templateUrl: 'questions-programming.html'
})

export class QuestionsProgrammingComponent implements OnInit {

    selectedLanguageList: string[];
    codingLanguageList: string[];
    categoryList: Category[];
    questionModel: QuestionBase;
    formControlModel: FormControlModel;

    nolanguageSelected: boolean;
    isCategoryReady: boolean;
    isLanguageReady: boolean;
    isFormSubmitted: boolean;
    isTestCaseAdded: boolean;
    code: any;
    testCases: CodeSnippetQuestionsTestCases[];

    private successMessage: string = 'Question saved successfully';
    private failedMessage: string = 'Question failed to save';
    private routeToDashboard = '/questions';

    constructor(private questionsService: QuestionsService,
        private categoryService: CategoryService,
        private snackBar: MdSnackBar,
        private router: Router) {

        this.nolanguageSelected = true;
        this.isCategoryReady = false;
        this.isLanguageReady = false;
        this.isTestCaseAdded = false;
        this.selectedLanguageList = new Array<string>();
        this.codingLanguageList = new Array<string>();
        this.categoryList = new Array<Category>();
        this.questionModel = new QuestionBase();
        this.formControlModel = new FormControlModel();
        this.testCases = new Array<CodeSnippetQuestionsTestCases>();
    }

    ngOnInit() {
        this.getCodingLanguage();
        this.getCategory();
    }
    /**
     *  Adds test cases of code snippet question
     */
    addTestCases() {
        this.testCases.push(new CodeSnippetQuestionsTestCases());
        //this.isTestCaseAdded = true;
    }

    /**
     * Gets all the coding languages
     */
    private getCodingLanguage() {
        this.questionsService.getCodingLanguage().subscribe((response) => {
            this.codingLanguageList = response;
            this.codingLanguageList.sort();
            this.isLanguageReady = true;
        });
    }

    /**
     * Gets all the categories
     */
    private getCategory() {
        this.categoryService.getAllCategories().subscribe((response) => {
            this.categoryList = response;
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

    /**
     * Opens snack bar
     * @param message: message to display
     * @param enableRouting: enable routing after snack bar dismissed
     * @param routeTo: routing path 
     */
    private openSnackBar(message: string, enableRouting: boolean = false, routeTo: string = '') {
        let snackBarAction = this.snackBar.open(message, 'Dismiss', {
            duration: 3000
        });

        if (enableRouting) {
            snackBarAction.afterDismissed().subscribe(() => {
                this.router.navigate([routeTo]);
            });
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
            this.questionModel.codeSnippetQuestion.testCases = this.testCases;
            this.questionModel.codeSnippetQuestion.languageList = [];
            this.selectedLanguageList.forEach(language => {
                this.questionModel.codeSnippetQuestion.languageList.push(language);
            });
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
        }
    }
}

class FormControlModel {
    showTestCase: boolean;
    collapseTestCase: boolean;
    showNewTestCase: boolean;
}

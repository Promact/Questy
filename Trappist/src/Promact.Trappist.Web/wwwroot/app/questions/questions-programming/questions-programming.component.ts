import { Component, OnInit, ViewChild } from '@angular/core';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionBase } from '../question';
import { DifficultyLevel } from '../enum-difficultylevel';
import { MdSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { CodeSnippetQuestionsTestCases } from '../../questions/code-snippet-questions-test-cases.model';
import { TestCaseType } from '../enum-test-case-type';
import { QuestionType } from '../enum-questiontype';

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
    isQuestionEmpty: boolean;

    nolanguageSelected: boolean;
    isCategoryReady: boolean;
    isLanguageReady: boolean;
    isFormSubmitted: boolean;
    isQuestionEdited: boolean;
    isQuestionDuplicated: boolean;
    isDefaultTestCaseAdded: boolean;
    isCkeditorDirtly: boolean;
    code: any;
    testCases: CodeSnippetQuestionsTestCases[];
    isCategorySelected: boolean;
    selectedCategoryName: string;
    selectedDifficultyLevel: string;
    //To enable enum testCaseType in template 
    testCaseType: TestCaseType;
    questionId: number;    

    private successMessage: string = 'Question saved successfully.';
    private failedMessage: string = 'Question failed to save.';
    private routeToDashboard = ['questions', this.selectedCategory,this.selectedDifficultyLevel];

    constructor(private questionsService: QuestionsService,
        private categoryService: CategoryService,
        private snackBar: MdSnackBar,
        private router: Router,
        private route: ActivatedRoute) {
        this.nolanguageSelected = true;
        this.isCategoryReady = false;
        this.isLanguageReady = false;
        this.isQuestionEdited = false;
        this.isQuestionDuplicated = false;
        this.selectedLanguageList = new Array<string>();
        this.codingLanguageList = new Array<string>();
        this.categoryList = new Array<Category>();
        this.questionModel = new QuestionBase();
        this.selectedCategory = 'AllCategory';
        this.selectedDifficulty = 'Easy';
        this.formControlModel = new FormControlModel();
        this.testCases = new Array<CodeSnippetQuestionsTestCases>();
        this.isDefaultTestCaseAdded = true;
        this.isQuestionEmpty = true;
        this.isCkeditorDirtly = false;
    }

    ngOnInit() {
        this.questionId = this.route.snapshot.params['id'];
        this.selectedCategory = this.route.snapshot.params['categoryName'];
        this.selectedDifficultyLevel = this.route.snapshot.params['difficultyLevelName'];

        if (!this.questionId) {
            this.getCodingLanguage();
            this.getCategory();
        }
        else if (this.router.url.includes('duplicate')) {
            this.isQuestionDuplicated = true;
            this.prepareToEdit(+this.questionId);
        }
        else {
            this.isQuestionEdited = true;
            this.prepareToEdit(+this.questionId);
        }
    }

    /**
     * Prepares the form for editting
     */
    prepareToEdit(id: number) {
        if (isNaN(id)) {
            this.openSnackBar('Question not found.', true, this.routeToDashboard);
        }
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

                //If question fetched is not a CodeSnippetQuestion the show error message
                if (this.questionModel.question.questionType !== QuestionType.codeSnippetQuestion)
                    this.openSnackBar('Question not found.', true, this.routeToDashboard);

                this.selectedDifficulty = DifficultyLevel[this.questionModel.question.difficultyLevel];
                this.testCases = this.questionModel.codeSnippetQuestion.codeSnippetQuestionTestCases;

                //If Question has no test case show the button to add new test case
                if (this.testCases.length > 0)
                    this.formControlModel.showTestCase = true;

                this.getCodingLanguage();
                this.getCategory();
            },
            err => {
                this.openSnackBar('Question not found.', true, this.routeToDashboard);
            });
    }

    /**
     *  Adds test cases of code snippet question
     */
    addTestCases() {
        let testCase = new CodeSnippetQuestionsTestCases();
        testCase.testCaseType = TestCaseType.Default;
        testCase.id = this.findMaxId() + 1;
        this.testCases.push(testCase);
        this.isDefaultTestCaseAdded = true;
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
        if (this.testCases.length === 0) {
            this.formControlModel.showTestCase = false;
        }
    }

    trimString(str: string) {
        if (str) {
            return str.trim();
        }
    }

    onFocusCkeditor(event) {
        let trimedQuestion = this.questionModel.question.questionDetail.replace(/<p>|&nbsp;|<\/p>/gi, '').trim();
        if (trimedQuestion === '')
            this.questionModel.question.questionDetail = trimedQuestion;
             
        this.isCkeditorDirtly = true;
    }
    /**
     * Gets all the coding languages
     */
    private getCodingLanguage() {
        this.questionsService.getCodingLanguage().subscribe((response) => {
            this.codingLanguageList = response;
            this.codingLanguageList.sort();

            if (this.isQuestionEdited || this.isQuestionDuplicated) {
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
            if (this.selectedCategoryName === undefined && this.selectedDifficultyLevel === undefined) {
                this.selectedCategory = 'AllCategory';
                this.selectedDifficultyLevel = 'All';
            }
            this.showPreSelectedCategoryAndDifficultyLevel(this.selectedCategory, this.selectedDifficultyLevel);
            //If question is being editted then set the category
            if (this.isQuestionEdited || this.isQuestionDuplicated)
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
    private openSnackBar(message: string, enableRouting: boolean = false, routeTo: (string | number)[] = ['']) {
        let snackBarAction = this.snackBar.open(message, 'Dismiss', {
            duration: 3000
        });
        if (enableRouting) {
            this.router.navigate(routeTo);
        }
    }

    private isTestCaseValid() {
        return this.testCases.some(testcase => +testcase.testCaseType === TestCaseType.Default);
    }

    /**
     * Sends post request to add code snippet question
     * @param isCodeSnippetFormValid : Validation status of code snippet form
     */
    addCodingQuestion(isCodeSnippetFormValid: boolean) {
        this.isDefaultTestCaseAdded = this.isTestCaseValid();

        if (isCodeSnippetFormValid && !this.nolanguageSelected && this.isDefaultTestCaseAdded) {
            //Lock the form. Load spinner.
            this.isFormSubmitted = true;
            this.questionModel.question.questionType = QuestionType.codeSnippetQuestion;
            //Explicitly converting the id of the testcases to zero
            if (!this.isQuestionEdited || this.isQuestionDuplicated) {
                this.testCases.forEach(x => x.id = 0);
                this.questionModel.question.id = 0;
            }
            this.questionModel.codeSnippetQuestion.codeSnippetQuestionTestCases = this.testCases;
            this.questionModel.codeSnippetQuestion.languageList = [];
            this.selectedLanguageList.forEach(language => {
                this.questionModel.codeSnippetQuestion.languageList.push(language);
            });

            let subscription = this.isQuestionEdited
                ? this.questionsService.updateQuestionById(this.questionId, this.questionModel)
                : this.questionsService.addCodingQuestion(this.questionModel);

            subscription.subscribe(
                (response) => {
                    this.routeToDashboard = ['questions', this.selectedCategory, this.selectedDifficulty];
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

    showPreSelectedCategoryAndDifficultyLevel(categoryName: string, difficultyLevel: string) {
        if (categoryName !== 'AllCategory' && difficultyLevel !== 'All') {
            this.isCategorySelected = true;
            this.selectedDifficulty = difficultyLevel;
            this.questionModel.question.difficultyLevel = DifficultyLevel[this.selectedDifficulty];
            this.questionModel.question.categoryID = this.categoryList.find(x => x.categoryName === this.selectedCategory).id;
        }
        else if (categoryName === 'AllCategory' && difficultyLevel !== 'All') {
            this.isCategorySelected = false;
            this.selectedDifficulty = difficultyLevel;
            this.questionModel.question.difficultyLevel = DifficultyLevel[this.selectedDifficulty];
        }
        else if (categoryName !== 'AllCategory' && difficultyLevel === 'All') {
            this.isCategorySelected = true;
            this.questionModel.question.categoryID = this.categoryList.find(x => x.categoryName === this.selectedCategory).id;
        }
    }

}

class FormControlModel {
    showTestCase: boolean;
    collapseTestCase: boolean;
    showNewTestCase: boolean;
}

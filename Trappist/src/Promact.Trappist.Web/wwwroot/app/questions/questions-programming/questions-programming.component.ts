import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionModel } from '../question.application.model';
import { DifficultyLevel } from '../enum-difficultylevel';
import { CodingLanguageModel } from '../coding-language-model';

@Component({
    moduleId: module.id,
    selector: 'questions-programming',
    templateUrl: 'questions-programming.html'
})

export class QuestionsProgrammingComponent implements OnInit {

    selectedLanguageList: CodingLanguageModel[];
    codingLanguageList: CodingLanguageModel[];
    categoryList: Category[];
    questionModel: QuestionModel;

    nolanguageSelected: boolean;
    showTestCase: boolean;
    collapseTestCase: boolean;
    collapseNewTestCase: boolean;
    showNewTestCase: boolean;
    code: any;

    constructor(private questionsService: QuestionsService, private categoryService: CategoryService) {
        this.nolanguageSelected = true;
        this.selectedLanguageList = new Array<CodingLanguageModel>();
        this.codingLanguageList = new Array<CodingLanguageModel>();
        this.categoryList = new Array<Category>();
        this.questionModel = new QuestionModel();
    }

    ngOnInit() {
        this.getCodingLanguage();
        this.getCategory();
    }

    /**
     * Gets all the coding languages
     */
    private getCodingLanguage() {
        this.questionsService.getCodingLanguage().subscribe((response) => {
            this.codingLanguageList = response;
            this.sortCodingLanguage();
        });
    }

    /**
     * Gets all the categories
     */
    private getCategory() {
        this.categoryService.getAllCategories().subscribe((response) => {
            this.categoryList = response;
        });
    }

    /**
     * Adds language to the selectedLanguageList
     * @param language : language to add
     */
    selectLanguage(language: CodingLanguageModel) {
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
    removeLanguage(language: CodingLanguageModel) {
        this.selectedLanguageList.splice(this.selectedLanguageList.indexOf(language), 1);
        this.codingLanguageList.push(language);
        this.checkIfNoLanguageSelected();
        this.sortCodingLanguage();
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
        this.questionModel.question.category = this.categoryList.find(x => x.categoryName === category);
    }

    /**
     * Adds difficulty to the Question
     * @param difficulty
     */
    selectDifficulty(difficulty: string) {
        this.questionModel.question.difficultyLevel = DifficultyLevel[difficulty];
    }

    /**
     * Sorts codingLanguageList
     */
    private sortCodingLanguage() {
        this.codingLanguageList.sort((a, b) => a.languageCode - b.languageCode);
    }

    /**
     * Sends post request to add code snippet question
     * @param codeSnippetQuestionForm : code snippet form of type ngForm
     */
    onSubmit(codeSnippetQuestionForm: NgForm) {
        if (codeSnippetQuestionForm.valid && !this.nolanguageSelected) {
            this.questionModel.question.questionType = 2; // QuestionType 2 for programming question
            this.selectedLanguageList.forEach(language => {
                this.questionModel.codeSnippetQuestion.languageList.push(language.languageCode);
            });
            this.questionsService.postCodingQuestion(this.questionModel).subscribe((response) => {
            });
        }
    }
}

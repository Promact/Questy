import { Component, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { Category } from '../category.model';
import { QuestionModel } from '../question.application.model';
import { DifficultyLevel } from '../enum-difficultylevel';

@Component({
    moduleId: module.id,
    selector: 'questions-programming',
    templateUrl: 'questions-programming.html'
})

export class QuestionsProgrammingComponent implements OnInit {

    selectedLanguageList: string[];
    codingLanguageList: string[];
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
        this.selectedLanguageList = new Array<string>();
        this.codingLanguageList = new Array<string>();
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
            this.codingLanguageList.sort();
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
    selectLanguage(language: string) {
        let index = this.selectedLanguageList.indexOf(language);
        if (index === -1) {
            this.selectedLanguageList.push(language);
            this.codingLanguageList.splice(this.codingLanguageList.indexOf(language), 1);
            this.questionModel.codeSnippetQuestion.languageList = this.selectedLanguageList;
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
     * Sends post request to add code snippet question
     * @param codeSnippetQuestionForm : code snippet form of type ngForm
     */
    onSubmit(codeSnippetQuestionForm: NgForm) {
        if (codeSnippetQuestionForm.valid && !this.nolanguageSelected) {
            this.questionModel.question.questionType = 2; // QuestionType 2 for programming question
            this.questionsService.postCodingQuestion(this.questionModel).subscribe((response) => {
            });
        }
    }
}

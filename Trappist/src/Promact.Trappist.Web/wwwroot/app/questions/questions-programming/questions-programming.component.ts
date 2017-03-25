import { Component, OnInit, ViewChild } from "@angular/core";

@Component({
    moduleId: module.id,
    selector: 'questions-programming',
    templateUrl: 'questions-programming.html'
})

export class QuestionsProgrammingComponent {

    selectedLanguageList: CodingLanguage[] = new Array<CodingLanguage>();
    codingLanguageList: CodingLanguage[] = new Array<CodingLanguage>();
    categoryList: Category[] = new Array<Category>();
    question: Question = new Question();

    difficulty: string;

    nolanguageSelected: boolean;

    code: any;

    constructor(private questionsService: QuestionsService, private categoryService: CategoryService) {
        this.getCodingLanguage();
        this.getCategory();
    }

    /**
     * Gets all the coding languages
     */
    getCodingLanguage() {
        this.questionsService.getCodingLanguage().subscribe((response) => {
            this.codingLanguageList = response;
        });
    }

    /**
     * Gets all the categories
     */
    getCategory() {
        this.categoryService.getAllCategories().subscribe((response) => {
            this.categoryList = response;
        });
    }

    /**
     * Adds language to the selectedLanguageList
     * @param language : language to add
     */
    selectLanguage(language: CodingLanguage) {
        let index = this.selectedLanguageList.indexOf(language);
        if (index === -1) {
            this.selectedLanguageList.push(language);
            this.codingLanguageList.splice(this.codingLanguageList.indexOf(language), 1);
            this.question.codeSnippetQuestion.languageList = this.selectedLanguageList;
        }
        this.checkIfNoLanguageSelected();
    }

    /**
     * Removes language from selectedLanguageList 
     * @param language : Language to remove
     */
    removeLanguage(language: CodingLanguage) {
        this.selectedLanguageList.splice(this.selectedLanguageList.indexOf(language), 1);
        this.codingLanguageList.push(language);
        this.checkIfNoLanguageSelected();
    }

    /**
     * Toggles nolanguageSelected according to selectedLanguageList 
     */
    checkIfNoLanguageSelected() {
        this.nolanguageSelected = this.selectedLanguageList.length === 0 ? true : false;
    }

    /**
     * Adds category to the Question 
     * @param category : category to be added
     */
    selectCategory(category: Category) {
        this.question.category = category;
    }

    /**
     * Adds difficulty to the Question
     * @param difficulty
     */
    selectDifficulty(difficulty: string) {
        this.question.difficultyLevel = DifficultyLevel[difficulty];
    }

    /**
     * Sends post request to add code snippet question
     * @param codeSnippetForm : code snippet form of type ngForm
     */
    onSubmit(codeSnippetForm: NgForm) {
        if (codeSnippetForm.valid && !this.nolanguageSelected) {
            console.log(this.question);
            //To-Do call questionservice function to post question
        }
    }
}

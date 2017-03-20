import { Component, OnInit, ViewChild } from "@angular/core";
import { CodingLanguage } from "../coding.language.enum";
import { CategoryService } from "../categories.service";
import { QuestionsService } from "../questions.service";
import { ProgrammingQuestion } from "../question.programming.model";
import { DifficultyLevel } from "../difficulty.level.enum";

@Component({
    moduleId: module.id,
    selector: "questions-programming",
    templateUrl: "questions-programming.html"
})

export class QuestionsProgrammingComponent {

    codingLanguages: string[] = ['Java', 'C++', 'C'];
    selectedCodingLanguages: string[] = new Array<string>();
    categories: string[] = new Array<string>();

    languageInputText: string = "";
    selectedDifficultyLevel: string;
    questionDetail: string;
    selectedCategory: string;
    allContentReady: boolean = false;

    checkCodeComplexity: boolean = false;
    checkTimeComplexity: boolean = false;
    runBasicTestCase: boolean = false;
    runCornerTestCase: boolean = false;
    runNecessaryTestCase: boolean = false;

    constructor(private categoryService: CategoryService, private programmingQuestion: ProgrammingQuestion, private questionService: QuestionsService) {
        this.codingLanguages.sort();
        this.getCategories();
    }

    getCategories() {
        this.categoryService.getAllCategories().subscribe((response) => {
            this.categories = response;
            this.selectedCategory = this.categories[0];
            this.allContentReady = true;
        });
    }

    selectLanguage(language: string) {
        var index = this.codingLanguages.findIndex(x => x.toLowerCase() === language.toLowerCase());
        if (index != -1) {
            this.selectedCodingLanguages.push(this.codingLanguages[index]);
            this.codingLanguages.splice(index, 1);
            this.languageInputText = null;
        }
    }

    removeSelectedLanguage(selectedLanguage: string) {
        this.codingLanguages.push(selectedLanguage);
        this.selectedCodingLanguages.splice(this.selectedCodingLanguages.indexOf(selectedLanguage), 1);
        this.codingLanguages.sort();
    }

    convertLanguageStringToEnum() {
        var languageList: CodingLanguage[] = new Array<CodingLanguage>();

        this.selectedCodingLanguages.forEach((x) => languageList.push(CodingLanguage[x]));

        console.log(languageList);
        return languageList;
    }

    submitForm() {
       this.programmingQuestion.questionDetail = this.questionDetail;
        this.programmingQuestion.questionType = 2; //Type = programming question 
        this.programmingQuestion.difficultyLevel = DifficultyLevel[this.selectedDifficultyLevel];
        this.programmingQuestion.createdBy = "USER"; //To-Do Add current user name
        this.programmingQuestion.categoryId = 1; //To-Do Add category Id from category class when available
        this.programmingQuestion.checkCodeComplexity = this.checkCodeComplexity;
        this.programmingQuestion.checkCodeComplexity = this.checkTimeComplexity;
        this.programmingQuestion.runBasicTestCase = this.runBasicTestCase;
        this.programmingQuestion.runCornerTestCase = this.runCornerTestCase;
        this.programmingQuestion.runNecessaryTestCase = this.runNecessaryTestCase;
        this.programmingQuestion.languageList = this.convertLanguageStringToEnum();

        this.questionService.postCodeSnippetQuestion(this.programmingQuestion).subscribe((response) => {
            if (response == this.programmingQuestion) {
                console.log("Added successfully");
            }
         });
    }
}

import { Component, OnInit, ViewChild } from "@angular/core";
import { CodingLanguage } from "../coding.language.enum";
import { CategoryService } from "../categories.service";
import { QuestionsService } from "../questions.service";
import { ProgrammingQuestion } from "../question.programming.model";
import { DifficultyLevel } from "../difficulty.level.enum";
import { Category } from "../category.model";

@Component({
    moduleId: module.id,
    selector: "questions-programming",
    templateUrl: "questions-programming.html"
})

export class QuestionsProgrammingComponent {

    codingLanguages: string[] = ['Java', 'C++', 'C'];
    selectedCodingLanguages: string[] = new Array<string>();
    categories: Category[] = new Array<Category>();

    languageInputText: string = "";
    selectedDifficultyLevel: string;
    questionDetail: string;
    selectedCategory: Category;
    allContentReady: boolean = false;
    noLanguageSelected: boolean = false;

    checkCodeComplexity: boolean = false;
    checkTimeComplexity: boolean = false;
    runBasicTestCase: boolean = false;
    runCornerTestCase: boolean = false;
    runNecessaryTestCase: boolean = false;

    constructor(private categoryService: CategoryService, private programmingQuestion: ProgrammingQuestion, private questionService: QuestionsService) {
        this.codingLanguages.sort();
        this.getCategories();
        this.selectedCategory = new Category();
        this.selectedDifficultyLevel = "Easy";
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
            this.validate();
        }
    }

    removeSelectedLanguage(selectedLanguage: string) {
        this.codingLanguages.push(selectedLanguage);
        this.selectedCodingLanguages.splice(this.selectedCodingLanguages.indexOf(selectedLanguage), 1);
        this.codingLanguages.sort();
    }

    selectCategory(category: string) {
        this.selectedCategory = this.categories.find((x) => x.categoryName === category);
    }

    convertLanguageStringToEnum() {
        var languageList: CodingLanguage[] = new Array<CodingLanguage>();
        this.selectedCodingLanguages.forEach((x) => {
            languageList.push(CodingLanguage[x]);
        });
        return languageList;
    }

    submitForm() {
        if (this.validate()) {
            //Map question
            this.programmingQuestion.questionDetail = this.questionDetail;
            this.programmingQuestion.questionType = 2; //Type = programming question 
            this.programmingQuestion.difficultyLevel = DifficultyLevel[this.selectedDifficultyLevel];
            this.programmingQuestion.createdBy = "USER"; //To-Do Add current user name
            this.programmingQuestion.categoryId = this.selectedCategory.id;
            this.programmingQuestion.checkCodeComplexity = this.checkCodeComplexity;
            this.programmingQuestion.checkCodeComplexity = this.checkTimeComplexity;
            this.programmingQuestion.runBasicTestCase = this.runBasicTestCase;
            this.programmingQuestion.runCornerTestCase = this.runCornerTestCase;
            this.programmingQuestion.runNecessaryTestCase = this.runNecessaryTestCase;
            this.programmingQuestion.languageList = this.convertLanguageStringToEnum();

            this.questionService.postCodeSnippetQuestion(this.programmingQuestion).subscribe((response) => {
                if (response != null || typeof response != 'undefined') {
                    //To-Do redirect to another page
                    console.log("Added successful");
                }
            });
        }
    }

    validate() {
        if (this.selectedCodingLanguages.length == 0) {
            this.noLanguageSelected = true;

            let element = document.getElementById('codingLanguage');
            element.scrollIntoView(true);

            return false;
        }
        this.noLanguageSelected = false;
        return true;
    }
}

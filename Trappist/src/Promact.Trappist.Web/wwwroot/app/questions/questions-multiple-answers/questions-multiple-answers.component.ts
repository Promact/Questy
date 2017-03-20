import { Component, OnInit, ViewChild } from "@angular/core";
import { CategoryService } from "../categories.service";
import { Category } from "../category.model";
import { SingleMultipleAnswerQuestionOption } from "../options.model";
import { SingleMultipleQuestion } from "../single-multiple-question";
import { QuestionsService } from "../questions.service";

@Component({
    moduleId: module.id,
    selector: "questions-multiple-answers",
    templateUrl: "questions-multiple-answers.html"
})

export class QuestionsMultipleAnswersComponent {
    display: boolean = false;
    isOptionSelected: boolean = true;
    isCategorySelected: boolean = true;
    isDifficultyLevelSelected: boolean = true;
    categoryName: string[] = new Array<string>();
    questionType: string[] = ["Easy", "Medium", "Hard"];
    multipleAnswerQuestion: SingleMultipleQuestion = new SingleMultipleQuestion();
    constructor(private categoryService: CategoryService, private questionService: QuestionsService) {
        this.getAllCategories();
        this.multipleAnswerQuestion = new SingleMultipleQuestion();
        this.multipleAnswerQuestion.singleMultipleAnswerQuestion.questionType = 1;
        for (var i = 0; i < 4; i++) {
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
        }
    }

    //Return category list
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryName = CategoriesList;

        });
    }

    //Validate options,category and difficulty level selection
    validate() {
        this.display = true;
        if (this.multipleAnswerQuestion.singleMultipleAnswerQuestion.category.categoryName == undefined) {
            this.isCategorySelected = false;
        }
        if (this.multipleAnswerQuestion.singleMultipleAnswerQuestion.difficultyLevel == undefined) {
            this.isDifficultyLevelSelected = false;
        }

    }
    multipleQuestionAnswerAdd(multipleAnswerQuestion: SingleMultipleQuestion) {
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).subscribe((response) => {
            if (response.ok) {

            }
        });
    }
}

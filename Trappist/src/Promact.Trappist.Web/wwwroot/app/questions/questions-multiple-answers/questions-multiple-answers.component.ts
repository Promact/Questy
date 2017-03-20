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
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryName = CategoriesList;

        });
    }
    multipleQuestionAnswerAdd(multipleAnswerQuestion: SingleMultipleQuestion) {
        console.log(multipleAnswerQuestion);
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).subscribe((response) => {
            if (response.ok) {

            }
        });
    }
}

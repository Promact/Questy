import { Component, OnInit, ViewChild } from "@angular/core";
import { CategoryService } from "../categories.service";
import { Category } from "../category.model";
import { SingleMultipleAnswerQuestionOption } from "../options.model";
import { SingleMultipleQuestion } from "../single-multiple-question";
import { QuestionsService } from "../questions.service";
@Component({
    moduleId: module.id,
    selector: "questions-single-answer",
    templateUrl: "questions-single-answer.html"
})

export class QuestionsSingleAnswerComponent {
    categoryName: string[] = new Array<string>();
    questionType: string[] = ["Easy","Medium","Hard"];
    singleAnswerQuestion: SingleMultipleQuestion = new SingleMultipleQuestion();
    constructor(private categoryService: CategoryService, private questionService: QuestionsService) {
            this.getAllCategories();      
            this.singleAnswerQuestion = new SingleMultipleQuestion();
            this.singleAnswerQuestion.singleMultipleAnswerQuestion.questionType = 0;
            for (var i = 0; i < 4; i++)
            {
                this.singleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
                this.singleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
            }         
    }
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryName = CategoriesList;

        });
    }
    SingleQuestionAnswerAdd(singleAnswerQuestion: SingleMultipleQuestion) {
        this.questionService.addSingleAnswerQuestion(singleAnswerQuestion).subscribe((response) => {
            if (response.ok) {
                
                              }  
                        });     
}
}
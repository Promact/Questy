import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";


@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {

    constructor(private questionsService: QuestionsService) {
        this.getAllQuestions();
    }

    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            console.log(questionsList);
        });
    }

}
import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { Question } from "./question.model";
import { SingleMultipleQuestion } from "./single-multiple-question";
@Injectable()
export class QuestionsService {

    private singleMultipleQuestionApiUrl = "api/singlemultiplequestion";
    private questionsApiUrl = "api/question";
    constructor(private httpService: HttpService) {
        
    }

    //add single answer question
    addSingleAnswerQuestion(singleAnswerQuestion: SingleMultipleQuestion) {
        console.log(singleAnswerQuestion);
        return this.httpService.post(this.singleMultipleQuestionApiUrl, singleAnswerQuestion);

    }
    //get list of questions
    getQuestions() {
            return this.httpService.get(this.questionsApiUrl);
        }
}
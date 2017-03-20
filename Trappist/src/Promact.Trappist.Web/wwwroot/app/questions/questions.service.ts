import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { Question } from "./question.model";
import { SingleMultipleQuestion } from "./single-multiple-question";
@Injectable()
export class QuestionsService {

    private questionApiUrl = "api/singlemultiplequestion";

    constructor(private httpService: HttpService) {
        
    }

    //add single answer question
    addSingleAnswerQuestion(singleAnswerQuestion:SingleMultipleQuestion) {
        console.log(singleAnswerQuestion);
        return this.httpService.post(this.questionApiUrl, singleAnswerQuestion);
    
} 

}
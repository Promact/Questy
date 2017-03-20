import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { ProgrammingQuestion } from "./question.programming.model";

@Injectable()
export class QuestionsService {

    private questionsApiUrl = "api";

    constructor(private httpService: HttpService) {
        
    }

    /**
     * get list of questions
     */
    getQuestions() {
        return this.httpService.get(this.questionsApiUrl+"/question");
    }

    postCodeSnippetQuestion(codeSnippetQuesion: ProgrammingQuestion) {
        return this.httpService.post(this.questionsApiUrl + "/codesnippetquestion", codeSnippetQuesion);
    }
}
import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
@Injectable()
export class QuestionsService {
    private questionsApiUrl = "api/question";
    constructor(private httpService: HttpService) {}
    /**
     * get list of questions
     */
    getQuestions() {
        return this.httpService.get(this.questionsApiUrl);
    }
}
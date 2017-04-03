import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
@Injectable()
export class QuestionsService {
    private questionsApiUrl = 'api/question';
    constructor(private httpService: HttpService) {}
    /**
     *To get list of Questions
     */
    getQuestions() {
        return this.httpService.get(this.questionsApiUrl);
    }
}
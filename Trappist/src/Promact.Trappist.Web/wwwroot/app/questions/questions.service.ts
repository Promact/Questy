﻿import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Question } from './question.model';
import { Questions } from './question';

@Injectable()
export class QuestionsService {
    private questionsApiUrl = 'api/question';

    constructor(private httpService: HttpService) {
        
    }

    /**
     * Add question
     * @param question
     */
    addSingleAnswerQuestion(question: Questions) {
        console.log(question);
        return this.httpService.post(this.questionsApiUrl, question);
    }

    /**
     *To get list of Questions
     */
    getQuestions() {
         return this.httpService.get(this.questionsApiUrl);
    }
    
}
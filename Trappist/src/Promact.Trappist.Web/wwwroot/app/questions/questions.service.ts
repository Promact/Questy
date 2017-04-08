import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Question } from './question.model';
import { QuestionBase } from './question';

@Injectable()
export class QuestionsService {
    private questionsApiUrl = 'api/question';

    constructor(private httpService: HttpService) {

    }

    /**
     * Add single multiple answer question
     * @param question
     */
    addSingleMultipleAnswerQuestion(question: QuestionBase) {
        return this.httpService.post(this.questionsApiUrl, question);
    }

    /**
     *To get list of Questions
     */
    getQuestions() {
        return this.httpService.get(this.questionsApiUrl);
    }

    /**
     * gets list of coding languages
     */
    getCodingLanguage() {
        return this.httpService.get(this.questionsApiUrl + '/codinglanguage');
    }

    /**
     * Calls API to post code snippet question
     * @param question: QuestionBase class object 
     */
    addCodingQuestion(question: QuestionBase) {
        return this.httpService.post(this.questionsApiUrl, question);
    }

    /**
     * API to delete Question
     * @param id:Id to delete Question
     */
    deleteQuestion(id: number) {
        return this.httpService.delete(this.questionsApiUrl + '/' + id);
    }
}
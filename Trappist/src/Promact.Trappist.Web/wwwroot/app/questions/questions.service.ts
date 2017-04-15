import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Question } from './question.model';
import { QuestionBase } from './question';
import { QuestionDisplay } from '../questions/question-display';

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
     * Update single multiple answer question
     * @param question
     */
    updateSingleMultipleAnswerQuestion(questionId: number, question: QuestionBase) {
        return this.httpService.put(this.questionsApiUrl + '/' + questionId, question);
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
     * Gets Question base on Question id
     * @param id
     */
    getQuestionById(id: number) {
        return this.httpService.get(this.questionsApiUrl + '/' + id);
    }
}
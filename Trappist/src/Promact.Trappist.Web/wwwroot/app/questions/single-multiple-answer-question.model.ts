import { SingleMultipleAnswerQuestionOption } from '../questions/single-multiple-answer-question-option.model';
export class SingleMultipleAnswerQuestion {
    singleMultipleAnswerQuestionOption: SingleMultipleAnswerQuestionOption[];
    id: number;
    constructor() {
        this.singleMultipleAnswerQuestionOption = new Array<SingleMultipleAnswerQuestionOption>();
    }
}
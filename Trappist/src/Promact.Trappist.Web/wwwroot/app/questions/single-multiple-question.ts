import { DifficultyLevel } from './enum-difficultylevel';
import { QuestionType } from './enum-questiontype';
import { Category } from './category.model';
import { SingleMultipleAnswerQuestionOption } from './single-multiple-answer-question-option.model';

export class SingleMultipleAnswerQuestion {
    id: number;
    singleMultipleAnswerQuestionOption: Array<SingleMultipleAnswerQuestionOption>;

    constructor() {
        this.singleMultipleAnswerQuestionOption = new Array<SingleMultipleAnswerQuestionOption>();
    }
}
import { DifficultyLevel } from './enum-difficultylevel';
import { QuestionType } from './enum-questiontype';
import { Category } from './category.model';
import { SingleMultipleAnswerQuestionOption } from './single-multiple-answer-question-option.model';
import { SingleMultipleAnswerQuestion } from './single-mutiple-answer-question.model';

export class SingleMultipleQuestion {
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
    singleMultipleAnswerQuestionOption: Array<SingleMultipleAnswerQuestionOption>;
    constructor() {
        this.singleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion();
        this.singleMultipleAnswerQuestionOption = new Array<SingleMultipleAnswerQuestionOption>();
    }
}
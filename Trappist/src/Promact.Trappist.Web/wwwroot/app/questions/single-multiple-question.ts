import { DifficultyLevel } from "./enum-difficultylevel";
import { QuestionType } from "./enum-questiontype";
import { Category } from "./category.model";
import { SingleMultipleAnswerQuestionOption } from "./single-multiple-question-option.model";
import { SingleMultipleAnswerQuestion } from "./single-mutiple-question-answer.model";
export class SingleMultipleQuestion {
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion();
    singleMultipleAnswerQuestionOption: Array<SingleMultipleAnswerQuestionOption> = new Array<SingleMultipleAnswerQuestionOption>();

}
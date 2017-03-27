import { DifficultyLevel } from '../questions/enum-difficultylevel';
import { QuestionType } from '../questions/enum-questiontype';
//import { SingleMultipleAnswerQuestion } from '../questions/single-multiple-answer-question.model';
import { Category } from '../questions/category.model';
import { CodeSnippetQuestion } from './code.snippet.model';
export class Question {
    id: number;
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: DifficultyLevel;
    category: Category;
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
    codeSnippetQuestion: CodeSnippetQuestion;
}
//import { SingleMultipleAnswerQuestion } from '../questions/single-multiple-answer-question.model';
class SingleMultipleAnswerQuestionOption {
    option: string;
    isAnswer: boolean;
}
//import { Question } from '../questions/question.model';
//import { SingleMultipleAnswerQuestionOption } from '../questions/single-multiple-answer-question-option.model';
class SingleMultipleAnswerQuestion {
    id: number;
    singleMultipleAnswerQuestionOption: SingleMultipleAnswerQuestionOption[] = new Array<SingleMultipleAnswerQuestionOption>();
}
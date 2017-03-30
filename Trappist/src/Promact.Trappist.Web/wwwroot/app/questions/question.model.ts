import { DifficultyLevel } from '../questions/enum-difficultylevel';
import { QuestionType } from '../questions/enum-questiontype';
import { Category } from '../questions/category.model';
import { SingleMultipleAnswerQuestion } from '../questions/single-multiple-answer-question.model';
import { CodeSnippetQuestion } from './code.snippet.model';
export class Question {
    id: number;
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: DifficultyLevel;
    categoryID: number;
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
    codeSnippetQuestion: CodeSnippetQuestion;
    constructor() {
        this.singleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion();
        this.codeSnippetQuestion = new CodeSnippetQuestion();
    }
}

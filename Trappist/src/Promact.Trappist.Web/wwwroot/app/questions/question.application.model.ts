import { Question } from './question.model';
import { SingleMultipleAnswerQuestion } from './single-multiple-question';
import { CodeSnippetQuestion } from './code.snippet.model';
export class QuestionModel {
    question: Question;
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
    codeSnippetQuestion: CodeSnippetQuestion;

    constructor() {
        this.question = new Question();
        this.singleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion();
        this.codeSnippetQuestion = new CodeSnippetQuestion();
    }
}
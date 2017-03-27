import { Question } from './question.model';
import { SingleMultipleAnswerQuestionAC } from './single-multiple-question';
import { CodeSnippetQuestion } from './code.snippet.model';
export class QuestionModel {
    question: Question;
    singleMultipleAnswerQuestionAC: SingleMultipleAnswerQuestionAC;
    codeSnippetQuestion: CodeSnippetQuestion;

    constructor() {
        this.question = new Question();
        this.singleMultipleAnswerQuestionAC = new SingleMultipleAnswerQuestionAC();
        this.codeSnippetQuestion = new CodeSnippetQuestion();
    }
}
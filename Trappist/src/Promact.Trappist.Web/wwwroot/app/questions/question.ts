import { Question } from './question.model';
import { SingleMultipleQuestion } from './single-multiple-question';
import { CodeSnippetAC } from './code-snippet';

export class QuestionBase {
    question: Question;
    singleMultipleAnswerQuestion: SingleMultipleQuestion;
    codeSnippetQuestion: CodeSnippetAC;
    constructor() {
        this.question = new Question();
        this.singleMultipleAnswerQuestion = new SingleMultipleQuestion();
    }
}
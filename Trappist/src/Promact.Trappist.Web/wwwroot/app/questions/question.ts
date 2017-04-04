import { Question } from './question.model';
import { SingleMultipleAnswerQuestion } from './single-multiple-question';
import { CodeSnippetAC } from './code-snippet';

export class QuestionBase {
    question: Question;
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
    codeSnippetQuestion: CodeSnippetAC;

    constructor() {
        this.question = new Question();
        this.singleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion();
    }
}
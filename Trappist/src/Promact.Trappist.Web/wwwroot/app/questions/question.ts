import { Question } from './question.model';
import { SingleMultipleQuestion } from './single-multiple-question';
import { CodeSnippetAC } from './code-snippet';

export class Questions {
    question: Question = new Question();
    singleMultipleAnswerQuestionAC: SingleMultipleQuestion = new SingleMultipleQuestion();
    codeSnippetQuestion: CodeSnippetAC = null;
}
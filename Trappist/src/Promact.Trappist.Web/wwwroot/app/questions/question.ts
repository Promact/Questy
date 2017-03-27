import { Question } from './question.model';
import { SingleMultipleAnswerQuestionAC } from './single-multiple-question';
import { CodeSnippetAC } from './code-snippet';

export class QuestionAC {
    question: Question = new Question();
    singleMultipleAnswerQuestionAC: SingleMultipleAnswerQuestionAC = new SingleMultipleAnswerQuestionAC();
    codeSnippetQuestion: CodeSnippetAC = null;
}
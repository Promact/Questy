import { QuestionBase } from '../questions/question';
import { QuestionStatus } from './question_status.enum';
export class TestQuestions {
    question: QuestionBase;
    questionStatus: QuestionStatus;

    constructor() {
        this.question = new QuestionBase();
    }
}
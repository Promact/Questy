import { QuestionStatus } from './question_status.enum';
import { Code } from './code.model';
export class TestAnswer {
    questionId: number;
    optionChoice: number[];
    code: Code;
    questionStatus: QuestionStatus;

    constructor() {
        this.optionChoice = new Array<number>();
        this.code = new Code();
    }    
}
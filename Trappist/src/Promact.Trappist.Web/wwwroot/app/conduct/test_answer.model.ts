import { QuestionStatus } from './question_status.enum';
export class TestAnswer {
    questionId: number;
    optionChoice: number[];
    code: string;
    questionStatus: QuestionStatus;

    constructor() {
        this.optionChoice = new Array<number>();
    }    
}
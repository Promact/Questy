import { BrowserTolerance } from '../tests/enum-browsertolerance';

export class TestInstructions {
    public browserTolerance: BrowserTolerance;
    public correctMarks: number;
    public incorrectMarks: number;
    public totalNumberOfQuestions: number;
    public categoryNameList: string[];

    constructor() {
        this.categoryNameList = new Array<string>();
    }
}
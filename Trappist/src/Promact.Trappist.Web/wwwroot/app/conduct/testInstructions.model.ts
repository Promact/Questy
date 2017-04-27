
export class TestInstructions {
    public duration: number;
    public correctMarks: number;
    public incorrectMarks: number;
    public totalNumberOfQuestions: number;
    public categoryNameList: string[];

    constructor() {
        this.categoryNameList = new Array<string>();
    }
}
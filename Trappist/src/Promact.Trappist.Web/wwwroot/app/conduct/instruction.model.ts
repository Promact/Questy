export class Instruction {
    public duration: number;
    public warningTime: number;
    public correctMarks: number;
    public incorrectMarks: number;
    public totalNumberOfQuestions: number;
    public categoryNameList: string[];

    constructor() {
        this.categoryNameList = new Array<string>();
    }
}
export class CodeResponse {
    message: string;
    error: string;
    errorOccurred: boolean;
    output: string;
    timeConsumed: number;
    memoryConsumed: number;
    totalTestCasePassed: number;
    totalTestCases: number;

    constructor() {
        this.totalTestCasePassed = 0;
        this.totalTestCases = 0;
    }
}
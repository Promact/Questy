export class CodeSnippetQuestion {
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: number[];

    constructor() {
        this.languageList = new Array<number>();
    }
}
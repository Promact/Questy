export class CodeSnippetQuestion {
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: string[];

    constructor() {
        this.languageList = new Array<string>();
    }
}
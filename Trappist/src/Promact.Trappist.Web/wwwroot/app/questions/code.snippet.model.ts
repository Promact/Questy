import { CodeSnippetQuestionsTestCases } from '../questions/code-snippet-questions-test-cases.model'
export class CodeSnippetQuestion {
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: string[];
    testCases: CodeSnippetQuestionsTestCases[];

    constructor() {
        this.languageList = new Array<string>();
        this.testCases = new Array<CodeSnippetQuestionsTestCases>();
    }
}
import { CodeSnippetQuestionsTestCases } from '../questions/code-snippet-questions-test-cases.model';

export class CodeSnippetQuestion {
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: string[];
    codeSnippetQuestionTestCases: CodeSnippetQuestionsTestCases[];

    constructor() {
        this.languageList = new Array<string>();
        this.codeSnippetQuestionTestCases = new Array<CodeSnippetQuestionsTestCases>();
    }
}
import { TestCaseType } from '../questions/enum-test-case-type';

export class CodeSnippetQuestionsTestCases {
    id: number;
    testCaseTitle: string;
    testCaseDescription: string;
    testCaseType: TestCaseType;
    testCaseInput: string;
    testCaseOutput: string;
    testCaseMarks: any;
}
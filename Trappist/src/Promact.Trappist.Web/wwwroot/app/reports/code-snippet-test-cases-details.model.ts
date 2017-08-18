import { TestCaseType } from '../questions/enum-test-case-type';
import { ProgrammingLanguage } from './programminglanguage.enum';

export class CodeSnippetTestCasesDetails {
    testCaseName: string;
    testCaseType: TestCaseType;
    testCaseInput: string;
    expectedOutput: string;
    testCaseMarks: number;
    processing: number;
    memory: number;
    actualOutput: string;
    isTestCasePassing: boolean;
}
import { CodingLanguage } from './coding.language.model';
export class CodeSnippetQuestion {
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: CodingLanguage[];
}
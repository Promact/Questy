import { CodingLanguage } from "./coding.language.enum";

export class ProgrammingQuestion {
    questionDetail: string;
    questionType: number;
    difficultyLevel: number;
    createdBy: string;
    categoryId: number;
    checkCodeComplexity: boolean;
    checkTimeComplexity: boolean;
    runBasicTestCase: boolean;
    runCornerTestCase: boolean;
    runNecessaryTestCase: boolean;
    languageList: CodingLanguage[] = new Array<CodingLanguage>();
}
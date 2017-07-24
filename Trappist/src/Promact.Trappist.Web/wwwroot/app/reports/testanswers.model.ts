import { TestConduct } from './testConduct.model';

export class TestAnswers {
    id: number;
    testConductId: number;
    answeredCodeSnippet: string;
    answeredOption: number;
    testCoduct: TestConduct;
}
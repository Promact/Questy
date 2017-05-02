import { TestOrder } from './enum-testorder';
import { Category } from '../questions/category.model';
import { BrowserTolerance } from './enum-browsertolerance';
import { AllowTestResume } from './enum-allowtestresume';

export class Test {
    id: number;
    testName: string;
    link: string;
    startDate: Date;
    endDate: Date;
    duration: number;
    warningTime: number;
    fromIpAddress: string;
    toIpAddress: string;
    warningMessage: string;
    correctMarks: string;
    incorrectMarks: string;
    browserTolerance: number;
    createdDateTime: Date;
    questionOrder: TestOrder;
    optionOrder: TestOrder;
    allowTestResume: AllowTestResume;
    categoryAcList: Category[] = [];
    isEditTestEnabled: boolean;
    isQuestionMissing: boolean;
    public isPaused: boolean;
    public isLaunched: boolean;
    numberOfTestAttendees: number;
    numberOfTestSections: number;
    numberOfTestQuestions: number;
}

export class TestCategory {
    public id: number;
    public categoryId: number;
    public testId: number;
}

export class TestQuestion {
    public id: number;
    public testId: number;
    public questionId: number;    
    public isSelect: boolean;
}




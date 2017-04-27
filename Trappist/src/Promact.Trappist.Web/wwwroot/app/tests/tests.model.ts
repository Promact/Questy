import { TestOrder } from './enum-testorder';
import { Category } from '../questions/category.model';
import { BrowserTolerance } from './enum-browsertolerance';

export class Test {
    public id: number;
    public testName: string;
    public link: string;
    public startDate: Date;
    public endDate: Date;
    public duration: number;
    public warningTime: number;
    public fromIpAddress: string;
    public toIpAddress: string;
    public warningMessage: string;
    public correctMarks: string;
    public incorrectMarks: string;
    public browserTolerance: BrowserTolerance;
    public questionOrder: TestOrder;
    public optionOrder: TestOrder;
    categoryAcList: Category[] = [];
    public numberOfTestAttendees: number;
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




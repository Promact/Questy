import { Category } from '../questions/category.model';
import { Question } from '../questions/question.model';

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
    public browserTolerance: number;
}

export class TestCategory {
    public id: number;
    public categoryId: number;
    public testId: number;
    public test: Test;
    public category: Category;
}


export class TestQuestion {
    public id: number;
    public cateId: number;
    public test: Test;
    public testId: number;
    public question: Question;
    public questionId: number;
    public testCategory: TestCategory;
    public testCategoryId: number;
    public isSelect: boolean;
}

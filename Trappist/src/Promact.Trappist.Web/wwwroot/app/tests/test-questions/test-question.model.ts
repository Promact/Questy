import { Test } from '../tests.model';
import { Question } from '../../questions/question.model';
import { TestCategory } from '../test-sections/test-category.model';


export class TestQuestion {  
    id: number;
    test: Test;
    testId: number;
    question: Question;
    questionId: number;
    testCategory: TestCategory;
    testCategoryId: number;    
}
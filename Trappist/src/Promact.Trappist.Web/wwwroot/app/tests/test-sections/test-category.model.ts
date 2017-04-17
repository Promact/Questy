import { Test } from '../tests.model';
import { Category } from '../../questions/category.model';

export class TestCategory {
    id: number;
    test: Test;
    testId: number;
    category: Category;
    categoryId: number; 
}
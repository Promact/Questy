import { Category } from '../questions/category.model';
import { TestCategory } from '../tests/tests.model';

export class TestDetails {
    id: number;
    testName: string;
    category: Category[] = [];
}
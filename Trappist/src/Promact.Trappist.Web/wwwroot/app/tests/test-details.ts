import { Category } from '../questions/category.model';

export class TestDetails {
    id: number;
    testName: string;
    categoryAcList: Category[] = [];
}
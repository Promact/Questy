import { Category } from '../questions/category.model';
import { Question } from '../questions/question.model';
import { QuestionBase } from '../questions/question';

export class TestDetails {
    id: number;
    testName: string;
    categoryACList: Category[] = [];
    questionAC: QuestionBase[] = [];
}
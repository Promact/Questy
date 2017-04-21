import { QuestionBase } from '../questions/question';
export class Category {
        id: number;
        categoryName: string;
        isSelect: boolean;
        questionList: QuestionBase[];
} 
import { Question } from '../questions/question.model';


export class Category {
        id: number;
        categoryName: string;
        isSelect: boolean;
        questions: Question[] = [];
} 
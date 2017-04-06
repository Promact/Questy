import { Question } from './question.model';
import { QuestionBase } from './question';

export class Category {
        id: number;
        categoryName: string; 
        question: QuestionBase[] = [];
        isactive: boolean = false;
        isState: boolean;
        selectAll: boolean;
        numberOfQuestion: number;
} 
﻿import { Question } from './question.model';
import { QuestionBase } from './question';

export class Category {
        id: number;
        categoryName: string; 
        questionList: QuestionBase[] = [];
        isAccordionOpen: boolean = false;
        isAlreadyClicked: boolean;
        selectAll: boolean;
        numberOfQuestion: number;
        isSelect: boolean;
} 
import { Category } from '../questions/category.model';
import { QuestionType } from '../questions/enum-questiontype';
import { DifficultyLevel } from '../questions/enum-difficultylevel';
import { SingleMultipleAnswerQuestion } from '../questions/single-multiple-question';
export class QuestionDisplay {
    id: number;
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: DifficultyLevel;
    category: Category;
    singleMultipleAnswerQuestion: SingleMultipleAnswerQuestion;
}
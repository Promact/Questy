import { DifficultyLevel } from "./enum-difficultylevel";
import { QuestionType } from "./enum-questiontype";
import { Category } from "./category.model";
export class SingleMultipleAnswerQuestion {
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: DifficultyLevel;
    category: Category = new Category();
}
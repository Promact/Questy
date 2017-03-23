import { DifficultyLevel } from "../questions/enum-difficultylevel"
import { QuestionType } from "../questions/enum-questiontype"
import { Option } from "../questions/option.model";
import{Category} from "../questions/category.model"
export class Question {
    id: number;
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: DifficultyLevel;
    category: Category;
    singleMultipleAnswerQuestionOption: Option[] = new Array<Option>();
}
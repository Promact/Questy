import { DifficultyLevel } from "../questions/enum-difficultylevel"
import { QuestionType } from "../questions/enum-questiontype"
import { Option } from "../questions/option.model";
import{Category} from "../questions/category.model"
export class Question {
    Id: number;
    questionDetail: string;
    questionType: QuestionType;
    difficultyLevel: number;
    createdBy: string;
    updatedBy: string;
    category: Category;
    singleMultipleAnswerQuestionOption: Option[] = new Array<Option>();

}
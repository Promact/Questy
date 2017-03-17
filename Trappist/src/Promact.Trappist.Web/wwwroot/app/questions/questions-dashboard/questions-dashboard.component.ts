import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Question } from "../../questions/question.model"
import { DifficultyLevel } from "../../questions/enum-difficultylevel"
import { QuestionType } from "../../questions/enum-questiontype"
@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {

    questionDisplay: Question[] = new Array<Question>();
    categoryName: string[] = new Array<string>();
     //To enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    //To enable enum questiontype in template 
    QuestionType = QuestionType;

    alpha: string[] = ["a","b","c","d","e","..."];
    
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {
        this.getAllQuestions();
		this.getAllCategories();
    }
    isCorrectAnswer(isAnswer: boolean)
    {
        if (isAnswer)
        {
            return "correct";
        }
    }
	//To Get All The categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryName = CategoriesList;
        });
    }

    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            this.questionDisplay = questionsList;
        });
    }

    // Open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }

    // Open Delete Category Dialog
    deleteCategoryDialog() {
      this.dialog.open(DeleteCategoryDialogComponent);
    }

}

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent { }
export class Category {
    CategoryName: string;
}

@Component({
  moduleId: module.id,
  selector: 'delete-category-dialog',
  templateUrl: "delete-category-dialog.html"
})
export class DeleteCategoryDialogComponent { }

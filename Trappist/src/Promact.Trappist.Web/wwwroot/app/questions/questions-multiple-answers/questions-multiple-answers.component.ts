import { Component, OnInit, ViewChild } from "@angular/core";
import { MdSnackBar } from '@angular/material';
import { CategoryService } from "../categories.service";
import { Category } from "../category.model";
import { Router } from '@angular/router';
import { SingleMultipleAnswerQuestionOption } from "../single-multiple-question-option.model";
import { SingleMultipleQuestion } from "../single-multiple-question";
import { QuestionsService } from "../questions.service";

@Component({
    moduleId: module.id,
    selector: "questions-multiple-answers",
    templateUrl: "questions-multiple-answers.html"
})

export class QuestionsMultipleAnswersComponent {
    display: boolean = false;
    isOptionSelected: boolean = true;
    isCategorySelected: boolean = true;
    isDifficultyLevelSelected: boolean = true;
    questionType: string[]=new Array<string>();
    categoryArray: Category[] = new Array<Category>();
    
    multipleAnswerQuestion: SingleMultipleQuestion = new SingleMultipleQuestion();
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {
        this.getAllCategories();
        this.multipleAnswerQuestion = new SingleMultipleQuestion();
        this.multipleAnswerQuestion.singleMultipleAnswerQuestion.questionType = 1;
        for (var i = 0; i < 4; i++) {
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
        }
    }

    //Return category list
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
            this.questionType=["Easy", "Medium", "Hard"];
        });
    }

    //Validate options,category and difficulty level selection
    validate() {
        this.display = true;
        if (this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer == false &&
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[1].isAnswer == false &&
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[2].isAnswer == false &&
            this.multipleAnswerQuestion.singleMultipleAnswerQuestionOption[3].isAnswer == false)
        {
            this.isOptionSelected = false;
        }
        if (this.multipleAnswerQuestion.singleMultipleAnswerQuestion.category.categoryName == undefined) {
            this.isCategorySelected = false;
        }
        if (this.multipleAnswerQuestion.singleMultipleAnswerQuestion.difficultyLevel == undefined) {
            this.isDifficultyLevelSelected = false;
        }

    }
    multipleQuestionAnswerAdd(multipleAnswerQuestion: SingleMultipleQuestion) {
        this.questionService.addSingleAnswerQuestion(multipleAnswerQuestion).subscribe((response) => {
            if (response.ok) {

            }
        });
        let snackBarRef = this.snackBar.open('Saved Changes Successfully', 'Dismiss', {
            duration: 3000,
        });
        snackBarRef.afterDismissed().subscribe(() => {
            this.router.navigate(['/questions']);

        });     
    }
}

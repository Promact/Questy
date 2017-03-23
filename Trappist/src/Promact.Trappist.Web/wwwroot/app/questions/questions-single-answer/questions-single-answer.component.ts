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
    selector: "questions-single-answer",
    templateUrl: "questions-single-answer.html"
})

export class QuestionsSingleAnswerComponent {
    value: number=5;
    display: boolean = false;
    isOptionSelected: boolean = true;
    isCategorySelected: boolean = true;
    isDifficultyLevelSelected: boolean = true;
    categoryArray: Category[] = new Array<Category>();
    questionType: string[] = new Array<string>();
    singleAnswerQuestion: SingleMultipleQuestion = new SingleMultipleQuestion();
    constructor(private categoryService: CategoryService, private questionService: QuestionsService, private router: Router, public snackBar: MdSnackBar) {
            this.questionType = ["Easy", "Medium", "Hard"];  
            this.getAllCategories();               
            this.singleAnswerQuestion = new SingleMultipleQuestion();
            this.singleAnswerQuestion.singleMultipleAnswerQuestion.questionType = 0;
            for (var i = 0; i < 4; i++)
            {
                this.singleAnswerQuestion.singleMultipleAnswerQuestionOption.push(new SingleMultipleAnswerQuestionOption);
                this.singleAnswerQuestion.singleMultipleAnswerQuestionOption[i].isAnswer = false;
            }         
    }

    //Return category list
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
            
        });
    }

    //Validate options,category and difficulty level selection
    validate()
    {
        this.display = true;
     
        if (this.value == 5)
        {
            this.isOptionSelected = false;
        }
        if (this.singleAnswerQuestion.singleMultipleAnswerQuestion.category.categoryName == undefined)
        {
            this.isCategorySelected = false;
        }
        if (this.singleAnswerQuestion.singleMultipleAnswerQuestion.difficultyLevel == undefined)
        {
            this.isDifficultyLevelSelected = false;
        }

    }

    SingleQuestionAnswerAdd(singleAnswerQuestion: SingleMultipleQuestion)
    {
       
        this.singleAnswerQuestion.singleMultipleAnswerQuestionOption[this.value].isAnswer = true;
        this.questionService.addSingleAnswerQuestion(singleAnswerQuestion).subscribe((response) => {
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
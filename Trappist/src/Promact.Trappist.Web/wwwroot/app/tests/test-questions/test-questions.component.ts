import { Component, OnInit } from '@angular/core';
import { Question } from '../../questions/question.model';
import { Category } from '../../questions/category.model';
import { TestService } from '../tests.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { TestQuestion } from '../test-questions/test-question.model';
import { TestCategory } from '../test-sections/test-category.model';
import { TestDetails } from '../test';
import { QuestionBase } from '../../questions/question';
import { QuestionType } from '../../questions/enum-questiontype';
@Component({
    moduleId: module.id,
    selector: 'test-questions',
    templateUrl: 'test-questions.html'
})

export class TestQuestionsComponent implements OnInit {
    editName: boolean;
    categories: Category[] = [];
    questions: QuestionBase[][] = [];
    DifficultyLevel = DifficultyLevel;
    QuestionType = QuestionType;
    selectedQuestions: number[] = [];
    questionsToAdd: QuestionBase[] = [];
    testId: number;
    testDetails: TestDetails;
    optionName: string[] = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];

    constructor(private testService: TestService, public snackBar: MdSnackBar, public router: ActivatedRoute, public route: Router) {
        this.testDetails = new TestDetails();
    }
    ngOnInit() {
        this.router.params.subscribe(params => {
            this.testId = params['id'];
        });
        this.getTestDetails();
    }

    openSnackBar(text: string) {
        this.snackBar.open(text, 'Dismiss', {
            duration: 5000
        });
    }

    getAllquestions(category: Category, i: number) {
        if (!category.isactive) {
            category.isactive = true;
            if (!category.isState) {
                category.isState = true;
                this.testService.getQuestions(this.testDetails.id, category.id).subscribe(response => {
                    this.testDetails.categoryACList[i].question = response;
                    this.selectedQuestions[i] = this.testDetails.categoryACList[i].question.length;
                    this.testDetails.categoryACList[i].numberOfQuestion = this.testDetails.categoryACList[i].question.filter(function (x) {
                        return x.question.isSelect;
                    }).length;
                    console.log(this.testDetails);

                });
            } else category.isState = true;
        } else category.isactive = false;
    }
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }

    getTestDetails() {
        this.testService.getTestDetails(this.testId).subscribe(response => {
            this.testDetails = response;
        });
    }
    selectQuestion(question: QuestionBase, category: Category) {
      
        if (question.question.isSelect) {
            if (category.question.every(function (x) {
                return x.question.isSelect;
            }))
                category.selectAll = true;
            category.numberOfQuestion++;
        } else {
            category.selectAll = false;
            question.question.isSelect = false;
            category.numberOfQuestion--;
        }
    }
    AddTestQuestion() {

        this.questionsToAdd = new Array<QuestionBase>();
        for (let category of this.testDetails.categoryACList) {
            this.questionsToAdd = this.questionsToAdd.concat(category.question);
            category.question.filter(function (x) {
                return x.question.isSelect;
            });
        }

        this.testService.addTestQuestions(this.questionsToAdd, this.testId).subscribe(response => {
            if (response)
                this.openSnackBar(response.message);
        },
            error => {
                this.openSnackBar('Oops! something went wrong..please try after sometimes');
            });
    }

    SelectAll(category: Category, selectedQuestions: number) {
       
        category.question.map(function (question) {
            if (category.selectAll) {
                question.question.isSelect = true;
                category.numberOfQuestion = selectedQuestions;
            }
            else {
                question.question.isSelect = false;
                category.numberOfQuestion = 0;
            }    
        });        
    }
    SaveExit() {
        this.AddTestQuestion();
        this.route.navigate(['/tests']);
    }
}

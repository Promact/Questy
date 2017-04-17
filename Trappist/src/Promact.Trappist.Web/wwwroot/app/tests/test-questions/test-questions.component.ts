import { Component, OnInit } from '@angular/core';
import { Question } from '../../questions/question.model';
import { Category } from '../../questions/category.model';
import { TestService } from '../tests.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { TestDetails } from '../test';
import { QuestionBase } from '../../questions/question';
import { QuestionType } from '../../questions/enum-questiontype';
import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'test-questions',
    templateUrl: 'test-questions.html'
})

export class TestQuestionsComponent implements OnInit {
    editName: boolean;
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
        if (!category.isAccordionOpen) {
            category.isAccordionOpen = true;
            if (!category.isAlreadyClicked) {
                category.isAlreadyClicked = true;
                this.testService.getQuestions(this.testDetails.id, category.id).subscribe(response => {
                    this.testDetails.categoryACList[i].questionList = response;
                    this.selectedQuestions[i] = this.testDetails.categoryACList[i].questionList.length;
                    this.testDetails.categoryACList[i].numberOfQuestion = this.testDetails.categoryACList[i].questionList.filter(function (question) {
                        return question.question.isSelect;
                    }).length;
                });
            } else category.isAlreadyClicked = true;
        } else category.isAccordionOpen = false;
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
            if (category.questionList.every(function (question) {
                return question.question.isSelect;
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
            this.questionsToAdd = this.questionsToAdd.concat(category.questionList);
            category.questionList.filter(function (question) {
                return question.question.isSelect;
            });
        }

        this.testService.addTestQuestions(this.questionsToAdd, this.testId).subscribe(response => {
            if (response)
                this.openSnackBar(response.message);
        },
            error => {
                this.openSnackBar('Oops! something went wrong..please try after sometime');
            });
    }

    SelectAll(category: Category, selectedQuestions: number) {

        category.questionList.map(function (questionList) {
            if (category.selectAll) {
                questionList.question.isSelect = true;
                category.numberOfQuestion = selectedQuestions;
            }
            else {
                questionList.question.isSelect = false;
                category.numberOfQuestion = 0;
            }
        });
    }
    SaveExit() {
        this.AddTestQuestion();
        this.route.navigate(['/tests']);
    }
export class TestQuestionsComponent implements OnInit {
    testSettings: Test;
    testId: number;
    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
    }

    /**
    * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
    */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestById(this.testId);
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestById(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testSettings = (response);
        });
    }
    
}

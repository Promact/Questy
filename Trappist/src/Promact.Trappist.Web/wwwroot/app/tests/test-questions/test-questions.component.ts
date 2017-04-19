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
import { Test } from '../tests.model';

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
    testSettings: Test;
    testId: number;
    testDetails: TestDetails;
    optionName: string[] = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];

    constructor(private testService: TestService, public snackBar: MdSnackBar, public router: ActivatedRoute, public route: Router) {
        this.testDetails = new TestDetails();
        this.testSettings = new Test();
    }
    ngOnInit() {
        this.router.params.subscribe(params => {
            this.testId = params['id'];
        });
        this.getTestDetails();
        this.getTestById(this.testId);
    }

    openSnackBar(text: string) {
        this.snackBar.open(text, 'Dismiss', {
            duration: 5000
        });
    }
    /**
     * Gets all the questions of particular category by passing its Id
     * @param category
     * @param i is index of category
     */
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
    /**
     * Gets the details of a test by passing its Id
     */
    getTestDetails() {
        this.testService.getTestDetails(this.testId).subscribe(response => {
            this.testDetails = response;
        }); 
    }

    /**
     * Selects questions from the list of questions of a particular category
     * @param question is an object of QuestionBase
     * @param category is an object of Category
     */
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
  
   
    /**
     * Adds all the questions to to database and navigate to test-settings.component
     */
    saveNext() {
        this.loader = true;
        this.questionsToAdd = new Array<QuestionBase>();
        //It checks for every category of a test
        for (let category of this.testDetails.categoryAcList) {
            //If question list of a category is not null
            if (category.isSelect && category.questionList !== null)
                //Every question from category are concatenated to single array which will be sent to add to test
                this.questionsToAdd = this.questionsToAdd.concat(category.questionList);
        }
        this.testService.addTestQuestions(this.questionsToAdd, this.testId).subscribe(response => {
            if (response) {
                this.openSnackBar(response.message);
                if (this.isSaveExit) {
                    this.loader = false;
                    this.route.navigate(['/tests']);
                }
                else
                    this.loader = false;
                this.route.navigate(['tests/' + this.testId + '/settings']);
            }
        },
            error => {
                this.loader = false;
                this.openSnackBar('Oops! something went wrong..please try after sometime');
            });
    }

    /**
     * Selects and deselect all the questions of a category
     * @param category object
     * @param totalNumberOfQuestions number of all the questions of a category
     */
    selectAll(category: Category, totalNumberOfQuestions: number) {
        category.questionList.map(function (questionList) {
            //If selectAll checkbox is selected
            if (category.selectAll) {
                //every question is selected
                questionList.question.isSelect = true;
                category.numberOfSelectedQuestion = totalNumberOfQuestions;
            }
            else {
                //If selectAll checkbox is unselected ,then every question is deselected
                questionList.question.isSelect = false;
                category.numberOfSelectedQuestion = 0;
            }
        });
    }

    /**
     * Adds the questions to question table and redirect to test dashboard
     */
    saveExit() {
        this.isSaveExit = true;
        this.saveNext();
    }
}

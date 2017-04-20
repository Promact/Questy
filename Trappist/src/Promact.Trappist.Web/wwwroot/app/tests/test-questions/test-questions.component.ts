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
                    this.testDetails.categoryAcList[i].questionList = response;
                    this.selectedQuestions[i] = this.testDetails.categoryAcList[i].questionList.length;
                    this.testDetails.categoryAcList[i].numberOfQuestion = this.testDetails.categoryAcList[i].questionList.filter(function (question) {
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
     * Adds all the questions to to database and navigate to test-view.component
     */
    SaveNext() {

        this.questionsToAdd = new Array<QuestionBase>();
        
        for (let category of this.testDetails.categoryAcList) {
            if (category.isSelect && category.questionList !== null) 
                this.questionsToAdd = this.questionsToAdd.concat(category.questionList);
         }
              
        this.testService.addTestQuestions(this.questionsToAdd, this.testId).subscribe(response => {
            if (response)
                this.openSnackBar(response.message);
        },
            error => {
                this.openSnackBar('Oops! something went wrong..please try after sometime');
            });
        this.route.navigate(['tests/' + this.testId + '/view']);
    }
    /**
     * Selects and deselect all the questions of a category
     * @param category object
     * @param totalNumberOfQuestions number of all the questions of a category
     */
    SelectAll(category: Category, totalNumberOfQuestions: number) {

        category.questionList.map(function (questionList) {
            if (category.selectAll) {
                questionList.question.isSelect = true;
                category.numberOfQuestion = totalNumberOfQuestions;
            }
            else {
                questionList.question.isSelect = false;
                category.numberOfQuestion = 0;
            }
        });
    }
    /**
     * Adds the questions to question table and redirect to test dashboard
     */

    SaveExit() {
        this.SaveNext();
        this.route.navigate(['/tests']);
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

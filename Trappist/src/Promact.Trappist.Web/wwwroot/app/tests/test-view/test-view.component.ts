import { Component, OnInit } from '@angular/core';
import { MdDialog } from '@angular/material';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { DeleteTestDialogComponent } from '../tests-dashboard/delete-test-dialog.component';
import { Question } from '../../questions/question.model';
import { Category } from '../../questions/category.model';
import { TestService } from '../tests.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { Test } from '../tests.model';
import { QuestionBase } from '../../questions/question';
import { QuestionType } from '../../questions/enum-questiontype';
import { TestAttendees } from '../../conduct/register/register.model';

@Component({
    moduleId: module.id,
    selector: 'test-view',
    templateUrl: 'test-view.html'
})

export class TestViewComponent implements OnInit {
    testDetails: Test;
    testId: number;
    totalNumberOfQuestions: number[] = [];
    QuestionType = QuestionType;
    DifficultyLevel = DifficultyLevel;
    optionName: string[] = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
    isDeleteAllowed: boolean;
    Tests: Test[] = new Array<Test>();

    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute) {
        this.testDetails = new Test();
    }

    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestDetails(this.testId);
        this.getAllTests();
    }

    // get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => { this.Tests = (response); });
    }

    /**
     * Gets the details of the Test by Id
     * @param id contains the value of the Id from the route
     */
    getTestDetails(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testDetails = response;
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
            if (!category.isAlreadyClicked) {//If Accordion is already clicked then it wont call the server next time it is clicked,so that user can not lose its selected questions
                category.isAlreadyClicked = true;
                this.testService.getQuestions(this.testDetails.id, category.id).subscribe(response => {
                    this.testDetails.categoryAcList[i].questionList = response;//gets the total number of questions of particular category
                    this.totalNumberOfQuestions[i] = this.testDetails.categoryAcList[i].questionList.length;
                    this.testDetails.categoryAcList[i].numberOfSelectedQuestion = this.testDetails.categoryAcList[i].questionList.filter(function (question) {
                        return question.question.isSelect;
                    }).length;
                    category.selectAll = category.questionList.every(function (question) {
                        return question.question.isSelect;
                    });

                });
            }
            else {
                category.isAlreadyClicked = true;
            }
        }
        else {
            category.isAccordionOpen = false;
        }
    }

    /**
     * returns 'correct' class for correct option
     * @param isAnswer
     */
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }

    /**
     * Launches the Test Dialog Box
     */
    launchTestDialog() {
        let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.testSettingObject = this.testDetails;
    }

    /**
     * Redirects to the Test Sections Page from the Test View Page
     */
    navigateToTestSections() {
        this.router.navigate(['/tests', this.testId, 'sections']);
    }

    /**
     * Redirects to the Test Settings Page from the Test View Page
     */
    navigateToTestSettings() {
        this.router.navigate(['/tests', this.testId, 'settings']);
    }

    // Open Delete Test Dialog
    deleteTestDialog(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((response) => {
            this.isDeleteAllowed = response.response ? false : true;
            let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent).componentInstance;
            deleteTestDialog.testToDelete = test;
            deleteTestDialog.testArray = this.Tests;
            deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
        });
    }
}

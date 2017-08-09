﻿import { Component, OnInit } from '@angular/core';
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
import { DuplicateTestDialogComponent } from '../tests-dashboard/duplicate-test-dialog.component';
import { IncompleteTestCreationDialogComponent } from '../test-settings/incomplete-test-creation-dialog.component';
import { PopoverModule } from 'ngx-popover';


@Component({
    moduleId: module.id,
    selector: 'test-view',
    templateUrl: 'test-view.html'
})

export class TestViewComponent implements OnInit {
    testDetails: Test;
    testId: number;
    totalNumberOfQuestions: number[];
    questionType = QuestionType;
    difficultyLevel = DifficultyLevel;
    optionName: string[];
    isDeleteAllowed: boolean;
    tests: Test[];
    isEditTestEnabled: boolean;
    loader: boolean;

    copiedContent: boolean;
    testLink: string;
    tooltipMessage: string;
    isCategorySelected: boolean;
    disablePreview: boolean;

    constructor(public dialog: MdDialog, private testService: TestService, public snackBar: MdSnackBar, private router: Router, private route: ActivatedRoute) {
        this.testDetails = new Test();
        this.tests = new Array<Test>();
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
        this.totalNumberOfQuestions = [];

        this.tooltipMessage = 'Copy to Clipboard';
        this.isCategorySelected = false;
        this.disablePreview = false;
    }

    ngOnInit() {
        this.loader = true;
        this.testId = this.route.snapshot.params['id'];
        this.getTestDetails(this.testId);
        this.getAllTests();
    }

    // get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => { this.tests = (response); });
        this.isTestAttendeeExist();
    }
    openSnackBar(text: string) {
        this.snackBar.open(text, 'Dismiss', {
            duration: 3000
        });
    }

    openNewWindowForPreview() {
        window.open('/tests/' + this.testDetails.link + '/preview', 'preview', 'height=' + screen.height + ',width=' + screen.width + ',scrollbars=0, status = 0, titlebar = 0, toolbar = 0, resizable = 1, location = 0');
    }

    /**
     * Gets the details of the Test by Id
     * @param id contains the value of the Id from the route
     */
    getTestDetails(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testDetails = response;
            this.isCategorySelected = this.testDetails.categoryAcList.some(function (x) {
                return x.isSelect;
            });
            this.disablePreview = this.testDetails.categoryAcList === null || this.testDetails.categoryAcList.every(x => !x.isSelect) || this.testDetails.categoryAcList.every(x => x.numberOfSelectedQuestion === 0) || !this.testDetails.isLaunched;
            let linkOfTest = response.link;
            let domain = window.location.origin;
            this.testLink = domain + '/conduct/' + linkOfTest;
            this.testDetails.categoryAcList.forEach(x => {
                if (x.numberOfSelectedQuestion === 0)
                    x.isQuestionAbsent = true;
                else
                    x.isQuestionAbsent = false;
            });
            this.loader = false;
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
                    this.testDetails.categoryAcList[i].questionList = this.testDetails.categoryAcList[i].questionList.filter(function (x) {
                        return x.question.isSelect;
                    });
                    this.totalNumberOfQuestions[i] = this.testDetails.categoryAcList[i].questionList.length;
                    this.testDetails.categoryAcList[i].numberOfSelectedQuestion = this.testDetails.categoryAcList[i].questionList.length;
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
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        let isCategoryAdded = this.testDetails.categoryAcList.some(function (x) {
            return x.isSelect;
        });
        if (isCategoryAdded) {
            let isQuestionAdded = this.testDetails.categoryAcList.some(function (x) {
                return (x.numberOfSelectedQuestion !== 0);
            });
            if (isQuestionAdded) {
                let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
                instance.testSettingObject = testObject;
            }
            else {
                this.testDetails.isQuestionMissing = true;
                let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails });
            }
        }
        else {
            this.testDetails.isQuestionMissing = false;
            let dialogRef = this.dialog.open(IncompleteTestCreationDialogComponent, { data: this.testDetails });
        }
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
        this.testService.isTestAttendeeExist(test.id).subscribe((res) => {
            this.isDeleteAllowed = res.response ? false : true;
            let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent).componentInstance;
            deleteTestDialog.testToDelete = test;
            deleteTestDialog.testArray = this.tests;
            deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
        });
    }

    /**
     * Checks if any candidate has taken the test
     */
    isTestAttendeeExist() {
        this.testService.isTestAttendeeExist(this.testId).subscribe((res) => {
            if (res.response) {
                this.isEditTestEnabled = false;
            }
            else {
                this.isEditTestEnabled = true;
            }
        });
    }





    /**
     * Open duplicate test dialog
     * @param test: an object of Test class
     */
    duplicateTestDialog(test: Test) {
        let newTestObject = (JSON.parse(JSON.stringify(test)));
        let duplicateTestDialog = this.dialog.open(DuplicateTestDialogComponent).componentInstance;
        duplicateTestDialog.testName = newTestObject.testName + '_copy';
        duplicateTestDialog.testArray = this.tests;
        duplicateTestDialog.testToDuplicate = test;
    }

    /**
     * Navigates the user to the select sections page 
     * @param test: an object of Test class
     */
    editTest(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((res) => {
            if (!res.response) {
                this.router.navigate(['/tests/' + test.id + '/sections']);
            }
        });
    }

    /**
   * Displays the tooltip message
   * @param $event is of type Event and is used to call stopPropagation()
   */
    showTooltipMessage($event: Event, testLink: any) {
        $event.stopPropagation();
        setTimeout(() => {
            testLink.select();
        }, 0);
        this.tooltipMessage = 'Copied';

    }

    /**
     * Changes the tooltip message
     */
    changeTooltipMessage() {
        this.tooltipMessage = 'Copy to Clipboard';
    }
}

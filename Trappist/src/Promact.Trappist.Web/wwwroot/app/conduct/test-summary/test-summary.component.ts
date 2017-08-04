import { Component, OnInit } from '@angular/core';
import { ConductService } from '../conduct.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TestSummary } from '../testsummary.model';
import { TestStatus } from "../../reports/enum-test-state";
import { TestAttendee } from "../../reports/testattendee.model";
import { Test } from "../../tests/tests.model";
import { TestAnswer } from '../test_answer.model';
import { QuestionStatus } from '../question_status.enum';

@Component({
    moduleId: module.id,
    selector: 'test-summary',
    templateUrl: 'test-summary.html',
})
export class TestSummaryComponent implements OnInit {
    magicString: string;
    testSummaryObject: TestSummary;
    timeLeft: number;
    totalQuestionsInTest: number;
    numberOfAttemptedQuestions: number;
    numberOfUnAttemptedQuestions: number;
    numberOfReviewedQuestions: number;
    resumeTypeOfTest: number;
    timeLeftInHours: number;
    timeLeftInMinutes: number;
    timeLeftInSeconds: number;
    timeLeftInHoursVisible: boolean;
    timeLeftInMinutesVisible: boolean;
    timeLeftInSecondsVisible: boolean;
    isButtonVisible: boolean;
    testAttendee: TestAttendee;
    test: Test;
    testAnswers: TestAnswer[];
    testAnswer: TestAnswer;
    countA: number;
    countR: number;
    countU: number;

    constructor(private conductService: ConductService, private route: ActivatedRoute, private router: Router) {
        this.testSummaryObject = new TestSummary();
        this.testAttendee = new TestAttendee();
        this.test = new Test();
        this.testAnswers = new Array<TestAnswer>();
        this.testAnswer = new TestAnswer();
        this.countA = 0;
        this.countR = 0;
        this.countU = 0;
    }

    ngOnInit() {
        let url = window.location.pathname;
        this.magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test-summary'));
        this.getTestDetails(this.magicString);
    }

    getTestSummaryDetailsByLink(testLink: string) {
        this.conductService.getTestSummary(testLink).subscribe((response) => {
            this.testSummaryObject = response;
            this.timeLeft = this.testSummaryObject.timeLeft;
            this.totalQuestionsInTest = this.testSummaryObject.totalNumberOfQuestions;
            this.numberOfAttemptedQuestions = this.testSummaryObject.attemptedQuestions;
            this.numberOfUnAttemptedQuestions = this.testSummaryObject.unAttemptedQuestions;
            this.numberOfReviewedQuestions = this.testSummaryObject.reviewedQuestions;
            this.resumeTypeOfTest = this.testSummaryObject.resumeType;
            this.isButtonVisible = this.resumeTypeOfTest === 0 ? false : true;
            this.timeLeftInHours = Math.floor(this.timeLeft / 3600);
            this.timeLeftInHoursVisible = this.timeLeftInHours < 1 ? false : true;
            this.timeLeftInMinutes = Math.floor(this.timeLeft / 60);
            this.timeLeftInMinutesVisible = this.timeLeftInMinutes < 1 ? false : true;
            this.timeLeftInSeconds = Math.floor(this.timeLeft % 60);
            this.timeLeftInSecondsVisible = this.timeLeftInSeconds < 1 ? false : true;
        });
    }

    getTestDetails(testLink: string) {
        this.conductService.getTestByLink(testLink).subscribe((response1) => {
            this.test = response1;

            this.getTestAttendee(this.test.id);
            this.totalQuestionsInTest = this.test.numberOfTestQuestions;

        });
    }

    getUnSupervisedSummaryDetailsByLink(testAttendeeId: number) {
        this.conductService.getAnswer(testAttendeeId).subscribe((response) => {
            this.testAnswers = response;
            this.testAnswers.forEach(x => {
                let answeredQuestions = this.testAnswers.filter(x => x.questionStatus === QuestionStatus.answered);
                this.numberOfAttemptedQuestions = answeredQuestions.length;
                let reviewedQuestions = this.testAnswers.filter(x => x.questionStatus === QuestionStatus.review);
                this.numberOfReviewedQuestions = reviewedQuestions.length;
                let unAnsweredQuestions = this.testAnswers.filter(x => x.questionStatus === QuestionStatus.unanswered);
                this.numberOfUnAttemptedQuestions = unAnsweredQuestions.length;
            });
        });
    }

    startTest() {
        let url = window.location.pathname;
        let testUrl = url.substring(0, url.indexOf('/test-summary')) + '/test';
        window.location.href = testUrl;

    }

    /**
     * Called when end test button is clicked
     */
    endTestButtonClicked() {
        this.endTest(TestStatus.completedTest);
    }

    ///**
    //* Gets Test Attendee
    //* @param testId: Id of Test
    //*/
    getTestAttendee(testId: number) {
        this.conductService.getTestAttendeeByTestId(testId).subscribe((response) => {
            this.testAttendee = response;
            if (this.test.allowTestResume === 0)
                this.getTestSummaryDetailsByLink(this.test.link);
            else
                this.getUnSupervisedSummaryDetailsByLink(this.testAttendee.id);
        }, err => {
            this.router.navigate([''])
        });
    }

    ///**
    //* Ends test and route to test-end page
    //* @param testStatus: TestStatus object
    //*/
    private endTest(testStatus: TestStatus) {
        if (this.isButtonVisible) {
            this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe(response => {
                this.router.navigate(['test-end']);
            });
        }
    }
}

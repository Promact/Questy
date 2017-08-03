import { Component, OnInit } from '@angular/core';
import { ConductService } from '../conduct.service';
import { ActivatedRoute,Router } from '@angular/router';
import { TestSummary } from '../testsummary.model';
import { TestStatus } from "../../reports/enum-test-state";
import { TestAttendee } from "../../reports/testattendee.model";
import { Test } from "../../tests/tests.model";

@Component({
    moduleId: module.id,
    selector: 'test-summary',
    templateUrl: 'test-summary.html',
})
export class TestSummaryComponent implements OnInit{
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

    constructor(private conductService: ConductService, private route: ActivatedRoute, private router: Router) {
        this.testSummaryObject = new TestSummary();
        this.testAttendee = new TestAttendee();
        this.test = new Test();
    }

    ngOnInit() {
        let url = window.location.pathname;
        this.magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test-summary'));
        this.getTestSummaryDetailsByLink(this.magicString);
       
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
          //  this.getTestDetails(this.magicString);
        });
      
    }

    getTestDetails(testLink: string) {
        this.conductService.getTestByLink(testLink).subscribe((response1) => {
            this.test = response1;
            this.getTestAttendee(this.test.id);
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
        this.router.navigate(['test-end']);
    }

    /**
    * Gets Test Attendee
    * @param testId: Id of Test
    */
    getTestAttendee(testId: number) {
        this.conductService.getTestAttendeeByTestId(testId).subscribe((response) => {
            this.testAttendee = response;
        });
    }

    /**
    * Ends test and route to test-end page
    * @param testStatus: TestStatus object
    */
    private endTest(testStatus: TestStatus) {
        if (this.isButtonVisible) {
            this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe(response => {
                this.router.navigate(['test-end']);
            });
        }
    }
}

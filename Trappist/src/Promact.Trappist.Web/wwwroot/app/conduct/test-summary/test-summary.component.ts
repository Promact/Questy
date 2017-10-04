import { Component, OnInit } from '@angular/core';
import { ConductService } from '../conduct.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TestStatus } from '../teststatus.enum';
import { TestAttendee } from '../../reports/testattendee.model';
import { Test } from '../../tests/tests.model';
import { TestAnswer } from '../test_answer.model';
import { QuestionStatus } from '../question_status.enum';
import { AllowTestResume } from '../../tests/enum-allowtestresume';
import { ReportService } from '../../reports/report.service';
import { MdSnackBar, MdSnackBarRef } from '@angular/material';
import { Observable, Subscription } from 'rxjs/Rx';
import { TestLogs } from '../../reports/testlogs.model';
import { ConnectionService } from '../../core/connection.service';
declare let screenfull: any;

@Component({
    moduleId: module.id,
    selector: 'test-summary',
    templateUrl: 'test-summary.html',
})

export class TestSummaryComponent implements OnInit {

    private TIMEOUT_TIME: number = 10;
    magicString: string;
    testSummaryObject: number;
    timeLeft: number;
    totalQuestionsInTest: number;
    numberOfAttemptedQuestions: number;
    numberOfUnAttemptedQuestions: number;
    numberOfReviewedQuestions: number;
    resumeTypeOfTest: number;
    timeLeftInHours: number;
    testLogs: TestLogs;
    timeLeftInMinutes: number;
    timeLeftInSeconds: number;
    timeLeftInHoursVisible: boolean;
    timeLeftInMinutesVisible: boolean;
    timeLeftInSecondsVisible: boolean;
    isButtonVisible: boolean;
    testAttendee: TestAttendee;
    test: Test;
    testAnswers: TestAnswer[];
    isTimeLeftZero: boolean;
    testDuration: number;
    loader: boolean;
    isTestPreview: boolean;
    timeString: string;
    timeOutCounter: number;
    clockInterval: NodeJS.Timer;
    isTestResume: boolean;
    isTestClosedUnConditionally: boolean;
    disableButton: boolean;
    isAllowed: boolean;
    isTestBlocked: boolean;
    isTestExpired: boolean;

    constructor(private conductService: ConductService, private route: ActivatedRoute, private router: Router, private reportService: ReportService, private snackbarRef: MdSnackBar, private connectionService: ConnectionService) {
        this.testAttendee = new TestAttendee();
        this.test = new Test();
        this.testAnswers = new Array<TestAnswer>();
        this.isTimeLeftZero = false;
        this.isTestPreview = false;
        this.timeOutCounter = 0;
        this.timeString = '00:00:00';
        this.numberOfAttemptedQuestions = 0;
        this.numberOfUnAttemptedQuestions = 0;
        this.numberOfReviewedQuestions = 0;
        window.onbeforeunload = undefined;
    }

    ngOnInit() {
        this.loader = true;
        let url = window.location.pathname;
        this.magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test-summary'));
        this.getTotalQuestions();
        this.getTestDetails(this.magicString);
        history.pushState(null, null, null);
        window.addEventListener('popstate', function (event) {
            history.pushState(null, null, null);
        });

        this.connectionService.recievedAttendee.subscribe(attendee => {
            this.testAttendee.report = attendee.report;
            this.isAllowed = this.testAttendee.report.isAllowResume;
        });
    }

    /**
     * Gets the total number of questions in a particular test
     */
    getTotalQuestions() {
        this.conductService.getTestSummary(this.magicString).subscribe((response) => {
            this.testSummaryObject = response;
            this.totalQuestionsInTest = this.testSummaryObject;
        });
    }

    /**
     * Gets the elapsed time of test to calculate the time left of the test
     * @param attendeeId is the Id of the attendee giving the selected test
     */
    timeLeftOfTest(attendeeId: number) {
        this.loader = true;
        this.conductService.getElapsedTime(this.testAttendee.id).subscribe(value => {
            let spanTimeInSeconds = value * 60;
            let durationInSeconds = this.test.duration * 60;
            this.timeLeft = durationInSeconds - spanTimeInSeconds;
            this.timeLeft = this.timeLeft < 0 ? 0 : Math.round(this.timeLeft);
            this.timeString = this.secToTimeString(this.timeLeft);
            if (!this.isTestClosedUnConditionally)//If test was unsupervised then tick the clock
                this.clockInterval = setInterval(() => { this.countDown(); this.timeOut(); }, 1000);
            this.loader = false;
        });
    }

    /**
     * Gets the details of the test by the test link
     * @param testLink contains the link of the test from the route
     */
    getTestDetails(testLink: string) {
        this.conductService.getTestForSummary(testLink).subscribe((response1) => {
            this.test = response1;
            this.isTestClosedUnConditionally = this.test.allowTestResume === AllowTestResume.Supervised;
            this.testDuration = this.test.duration;
            this.getTestAttendee(this.test.id);
        });
    }

    /**
     * Gets the details required for displaying the test summary
     * @param testAttendeeId contains the Id of the test attendee giving the test 
     */
    getSummaryDetailsByAttendeeId(testAttendeeId: number) {
        this.conductService.getAnswer(testAttendeeId).subscribe((response) => {
            this.testAnswers = response;
            let answeredQuestions = this.testAnswers.filter(e => e.questionStatus === QuestionStatus.answered);
            this.numberOfAttemptedQuestions = answeredQuestions.length;

            if (isNaN(this.numberOfAttemptedQuestions)) {
                this.numberOfAttemptedQuestions = 0;
            }

            let reviewedQuestions = this.testAnswers.filter(x => x.questionStatus === QuestionStatus.review);
            this.numberOfReviewedQuestions = reviewedQuestions.length;
            this.numberOfUnAttemptedQuestions = this.totalQuestionsInTest - this.numberOfAttemptedQuestions - this.numberOfReviewedQuestions;

        }, err => {
            this.numberOfUnAttemptedQuestions = this.totalQuestionsInTest;
        });
    }

    /**
     * Takes the student back to the test on clicking the back to test button
     */
    startTest() {

        //Unsubscribe the clock once the test resumes
        if (!this.isTestClosedUnConditionally)
            clearInterval(this.clockInterval);

        this.conductService.addTestLogs(this.testAttendee.id, false, true).subscribe(response => {
            this.testLogs = response;
        });
        this.reportService.updateCandidateInfo(this.testAttendee.id, true).subscribe();

        //Back to test
        this.router.navigate(['test'], { replaceUrl: true });
    }

    /**
     * Called when end test button is clicked
     */
    endTestButtonClicked() {
        this.loader = true;
        this.endTest(TestStatus.completedTest);
    }

    /**
    * Gets Test Attendee
    * @param testId: Id of Test
    */
    getTestAttendee(testId: number) {
        this.conductService.getTestAttendeeByTestId(testId, this.isTestPreview).subscribe((response) => {
            this.testAttendee = response;
            this.getSummaryDetailsByAttendeeId(this.testAttendee.id);
            if (this.test.allowTestResume === AllowTestResume.Supervised)
                this.getCandidateInfoToResumeTest();
            if (this.testAttendee.report !== null) {
                this.isTestBlocked = this.testAttendee.report.testStatus === TestStatus.blockedTest;
                this.isTestExpired = this.testAttendee.report.testStatus === TestStatus.expiredTest;
            }
            this.timeLeftOfTest(this.testAttendee.id);
            this.isButtonVisible = this.test.allowTestResume === 0 ? false : true;
        }, err => {
            this.router.navigate(['']);
        });
    }
    /**
     * sends request to the test conductor for test resume
     */
    sendRequestForResume() {
        this.isTestResume = true;
        this.reportService.updateCandidateInfo(this.testAttendee.id, false).subscribe(response => {
            if (response) {
                this.connectionService.sendRequest(this.testAttendee.id);
                this.snackbarRef.open('Request sent successfully', 'Dismiss', {
                    duration: 4000,
                });
                this.testAttendee.report.isTestPausedUnWillingly = true;
                this.disableButton = true;
            }
        });
    }
    /**
     * checks if test has been allowed to resume
     */
    getCandidateInfoToResumeTest() {
        this.reportService.getInfoResumeTest(this.testAttendee.id).subscribe(response => {
            if (response) {
                this.testAttendee.report = response;
                this.isAllowed = this.testAttendee.report.isAllowResume;
            }
        });
    }

    /**
     * Converts seconds to time string format HH:MM:SS
     * @param seconds: Seconds to convert
     */
    private secToTimeString(seconds: number) {
        let hh = Math.floor(seconds / 3600);
        let mm = Math.floor((seconds - hh * 3600) / 60);
        let ss = Math.floor(seconds - (hh * 3600 + mm * 60));

        return (hh < 10 ? '0' + hh : hh) + ':' + (mm < 10 ? '0' + mm : mm) + ':' + (ss < 10 ? '0' + ss : ss);
    }

    /**
     * Counts down time
     */
    private countDown() {
        this.timeLeft = this.timeLeft - 1;

        //Prevent displaying negative time
        if (this.timeLeft < 0) {
            this.timeLeft = 0;
        }

        this.timeString = this.secToTimeString(this.timeLeft);

        if (this.timeLeft <= 0) {
            this.endTest(TestStatus.expiredTest);
        }
    }

    /**
     * Updates time on the server
     */
    private timeOut() {
        this.timeOutCounter += 1;

        if (this.timeOutCounter >= this.TIMEOUT_TIME) {
            let timeElapsed = this.test.duration * 60 - this.timeLeft;
            this.conductService.setElapsedTime(this.testAttendee.id, timeElapsed).subscribe();
            this.timeOutCounter = 0;
        }
    }

    /**
    * Ends test and route to test-end page
    * @param testStatus: TestStatus object
    */
    private endTest(testStatus: TestStatus) {
        //Unsubscribe the clock timer
        if (!this.isTestClosedUnConditionally)
            clearInterval(this.clockInterval);

        this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe(response => {
            this.router.navigate(['test-end'], { replaceUrl: true });
            this.loader = false;
        });
    }
    endYourTest() {
        this.loader = true;
        this.reportService.createSessionForAttendee(this.testAttendee, this.test.link, true).subscribe(response => {
            this.router.navigate(['test-end'], { replaceUrl: true });
            this.loader = false;
        });
    }
}

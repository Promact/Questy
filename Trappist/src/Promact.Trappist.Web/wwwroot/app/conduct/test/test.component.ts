import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar } from '@angular/material';
import { Test } from '../../tests/tests.model';
import { TestQuestions } from '../test_conduct.model';
import { TestOrder } from '../../tests/enum-testorder';
import { SingleMultipleAnswerQuestionOption } from '../../questions/single-multiple-answer-question-option.model';
import { ConductService } from '../conduct.service';
import { QuestionStatus } from '../question_status.enum';
import { QuestionType } from '../../questions/enum-questiontype';
import { QuestionBase } from '../../questions/question';
import { TestAttendee } from '../test_attendee.model';
import { TestAnswer } from '../test_answer.model';
import { TestStatus } from '../teststatus.enum';

//Temporary imports
import { QuestionDisplay } from '../../questions/question-display';

@Component({
    moduleId: module.id,
    selector: 'test',
    templateUrl: 'test.html',
})
export class TestComponent implements OnInit {

    timeString: string;
    test: Test;
    testQuestions: TestQuestions[];
    questionDetail: string;
    options: SingleMultipleAnswerQuestionOption[];
    isTestReady: boolean;
    questionIndex: number;
    questionStatus: QuestionStatus;
    isQuestionSingleChoice: boolean;
    checkedOption: string;
    testAttendee: TestAttendee;
    testAnswers: TestAnswer[];

    private seconds: number;
    private focusLost: number;
    private tolerance: number;
    private timeOutCounter: number;

    private WARNING_TIME: number = 300;
    private WARNING_MSG: string = 'Hurry up!';
    private TIMEOUT_TIME: number = 180;
    private ALERT_MARK: string = 'You can\'t mark already answered question';
    private ALERT_CLEAR: string = 'You can\'t clear already answered question';
    private ALERT_DISQUALIFICATION: string = 'You are disqualified for multiple attempts to loose browser focus';
    private ALERT_BROWSER_FOCUS_LOST: string = 'Warning: Browser focus was lost';

    private defaultSnackBarDuration: number = 3000;

    constructor(private router: Router,
        private snackBar: MdSnackBar,
        private conductService: ConductService,
        private route: ActivatedRoute) {

        this.seconds = 0;
        this.secToTimeString(this.seconds);
        this.focusLost = 0;
        this.tolerance = 2;
        this.isTestReady = false;
        this.timeString = this.secToTimeString(this.seconds);
        this.test = new Test();
        this.testQuestions = new Array<TestQuestions>();
        this.options = new Array<SingleMultipleAnswerQuestionOption>();
        this.questionStatus = QuestionStatus.unanswered;
        this.questionIndex = 0;
        this.testAttendee = new TestAttendee();
        this.testAnswers = new Array<TestAnswer>();
    }

    ngOnInit() {
        window.addEventListener('blur', (event) => { this.windowFocusLost(event) });
        this.getTestByLink();

    }

    /**
     * Gets Test by Test Link
     */
    getTestByLink() {
        let url = window.location.pathname;
        let link = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test'));

        this.conductService.getTestByLink(link).subscribe((response) => {
            this.test = response;
            this.seconds = this.test.duration * 60;
            this.tolerance = this.test.browserTolerance;
            this.getTestAttendee(this.test.id);
        });
    }

    /**
     * Gets Test Attendee
     * @param testId: Id of Test
     */
    getTestAttendee(testId: number) {
        this.conductService.getTestAttendeeByTestId(testId).subscribe((response) => {
            this.testAttendee = response;
            this.getTestQuestion(this.test.id);
        });
    }

    /**
     * Gets all the test testQuestions
     */
    getTestQuestion(id: number) {
        this.conductService.getQuestions(id).subscribe((response) => {

            this.testQuestions = response;

            if (this.test.questionOrder === TestOrder.Random) {
                this.shuffleQuestion();
            }

            if (this.test.optionOrder === TestOrder.Random) {
                this.shuffleOption();
            }

            Observable.interval(1000).subscribe(() => { this.countDown(); this.timeOut(); });

            this.isTestReady = true;

            this.getTestStatus(this.testAttendee.id);
        });
    }

    /**
     * Gets the TestStatus of Attendee
     * @param attendeeId: Id of Attendee
     */
    getTestStatus(attendeeId: number) {
        this.conductService.getTestStatus(this.testAttendee.id).subscribe((response) => {
            let testStatus = response;
            if (testStatus != TestStatus.allCandidates) {
                this.endTest(testStatus);
            }

            this.ResumeTest();
        }, err => {
            this.navigateToQuestionIndex(0);
            this.timeOutCounter = this.TIMEOUT_TIME;
        });
    }

    /**
     * Resumes Test if Attendee had already answered some question
     */
    ResumeTest() {
        this.conductService.getAnswer(this.testAttendee.id).subscribe((response) => {
            this.testAnswers = response;

            this.testQuestions.forEach(x => {
                let answer = this.testAnswers.find(y => y.questionId === x.question.question.id);
                if (answer) {
                    x.questionStatus = answer.questionStatus;
                    answer.optionChoice.forEach(y => x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.find(z => z.id === y).isAnswer = true);
                }
            });
            
            this.conductService.getElapsedTime(this.testAttendee.id).subscribe((response) => {
                let spanTime = response;
                let spanTimeInSeconds = spanTime * 60;
                this.seconds -= spanTime * 60;
            });

            this.testAnswers
            this.navigateToQuestionIndex(0);
            this.timeOutCounter = this.TIMEOUT_TIME;
        }, err => {
            this.navigateToQuestionIndex(0);
            this.timeOutCounter = this.TIMEOUT_TIME;
        });
    }

    /**
     * Navigate to a question by its index
     * @param index: index of question
     */
    navigateToQuestionIndex(index: number) {
        this.isTestReady = false;

        if (index < 0 || index >= this.testQuestions.length) {
            this.isTestReady = true;
            return;
        }
        this.questionDetail = this.testQuestions[index].question.question.questionDetail;
        this.options = this.testQuestions[index].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption;
        //Sets boolean if question is single choice
        this.isQuestionSingleChoice = this.testQuestions[index].question.question.questionType === QuestionType.singleAnswer;

        //Restore status of previous question
        this.testQuestions[this.questionIndex].questionStatus = this.questionStatus;
        //Save answer to database 
        if (this.questionIndex !== index)
            this.addAnswer(this.testQuestions[this.questionIndex]);
        else
            this.isTestReady = true;

        //Save status of new question
        this.questionStatus = this.testQuestions[index].questionStatus;
        //Remove review status if Attendee re-visits the question
        if (this.questionStatus == QuestionStatus.review)
            this.questionStatus = QuestionStatus.unanswered;

        //Mark new question as selected
        this.markAsSelected(index);
        //Update question index
        this.questionIndex = index;
        //Reset time counter for question
        this.timeOutCounter = 0;
    }

    /**
     * Call API to add Answer to the Database
     * @param testQuestion: TestQuestion object
     */
    addAnswer(testQuestion: TestQuestions) {
        //Remove previous question's answer from the array 
        let index = this.testAnswers.findIndex(x => x.questionId === testQuestion.question.question.id);
        if (index !== -1)
            this.testAnswers.splice(index, 1);

        //Add new answer
        let testAnswer = new TestAnswer();
        testAnswer.questionId = testQuestion.question.question.id;

        if (testQuestion.question.question.questionType !== QuestionType.codeSnippetQuestion) {
            testQuestion.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => {
                if (x.isAnswer)
                    testAnswer.optionChoice.push(x.id);
            });
        }

        if (testQuestion.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.some(x => x.isAnswer) && this.questionStatus != QuestionStatus.review) {

            testAnswer.questionStatus = QuestionStatus.answered;

            this.questionStatus = QuestionStatus.answered;
        }
        else {
            testAnswer.questionStatus = testQuestion.questionStatus;
        }

        this.testAnswers.push(testAnswer);

        this.conductService.addAnswer(this.testAttendee.id, this.testAnswers).subscribe((response) => {
            this.isTestReady = true;
            let questionIndex = this.testQuestions.findIndex(x => x.question.question.id === testAnswer.questionId);
            if (testAnswer.questionStatus === QuestionStatus.answered) {
                this.markAsAnswered(questionIndex);
            }
        });
    }

    /**
     * Marks the question as selected
     * @param index: index of the question
     */
    markAsSelected(index: number) {
        this.testQuestions[index].questionStatus = QuestionStatus.selected;
    }

    /**
     * Marks the question for review
     * @param index: index of question
     */
    markAsReview(index: number) {
        if (this.questionStatus !== QuestionStatus.answered) {
            this.testQuestions[index].questionStatus = this.questionStatus = QuestionStatus.review;
        } else {
            this.openSnackBar(this.ALERT_MARK);
        }
    }

    /**
     * Marks the question as answered
     * @param index: index of question
     */
    markAsAnswered(index: number) {
        this.testQuestions[index].questionStatus = QuestionStatus.answered;
    }

    /**
     * Clears the response
     * @param index: index of question
     */
    clearResponse(index: number) {
        if (this.questionStatus !== QuestionStatus.answered) {
            this.testQuestions[index].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.isAnswer = false);
        } else {
            this.openSnackBar(this.ALERT_CLEAR);
        }
    }

    /**
     * Selects an option
     * @param questionIndex
     * @param optionIndex
     * @param isSingleChoice
     */
    selectOption(questionIndex: number, optionIndex: number, isSingleChoice: boolean = false) {
        if (this.questionStatus != QuestionStatus.answered) {
            if (isSingleChoice) {
                this.clearResponse(questionIndex);
                this.testQuestions[questionIndex].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isAnswer = true;
            } else {
                this.testQuestions[questionIndex].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isAnswer = true;
            }
        }
    }

    /**
     * Returns stats of question as string 
     * @param status: index of the question
     */
    getQuestionStatus(status: QuestionStatus) {
        return QuestionStatus[status];
    }

    /**
     * Shuffle testQuestions 
     */
    private shuffleQuestion() {
        this.testQuestions = this.shuffleArray(this.testQuestions);
    }

    /**
     * Shuffle options
     */
    private shuffleOption() {
        this.testQuestions.forEach(x => {
            x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption = this.shuffleArray(x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption);
        });
    }

    private shuffleArray(arrayToShuffle: any[]) {
        let max = arrayToShuffle.length - 1;
        for (let i = 0; i < max; i++) {
            let pickIndex = Math.floor(Math.random() * max);

            //swap options
            [arrayToShuffle[i], arrayToShuffle[pickIndex]] = [arrayToShuffle[pickIndex], arrayToShuffle[i]]
        }

        return arrayToShuffle;
    }

    /**
     * Increments focus lost counter and shows warning message
     * @param event: Focus event 
     */
    private windowFocusLost(event: FocusEvent) {
        this.focusLost += 1;
        let message: string;
        let duration: number = 0;
        message = this.focusLost > this.test.browserTolerance ? this.ALERT_DISQUALIFICATION : this.ALERT_BROWSER_FOCUS_LOST;

        if (this.focusLost > this.test.browserTolerance) {
            this.openSnackBar(message, duration);
            this.endTest(TestStatus.blockedTest);
        }
        else
            this.openSnackBar(message, duration);
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
        this.seconds = this.seconds - 1;
        this.timeString = this.secToTimeString(this.seconds);

        if (this.seconds === this.WARNING_TIME) {
            this.openSnackBar(this.WARNING_MSG);
        }

        if (this.seconds < 0) {
            this.navigateToQuestionIndex(0);
            this.endTest(TestStatus.expiredTest);
        }
    }

    /**
     * Called when end test button is clicked
     */
    endTestButtonClicked() {
        this.endTest(TestStatus.completedTest);
    }

    /**
     * Updates time on the server
     */
    private timeOut() {
        this.timeOutCounter += 1;

        if (this.timeOutCounter >= this.TIMEOUT_TIME) {
            this.conductService.setElapsedTime(this.testAttendee.id).subscribe();
            this.timeOutCounter = 0;
        }
    }

    /**
     * Ends test and route to test-end page
     * @param testStatus: TestStatus object
     */
    private endTest(testStatus: TestStatus) {
        this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe();
        //A measure taken to add answer of question attempted just before the Test end
        this.navigateToQuestionIndex(0);
        window.close();
    }

    /**
    * Opens snack bar
    * @param message: message to display
    * @param duration: duration in seconds to show snackbar
    * @param enableRouting: enable routing after snack bar dismissed
    * @param routeTo: routing path 
    */
    private openSnackBar(message: string, duration: number = this.defaultSnackBarDuration, enableRouting: boolean = false, routeTo: (string | number)[] = ['']) {
        let snackBarAction = this.snackBar.open(message, 'Dismiss', {
            duration: duration
        });
        if (enableRouting) {
            this.router.navigate(routeTo);
        }
    }
}

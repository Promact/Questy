import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, AfterViewInit, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Observable } from 'rxjs/Rx';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar } from '@angular/material';
import { Test } from '../../tests/tests.model';
import { TestPreviewComponent } from '../../tests/test-preview/test-preview.compponent';
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
import { AceEditorComponent } from 'ng2-ace-editor';
import 'brace';
import 'brace/theme/cobalt';
import 'brace/theme/monokai';
import 'brace/theme/eclipse';
import 'brace/theme/solarized_light';
import 'brace/mode/java';
import 'brace/mode/c_cpp';
import { TestLogs } from '../../reports/testlogs.model';
import { AllowTestResume } from '../../tests/enum-allowtestresume';
import { CodeResponse } from '../code.response.model';
declare let screenfull: any;


//Temporary imports
import { QuestionDisplay } from '../../questions/question-display';
import { ReportService } from '../../reports/report.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
    moduleId: module.id,
    selector: 'test',
    templateUrl: 'test.html',
})
export class TestComponent implements OnInit {
    @ViewChild('editor') editor: AceEditorComponent;
    timeString: string;
    test: Test;
    testTypePreview: boolean;
    @Input() testLink: string;
    selectLanguage: string;
    selectedMode: string;
    languageMode: string[];
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
    isQuestionCodeSnippetType: boolean;
    themes: string[];
    codeAnswer: string;
    selectedTheme: string;
    testLogs: TestLogs;
    codeResult: string;
    showResult: boolean;
    isCloseWindow: boolean;
    isConnectionLoss: boolean;
    timeWarning: boolean;
    testEnded: boolean;
    isCodeProcessing: boolean;
    routeForTestEnd: any;
    istestEnd: boolean;
    url: string;

    private seconds: number;
    private focusLost: number;
    private tolerance: number;
    private timeOutCounter: number;
    private resumable: AllowTestResume;

    private WARNING_TIME: number = 300;
    private WARNING_MSG: string = 'Hurry up!';
    private TIMEOUT_TIME: number = 10;
    private ALERT_MARK: string = 'You can\'t mark already answered question.';
    private ALERT_CLEAR: string = 'You can\'t clear already answered question.';
    private ALERT_DISQUALIFICATION: string = 'You are disqualified for multiple attempts to loose browser focus.';
    private ALERT_BROWSER_FOCUS_LOST: string = 'Warning: Browser focus was lost.';
    private JAVA_CODE: string = 'import java.io.*;\nimport java.util.*;\nclass Program {//Do not change class name\n' +
    '   public static void main(String[] args) {\n' +
    '       System.out.println("Hello World");\n' +
    '   }\n' +
    '}\n';
    //Temporary solution for setting coding language on resume
    private CODING_LANGUAGES: string[] = ['Java', 'Cpp', 'C'];
    private defaultSnackBarDuration: number = 3000;
    private clockIntervalListener: Subscription;

    constructor(private router: Router,
        private snackBar: MdSnackBar,
        private conductService: ConductService,
        private route: ActivatedRoute,
        private elementRef: ElementRef,
        private reportService: ReportService,
        private location: Location) {

        this.languageMode = ['Java', 'Cpp', 'C'];
        this.seconds = 0;
        this.secToTimeString(this.seconds);
        this.focusLost = 0;
        this.tolerance = 2;
        this.isTestReady = false;
        this.selectedTheme = 'monokai';
        this.timeString = this.secToTimeString(this.seconds);
        this.test = new Test();
        this.testTypePreview = false;
        this.testQuestions = new Array<TestQuestions>();
        this.options = new Array<SingleMultipleAnswerQuestionOption>();
        this.questionStatus = QuestionStatus.unanswered;
        this.questionIndex = -1;
        this.testAttendee = new TestAttendee();
        this.testAnswers = new Array<TestAnswer>();
        this.isQuestionCodeSnippetType = false;
        this.selectLanguage = 'Java';
        this.selectedMode = 'java';
        this.codeAnswer = this.JAVA_CODE;
        this.themes = ['eclipse', 'solarized_light', 'monokai', 'cobalt'];
        this.codeResult = '';
        this.showResult = false;
        this.timeWarning = false;
        this.testEnded = false;
        this.isCodeProcessing = false;
        this.url = window.location.pathname;
    }

    ngOnInit() {
        this.getTestByLink(this.testLink);
        history.pushState(null, null, null);
        window.addEventListener('popstate', function (event) {
            history.pushState(null, null, null);
        });;
    }


    /**
     * saves the TestLogs if server is gone off
     */
    saveTestLogs() {
        this.conductService.addTestLogs(this.testAttendee.id, this.isCloseWindow, this.isConnectionLoss, false).subscribe(response => {
            this.testLogs = response;
        });

    }

    /**
     * changes the theme of editor
     * @param theme
     */
    changeTheme(theme: string) {
        this.selectedTheme = theme;
        this.editor.setTheme(theme);
    }
    /**
     * changes language mode of editor
     * @param language
     */
    changeLanguage(language: string) {
        let codeLanguage = language.toLowerCase();

        this.changeText();

        if (this.testAnswers[this.questionIndex]) {
            let codingAnswer = this.testAnswers.find(x => x.questionId === this.testQuestions[this.questionIndex].question.question.id);
            let answeredLanguage: string = isNaN(+codingAnswer.code.language) ? codingAnswer.code.language : this.CODING_LANGUAGES[codingAnswer.code.language];
            if (answeredLanguage.toLowerCase() === codeLanguage)
                this.codeAnswer = codingAnswer.code.source;
        }

        if (codeLanguage.toLowerCase() === 'c')
            codeLanguage = 'c_cpp';
        this.editor.setMode(language);
    }
    /**
     * changes the pre-defined text for the editor
     */
    changeText() {
        if (this.selectLanguage.toLowerCase() === 'java') {
            this.codeAnswer = this.JAVA_CODE;
            this.selectedMode = 'java';
        }

        if (this.selectLanguage.toLowerCase() === 'cpp') {
            this.selectedMode = 'c_cpp';
            this.codeAnswer = [
                '#include <iostream>'
                , 'using namespace std;'
                , 'int main()'
                , '{'
                , '     cout << "Hello World!";'
                , '}'
            ].join('\n');
        }
        if (this.selectLanguage.toLowerCase() === 'c') {
            this.codeAnswer = [
                ' #include <stdio.h>'
                , 'int main()'
                , '{'
                , '     printf("Hello, World!");'
                , '     return 0;'
                , '}'
            ].join('\n');
            this.selectedMode = 'c_cpp';
        }
    }
    /**
     * keep tracks of what user is writing in editor
     * @param code
     */
    onChange(code: string) {
        this.codeAnswer = code;
    }

    /**
     * Gets Test by Test Link
     */
    @Input() set getTestLink(link: string) {

        this.getTestByLink(link);

    } getTestByLink(link: string) {
        let url = window.location.pathname;
        if (link === '' || link === undefined)
            this.testLink = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test'));
        else {
            this.testLink = link;
            this.testTypePreview = true;
        }

        this.conductService.getTestByLink(this.testLink, this.testTypePreview).subscribe((response) => {
            this.test = response;
            this.seconds = this.test.duration * 60;
            this.tolerance = this.test.browserTolerance;
            this.WARNING_MSG = this.test.warningMessage;
            this.WARNING_TIME = this.test.warningTime * 60;
            this.resumable = this.test.allowTestResume;

            if (this.resumable === AllowTestResume.Supervised) {
                window.onbeforeunload = (ev) => {
                    this.saveTestLogs();
                    if (!this.testEnded) {
                        let dialogText = 'WARNING: Your report will not generate. Please use End Test button.';
                        ev.returnValue = dialogText;
                        return dialogText;
                    }
                };
            }

            if (this.testTypePreview)
                this.getTestQuestion(this.test.id);
            else this.getTestAttendee(this.test.id, this.testTypePreview);

        }, err => {
            window.location.href = window.location.origin + '/pageNotFound';
        });
    }

    /**
     * Gets Test Attendee
     * @param testId: Id of Test
     */
    getTestAttendee(testId: number, testTypePreview: boolean) {
        this.conductService.getTestAttendeeByTestId(testId, testTypePreview).subscribe((response) => {
            this.testAttendee = response;
            this.getTestQuestion(this.test.id);
        }, err => {
            this.router.navigate(['']);
        });
    }

    /**
     * Gets all the test testQuestions
     */
    @Input() set getQuestion(question: number) {
        this.getTestQuestion(question);

    } getTestQuestion(id: number) {
        this.conductService.getQuestions(id).subscribe((response) => {

            this.testQuestions = response;

            if (this.test.questionOrder === TestOrder.Random) {
                this.shuffleQuestion();
            }

            if (this.test.optionOrder === TestOrder.Random) {
                this.shuffleOption();
            }

            this.clockIntervalListener = Observable.interval(1000).subscribe(() => { this.countDown(); if (!this.testTypePreview) this.timeOut(); });
            this.isTestReady = true;
            if (this.testTypePreview)
                this.navigateToQuestionIndex(0);
            else this.getTestStatus(this.testAttendee.id);
        });
    }

    /**
     * Gets the TestStatus of Attendee
     * @param attendeeId: Id of Attendee
     */
    getTestStatus(attendeeId: number) {
        this.conductService.getTestStatus(this.testAttendee.id).subscribe((response) => {
            let testStatus = response;
            if (testStatus !== TestStatus.allCandidates) {
                //Close the window if Test is already completed
                this.routeForTestEnd = 'conduct/' + this.testLink;
                this.router.navigate(['/test-end'], { relativeTo: this.routeForTestEnd, replaceUrl: true });
            }
            window.addEventListener('blur', (event) => { if (this.test.browserTolerance !== 0 && !this.istestEnd) this.windowFocusLost(event); });
            window.addEventListener('offline', () => { this.isCloseWindow = false; this.isConnectionLoss = true; this.saveTestLogs(); this.endTest(TestStatus.completedTest); });

            this.resumeTest();
        }, err => {
            this.navigateToQuestionIndex(0);
            this.timeOutCounter = this.TIMEOUT_TIME;
        });
    }

    /**
     * Resumes Test if Attendee had already answered some question
     */
    resumeTest() {
        this.isTestReady = false;
        this.conductService.getAnswer(this.testAttendee.id).subscribe((response) => {
            this.testAnswers = response;

            this.testQuestions.forEach(x => {
                let answer = this.testAnswers.find(y => y.questionId === x.question.question.id);
                if (answer) {
                    x.questionStatus = answer.questionStatus;
                    answer.optionChoice.forEach(y => x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.find(z => z.id === y).isAnswer = true);
                }
            });

            this.getElapsedTime();

            this.navigateToQuestionIndex(0);

            this.timeOutCounter = this.TIMEOUT_TIME;
        }, err => {
            this.getElapsedTime();
            this.navigateToQuestionIndex(0);
            this.timeOutCounter = this.TIMEOUT_TIME;
            this.isTestReady = true;
        });

    }

    /**
     * Fetches the elapsed time from the server
     */
    getElapsedTime() {
        this.conductService.getElapsedTime(this.testAttendee.id).subscribe((response) => {
            let spanTime = response;
            let spanTimeInSeconds = Math.round(spanTime * 60);
            this.seconds -= spanTimeInSeconds;
            this.isTestReady = true;
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
        this.isQuestionCodeSnippetType = this.testQuestions[index].question.question.questionType === QuestionType.codeSnippetQuestion;

        if (!this.isQuestionCodeSnippetType) {
            this.options = this.testQuestions[index].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption;
            //Sets boolean if question is single choice
            this.isQuestionSingleChoice = this.testQuestions[index].question.question.questionType === QuestionType.singleAnswer;
        }

        //Save answer to database 
        if (this.questionIndex !== index) {
            //Prevent add answer for the first time on page load
            if (this.questionIndex !== -1) {
                //Only add answer that are of MCQ/SCQ type
                if (this.testQuestions[this.questionIndex].question.question.questionType !== QuestionType.codeSnippetQuestion) {
                    this.addAnswer(this.testQuestions[this.questionIndex]);
                } else {
                    //Add code snippet answer if its marked as unanswered or review
                    if (this.questionStatus === QuestionStatus.review || this.questionStatus === QuestionStatus.unanswered) {
                        this.addAnswer(this.testQuestions[this.questionIndex]);
                    }
                }
                //Restore status of previous question
                this.testQuestions[this.questionIndex].questionStatus = this.questionStatus;
            }
        }
        else {
            this.isTestReady = true;
            return;
        }

        //Set coding solution if the question is of coding type
        if (this.testQuestions[index].question.question.questionType === QuestionType.codeSnippetQuestion) {
            let codingAnswer = this.testAnswers.find(x => x.questionId === this.testQuestions[index].question.question.id);
            this.languageMode = this.testQuestions[index].question.codeSnippetQuestion.languageList;
            if (codingAnswer !== undefined) {
                this.codeAnswer = codingAnswer.code.source;
                this.selectLanguage = isNaN(+codingAnswer.code.language) ? codingAnswer.code.language : this.CODING_LANGUAGES[codingAnswer.code.language];
                this.codeResult = codingAnswer.code.result;
                if (this.codeResult)
                    this.showResult = true;
            } else {
                this.selectLanguage = this.languageMode[0];
                this.changeText();
                this.codeResult = '';
            }
        }

        //Set status of new question
        this.questionStatus = this.testQuestions[index].questionStatus;
        //Remove review status if Attendee re-visits the question
        if (this.questionStatus === QuestionStatus.review)
            this.questionStatus = QuestionStatus.unanswered;

        //Mark new question as selected
        this.markAsSelected(index);
        //Update question index
        this.questionIndex = index;
        //Reset time counter for question
        this.timeOutCounter = 0;
    }

    runCode() {
        this.showResult = true;
        this.isCodeProcessing = true;
        if (this.testTypePreview)
            this.codeResult = this.codeAnswer;
        else {
            this.codeResult = 'Processing....';

            this.questionStatus = QuestionStatus.answered;

            let solution = new TestAnswer();
            solution.code.source = this.codeAnswer;
            solution.code.language = this.selectLanguage;
            solution.questionId = this.testQuestions[this.questionIndex].question.question.id;
            solution.questionStatus = QuestionStatus.answered;

            //Remove previous question's answer from the array 
            let index = this.testAnswers.findIndex(x => x.questionId === solution.questionId);
            if (index !== -1)
                this.testAnswers.splice(index, 1);

            this.conductService.execute(this.testAttendee.id, solution).subscribe(res => {
                let codeResponse = new CodeResponse();
                codeResponse = res;
                this.codeResult = !codeResponse.errorOccurred ? codeResponse.message : codeResponse.error;
                solution.code.result = this.codeResult;
                this.testAnswers.push(solution);
                this.isCodeProcessing = false;
            }, err => {
                this.codeResult = 'Oops! server error has occured.';
                this.isCodeProcessing = false;
            });
        }
    }

    /**
     * Call API to add Answer to the Database
     * @param testQuestion: TestQuestion object
     */
    addAnswer(testQuestion: TestQuestions, _callback?: any) {

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

            if (testQuestion.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.some(x => x.isAnswer)
                && this.questionStatus !== QuestionStatus.review) {

                testAnswer.questionStatus = QuestionStatus.answered;

                this.questionStatus = QuestionStatus.answered;
            } else if (testQuestion.questionStatus === QuestionStatus.selected || testQuestion.questionStatus === QuestionStatus.answered) {
                testAnswer.questionStatus = QuestionStatus.unanswered;
            } else {
                testAnswer.questionStatus = testQuestion.questionStatus;
            }
        } else {
            testAnswer.code.source = this.codeAnswer;
            testAnswer.code.language = this.selectLanguage;
            testAnswer.code.result = this.codeResult;
            if (testQuestion.questionStatus === QuestionStatus.selected) {
                testAnswer.questionStatus = QuestionStatus.unanswered;
            } else {
                testAnswer.questionStatus = testQuestion.questionStatus;
            }
        }

        this.testAnswers.push(testAnswer);
        if (this.testTypePreview) {
            this.isTestReady = true;
            let questionIndex = this.testQuestions.findIndex(x => x.question.question.id === testAnswer.questionId);
            this.markAsAnswered(questionIndex);
        }
        else {
            this.conductService.addAnswer(this.testAttendee.id, testAnswer).subscribe((response) => {
                this.isTestReady = true;
                let questionIndex = this.testQuestions.findIndex(x => x.question.question.id === testAnswer.questionId);
                if (testAnswer.questionStatus === QuestionStatus.answered) {
                    this.markAsAnswered(questionIndex);
                }
                //call callback method if provided
                if (_callback)
                    _callback();
            }, err => {
                this.isTestReady = true;
            });
        }
    }

    /**
     * Marks the question as selected
     * @param index: index of the question
     */
    markAsSelected(index: number) {
        this.testQuestions[index].questionStatus = QuestionStatus.selected;
        if (this.testTypePreview)
            this.isTestReady = true;
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
        this.testQuestions[index].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption.forEach(x => x.isAnswer = false);
        this.questionStatus = QuestionStatus.unanswered;
    }

    /**
     * Selects an option
     * @param questionIndex
     * @param optionIndex
     * @param isSingleChoice
     */
    selectOption(questionIndex: number, optionIndex: number, isSingleChoice: boolean = false) {        
        if (isSingleChoice) {
            this.clearResponse(questionIndex);
            this.testQuestions[questionIndex].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isAnswer = true;
        } else {
            let checked = this.testQuestions[questionIndex].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isAnswer;
            this.testQuestions[questionIndex].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[optionIndex].isAnswer = !checked;
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
     * Called when end test button is clicked
     */
    endTestButtonClicked() {
        this.endTest(TestStatus.completedTest);
    }

    isLastQuestion() {
        return this.questionIndex === this.testQuestions.length - 1;
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
            if (x.question.question.questionType !== QuestionType.codeSnippetQuestion)
                x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption = this.shuffleArray(x.question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption);
        });
    }

    private shuffleArray(arrayToShuffle: any[]) {
        let max = arrayToShuffle.length - 1;
        for (let i = 0; i < max; i++) {
            let pickIndex = Math.floor(Math.random() * max);

            //swap options
            [arrayToShuffle[i], arrayToShuffle[pickIndex]] = [arrayToShuffle[pickIndex], arrayToShuffle[i]];
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

        if (this.focusLost <= this.test.browserTolerance) {
            message = this.ALERT_BROWSER_FOCUS_LOST;
        }



        if (this.focusLost > this.test.browserTolerance) {
            this.conductService.addTestLogs(this.testAttendee.id, false, false, false).subscribe((response: any) => {
                this.testLogs = response;
            });
            this.snackBar.dismiss();
            this.endTest(TestStatus.blockedTest);
        }
        else if (this.focusLost <= this.test.browserTolerance) {

            this.openSnackBar(message, duration);
            this.conductService.addTestLogs(this.testAttendee.id, false, false, false).subscribe((response: any) => {

                this.testLogs = response;
            });
        }
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
        this.seconds = this.seconds > 0 ? this.seconds - 1 : 0;
        this.timeString = this.secToTimeString(this.seconds);

        if (this.seconds <= this.WARNING_TIME && !this.timeWarning) {
            this.timeWarning = true;
            this.openSnackBar(this.WARNING_MSG);
        }

        if (this.seconds <= 0) {
            this.conductService.setElapsedTime(this.testAttendee.id).subscribe();
            this.endTest(TestStatus.expiredTest);
        }
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
        this.istestEnd = true;
        this.isTestReady = false;

        if (this.clockIntervalListener) {
            this.clockIntervalListener.unsubscribe();
        }

        if (this.testTypePreview) {
            if (screenfull.enabled) {
                screenfull.toggle();
            }
            this.location.back();
        }

        if (this.testQuestions[this.questionIndex].question.question.questionType !== QuestionType.codeSnippetQuestion
            || (this.testQuestions[this.questionIndex].question.question.questionType === QuestionType.codeSnippetQuestion && this.questionStatus !== QuestionStatus.answered)) {
            //Add answer and close window
            this.addAnswer(this.testQuestions[this.questionIndex], () => {
                this.isTestReady = false;
                this.closeWindow(testStatus);
            });
        } else {
            this.closeWindow(testStatus);
        }
    }

    /**
     * Closes window 
     */
    private closeWindow(testStatus: TestStatus) {
        if (this.resumable === AllowTestResume.Supervised) {
            this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe(response => {
                this.testEnded = true;
                if (testStatus !== TestStatus.blockedTest)
                    this.router.navigate(['test-summary'], { replaceUrl: true });
                else {
                    this.routeForTestEnd = 'conduct/' + this.testLink;
                    this.router.navigate(['/test-end/blocked'], { relativeTo: this.routeForTestEnd, replaceUrl: true });
                }
            });
        }

        else if (this.resumable === AllowTestResume.Unsupervised && testStatus !== TestStatus.blockedTest)
            this.router.navigate(['test-summary'], { replaceUrl: true });

        else if (this.resumable === AllowTestResume.Unsupervised && testStatus === TestStatus.blockedTest) {
            this.conductService.setTestStatus(this.testAttendee.id, testStatus).subscribe(response => {
                this.testEnded = true;
                this.routeForTestEnd = 'conduct/' + this.testLink;
                this.router.navigate(['/test-end/blocked'], { relativeTo: this.routeForTestEnd, replaceUrl: true });
            });
        }
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

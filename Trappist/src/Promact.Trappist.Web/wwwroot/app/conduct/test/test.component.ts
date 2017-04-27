import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Router } from '@angular/router';
import { MdSnackBar } from '@angular/material';
import { Test } from '../../tests/tests.model';
import { QuestionDisplay } from '../../questions/question-display';
import { TestOrder } from '../../tests/enum-testorder';

//Temporary imports
import { QuestionsService } from '../../questions/questions.service';

@Component({
    moduleId: module.id,
    selector: 'test',
    templateUrl: 'test.html',
})
export class TestComponent implements OnInit {

    timeString: string;
    test: Test;
    question: QuestionDisplay[];

    private seconds: number;
    private focusLost: number;
    private tolerance: number;

    private defaultSnackBarDuration: number = 3000;

    constructor(private router: Router,
        private snackBar: MdSnackBar,
        private questionServices: QuestionsService) {

        this.seconds = 310;
        this.secToTimeString(this.seconds);
        this.focusLost = 0;
        this.tolerance = 2;
        this.test = new Test();
        this.question = new Array<QuestionDisplay>();

        //Custom test
        this.test.testName = 'Very Difficult Test';
        this.test.browserTolerance = this.tolerance;
        this.test.duration = this.seconds;
        this.test.warningMessage = 'Hurry! Hurry! Hurry!'
        this.test.warningTime = 300;
        this.test.questionOrder = TestOrder.Random;
        this.test.optionOrder = TestOrder.Random;
    }

    ngOnInit() {
        window.onblur = () => { this.windowFocusLost() };
        Observable.interval(1000).subscribe(() => this.countDown());
    }

    /**
     * Gets all the test questions
     */
    getTestQuestion() {
        this.questionServices.getQuestions().subscribe((response) => {
            this.question = response;
        });
    }

    /**
     * Shuffle Questions and thier Options
     * @param shuffleOption
     */
    private shuffleQuestion(shuffleOption: boolean = false) {
        let shuffledQuestion = Array<QuestionDisplay>();
        let startIndex: number = 1;
        let endIndex = this.question.length;
        let randomIndex: number;

        while (startIndex <= endIndex) {
            randomIndex = Math.floor(Math.random() * endIndex) + startIndex;
            shuffledQuestion.push(this.question[randomIndex - 1]);
            startIndex += 1;
        }
    }

    /**
     * Increments focus lost counter and shows warning message
     */
    private windowFocusLost() {
        this.focusLost += 1;
        let message: string;
        let duration: number;
        [message, duration] = this.focusLost > this.test.browserTolerance ? ['Cheating not allowed. You are disqualified', 0] : ['Focus lost detected', 0];
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

        if (this.seconds === this.test.warningTime) {
            this.openSnackBar(this.test.warningMessage);
        }

        if (this.seconds < 0) {
            this.endTest();
        }
    }

    /**
     * Ends test and route to test-end page
     */
    private endTest() {
        //To-Do Add some functionality to end test before routing
        this.router.navigate(['test-end']);
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

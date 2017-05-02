/// <reference path="../../questions/enum-questiontype.ts" />
/// <reference path="../../questions/question-display.ts" />
/// <reference path="../../questions/question.model.ts" />
/// <reference path="../../tests/tests.model.ts" />
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService } from '../report.service';
import { TestAttendee } from '../testattendee.model';
import { QuestionDisplay } from '../../questions/question-display';
import { QuestionType } from '../../questions/enum-questiontype';
import { Question } from '../../questions/question.model';
import { TestQuestion } from '../../tests/tests.model';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';
@Component({
    moduleId: module.id,
    selector: 'individual-report',
    templateUrl: 'individual-report.html'
})

export class IndividualReportComponent implements OnInit {
    public pieChartLabels: string[];
    public pieChartData: number[];
    public pieChartType: string;
    testAttendeeId: number;
    testAttendee: TestAttendee;
    testName: string;
    loader: boolean;
    testState: string;
    testId: number;
    testQuestions: TestQuestion[];
    optionName: string[];
    correctAnswers: number;
    incorrectAnswers: number;
    notAttempted: number;
    totalQuestions: number;
    pieChartLabel: string[];
    //pieChartData: Array<number>;
    //pieChartType: string = 'pie';
    correct: number;
    incorrect: number;
    notAttemptedQuestions: number;
    constructor(private reportsService: ReportService, private route: ActivatedRoute) {
        this.loader = true;
        this.testQuestions = new Array<TestQuestion>();
        this.testAttendeeId = this.route.snapshot.params['id'];
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];

        
        this.pieChartLabels = ['Correct', 'Incorrect', 'Not Attempted'];
        this.pieChartData = [300, 500, 100];
        this.pieChartType = 'pie'
        //
    }

    ngOnInit() {
        this.getTestAttendeeDetails();
    }

    getTestAttendeeDetails() {
        this.reportsService.getTestAttendeeById(this.testAttendeeId).subscribe((response) => {
            this.testAttendee = response;
            this.testName = this.testAttendee.test.testName;
            this.testId = this.testAttendee.test.id;
            this.reportsService.getTestQuestions(this.testAttendee.test.id).subscribe((response) => {
                //console.log(response);
                this.testQuestions = response;
                //this.questionDisplay();
                //console.log(this.testQuestions);
                //console.log(this.testAttendee);
                this.testFinishStatus();
                //console.log(this.testState);
                //this.pieChartLabels = ['Download Sales', 'In-Store Sales', 'Mail Sales'];
                //this.pieChartData = [300, 500, 100];
                //this.pieChartType = 'pie'
                this.charts();
               

                this.loader = false;
            });
        });
    }

    testFinishStatus() {
        switch (this.testAttendee.testState) {
            case 0:
                this.testState = 'Completed';
                break;
            case 1:
                this.testState = 'Auto Submit';
                break;
            case 2:
                this.testState = 'Blocked';
                break;
        }
    }
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer)
            return 'correct';
    }

    charts() {
        this.correctAnswers = 45;
        this.incorrectAnswers = 30;
        this.notAttempted = 75;
        //this.pieChartValue();
    //    this.pieChartLabels = ['Download Sales', 'In-Store Sales', 'Mail Sales'];
    //    this.pieChartData = this.pieChartValue();
    //    this.pieChartType = 'pie';
        
    //    //this.pieChartLabel = ['correctAnswers', 'incorrectAnswers', 'notAttempted'];
    //    //this.pieChartData = [10, 20, 30];
    //    //this.pieChartType = 'pie';
    }
    public pieChartValue() {
       
        this.totalQuestions = this.correctAnswers + this.incorrectAnswers + this.notAttempted;
        this.correct = (this.correctAnswers / this.totalQuestions) * 360;
        this.incorrect = (this.incorrectAnswers / this.totalQuestions) * 360;
        this.notAttemptedQuestions = (this.notAttempted / this.totalQuestions) * 360;
        return [this.correct, this.incorrect, this.notAttemptedQuestions];
    }
}

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
import { TestQuestion} from '../../tests/tests.model';
@Component({
    moduleId: module.id,
    selector: 'individual-report',
    templateUrl: 'individual-report.html'
})

export class IndividualReportComponent implements OnInit {
    testAttendeeId: number;
    //@Input('testAttendeeDetails')
    testAttendee: TestAttendee;
    testName: string;
    loader: boolean;
    testState: string;
    testQuestions: TestQuestion[] = new Array<TestQuestion>();
    QuestionType = QuestionType;
    constructor(private reportsService: ReportService, private route: ActivatedRoute) {
        this.testAttendeeId = this.route.snapshot.params['id'];
        this.loader = true;
    }

    ngOnInit() {
        this.getTestAttendeeDetails();
    }

    getTestAttendeeDetails() {
        this.reportsService.getTestAttendeeById(this.testAttendeeId).subscribe((response) => {
            this.testAttendee = response;
            this.testName = this.testAttendee.test.testName;
            this.reportsService.getTestQuestions(this.testAttendee.test.id).subscribe((response) => {
                //console.log(response);
                this.testQuestions = response;
                //this.questionDisplay();
                console.log(this.testQuestions);
                console.log(this.testAttendee);
                this.testFinishStatus();
                console.log(this.testState);
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
}

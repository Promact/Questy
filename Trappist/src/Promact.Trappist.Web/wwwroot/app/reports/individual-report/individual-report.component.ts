import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService } from '../report.service';
import { TestAttendee } from '../testattendee.model';
import { QuestionDisplay } from '../../questions/question-display';
import { TestQuestion } from '../../tests/tests.model';
import { TestAnswers } from '../testanswers.model';
import { SingleMultipleAnswerQuestionOption } from '../../questions/single-multiple-answer-question-option.model';
import { ChartsModule } from 'ng2-charts/ng2-charts';
declare var jsPDF: any;

@Component({
    moduleId: module.id,
    selector: 'individual-report',
    templateUrl: 'individual-report.html'
})

export class IndividualReportComponent implements OnInit {
    questionPieChartLabels: string[];
    pieChartType: string;
    correctPieChartLabels: string[];
    testAttendeeId: number;
    testAttendee: TestAttendee;
    testName: string;
    loader: boolean;
    testStatus: string;
    testId: number;
    testQuestions: TestQuestion[];
    optionName: string[];
    correctAnswers: number;
    incorrectAnswers: number;
    notAttempted: number;
    totalQuestions: number;
    testAnswers: TestAnswers[];
    correctMarks: string;
    incorrectmarks: string;
    marks: number;
    percentage: number;
    percentile: number;
    timeTakenInHours: number;
    timeTakenInMinutes: number;
    easy: number;
    medium: number;
    hard: number;

    constructor(private reportsService: ReportService, private route: ActivatedRoute) {
        this.loader = true;
        this.testQuestions = new Array<TestQuestion>();
        this.testAttendeeId = this.route.snapshot.params['id'];
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
        this.testAnswers = new Array<TestAnswers>();
        this.questionPieChartLabels = ['Correct', 'InCorrect', 'Not Attempted'];
        this.correctPieChartLabels = ['easy', 'medium', 'hard'];
        this.pieChartType = 'pie';
        this.correctAnswers = this.incorrectAnswers = 0;
        this.easy = this.medium = this.hard = 0;
    }

    ngOnInit() {
        this.getTestAttendeeDetails();
    }

    /**
     * Gets all the datails to be displayed
     */
    getTestAttendeeDetails() {
        //Gets test attendee details
        this.reportsService.getTestAttendeeById(this.testAttendeeId).subscribe((response) => {
            this.testAttendee = response;
            this.testName = this.testAttendee.test.testName;
            this.testId = this.testAttendee.test.id;
            this.correctMarks = this.testAttendee.test.correctMarks;
            this.incorrectmarks = this.testAttendee.test.incorrectMarks;
            this.marks = this.testAttendee.report.totalMarksScored;
            this.percentage = this.testAttendee.report.percentage;
            this.percentile = this.testAttendee.report.percentile;
            this.timeTakenInHours = Math.floor(this.testAttendee.report.timeTakenByAttendee / 60);
            this.timeTakenInMinutes = this.testAttendee.report.timeTakenByAttendee % 60;
            //Gets all the answers given by the test attendee
            this.reportsService.getTestAttendeeAnswers(this.testAttendee.id).subscribe((response) => {
                this.testAnswers = response;
                console.log(this.testAnswers);
                //Gets all the questions present in the test
                this.reportsService.getTestQuestions(this.testAttendee.test.id).subscribe((response) => {
                    console.log(response);
                    this.testQuestions = response;
                    this.testFinishStatus();
                    this.attendeeAnswers();
                    this.questionPieChartValue();
                    this.correctPieChartValue();
                    this.loader = false;
                });
            });
        });
    }

    //Sets the test finish status of the candidate
    testFinishStatus() {
        switch (this.testAttendee.report.testStatus) {
            case 0:
                this.testStatus = 'Completed';
                break;
            case 1:
                this.testStatus = 'Auto Submit';
                break;
            case 2:
                this.testStatus = 'Blocked';
                break;
        }
    }

    //Calculates the no of correct and incorrect answers given by the candidate
    attendeeAnswers() {
        for (let question = 0; question < this.testQuestions.length; question++) {
            let options = this.testQuestions[question].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption;
            let oldAnswerStatus: boolean;
            let answerStatus: boolean;
            for (let option = 0; option < options.length; option++) {
                answerStatus = this.isAttendeeAnswerCorrect(options[option].id, options[option].isAnswer);
                if (this.testQuestions[question].question.questionType === 0) {
                    if (answerStatus === undefined)
                        answerStatus = oldAnswerStatus;
                    oldAnswerStatus = answerStatus;
                }
                else {
                    if (this.testQuestions[question].question.questionType === 1) {
                        let option = this.testQuestions[question].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption;
                        let answerCorrect = this.noOfAnswersCorrectGivenbyAttendee(option);
                        if (answerStatus === false ||(answerCorrect>0 && answerCorrect < 2)) {
                            answerStatus = false;
                            break;
                        }
                    }
                }
            }
            if (answerStatus === true) {
                this.correctAnswers++;
                this.testQuestions[question].questionStatus = 0;
                this.difficultywiseCorrectQuestions(this.testQuestions[question].question.difficultyLevel);
            }
            else {
                if (answerStatus === false) {
                    this.incorrectAnswers++;
                    this.testQuestions[question].questionStatus = 1;
                }
                else
                    this.testQuestions[question].questionStatus = 2;
            }
        }
    }

    /**
     * Calculates the number of correct answers given by a Test Attendee
     * @param options is an array of SingleMultipleAnswerQuestionOption Type
     */
    noOfAnswersCorrectGivenbyAttendee(options: Array<SingleMultipleAnswerQuestionOption>) {
        let noOfAnswersCorrect = 0;
        let isTestAttendeeAnswerCorrect: boolean;
        for (let option = 0; option < this.testAnswers.length; option++) {
            for (let i = 0; i < options.length; i++) {
                if (this.testAnswers[option].answeredOption === options[i].id ) {
                    if (options[i].isAnswer) {
                        isTestAttendeeAnswerCorrect = true;
                        noOfAnswersCorrect++;
                    }
                    else {
                        isTestAttendeeAnswerCorrect = false;
                        noOfAnswersCorrect++;
                    }
                }
            }
        };
        console.log(noOfAnswersCorrect);
        return noOfAnswersCorrect;
    }

    /**
     * Checks if the answers given by the candidate are correct or not
     * @param optionId is a number which contains the id of the option of a question
     * @param isAnswer is a boolean type variable for checking the answered option is correct or not
     */
    isAttendeeAnswerCorrect(optionId: number, isAnswer: boolean) {
        let isTestAttendeeAnswerCorrect: boolean;
        for (let option = 0; option < this.testAnswers.length; option++) {
            if (this.testAnswers[option].answeredOption === optionId) {
                if (isAnswer) {
                    isTestAttendeeAnswerCorrect = true;
                }
                else {
                    isTestAttendeeAnswerCorrect = false;
                }
            }
        };
        return isTestAttendeeAnswerCorrect;
    }

    /**
     * Checks for the corrrect option of a particular question
     * @param isAnswer is a boolean value which checks whether the answered option is correct or not
     */
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer)
            return true;
    }

    //Sets the values for the question pie chart(pie chart displayed for the complete test)
    public questionPieChartValue() {
        this.totalQuestions = this.testQuestions.length;
        this.notAttempted = this.totalQuestions - (this.correctAnswers + this.incorrectAnswers);
        return [this.correctAnswers, this.incorrectAnswers, this.notAttempted];
    }

    /**
     * Calculates the no of correct questions in each difficulty level
     * @param difficultyLevel is a number whose value determines the difficulty level of a question
     */
    difficultywiseCorrectQuestions(difficultyLevel: number) {
        switch (difficultyLevel) {
            case 0:
                this.easy++;
                break;
            case 1:
                this.medium++;
                break;
            case 2:
                this.hard++;
                break;
        }
    }

    //Sets the values of the pie chart that displays the no of correct answers in each difficulty level
    correctPieChartValue() {
        return [this.easy, this.medium, this.hard];
    }

    /**
   * Prints Individual report for a test attendee 
   * @param printSectionId is a string for getting the required element to be printed from the html
   */
    printIndividualReport(printSectionId: string) {
        this.loader = true;
        let elementToPrint = document.getElementById('printSectionId');
        let pdf = new jsPDF('p', 'mm', 'a4');
        let styles = {
            background: '#FFFFFF'
        };
        pdf.addHTML(elementToPrint, 0, 15, styles, () => {
           
            pdf.setProperties({
                title: 'Individual-test-report'
            });
            pdf.text(this.testAttendee.test.testName, 20, 13);
            pdf.setFontSize(5);
            pdf.autoPrint();
            pdf.output('dataurlnewwindow');
            this.loader = false;
        });
        
    }

    /**
     * Downloads the individual report of a test attendee in the form of a pdf
     * @param printSectionId is a string for getting the required element to be downloaded from the html
     */
    downloadIndividualReport(printSectionId: string) {
        this.loader = true;
        let dataToDownload = document.getElementById('printSectionId');
        let doc = new jsPDF('p', 'mm', 'a4');
        let styles = {
            background: '#FFFFFF',
        };
        doc.addHTML(dataToDownload, 0, 15, styles, () => {
            doc.setProperties({
                title: 'Individual-test-report'
            });
            doc.text(this.testAttendee.test.testName, 20, 13);
            doc.setFontSize(5);
            doc.save('individual-report.pdf');
            this.loader = false;
        });
    }
}

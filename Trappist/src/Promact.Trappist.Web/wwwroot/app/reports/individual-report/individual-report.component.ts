import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService } from '../report.service';
import { TestAttendee } from '../testattendee.model';
import { QuestionDisplay } from '../../questions/question-display';
import { TestQuestion } from '../../tests/tests.model';
import { TestAnswers } from '../testanswers.model';
import { SingleMultipleAnswerQuestionOption } from '../../questions/single-multiple-answer-question-option.model';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { TestLogs } from '../testlogs.model';
import { TestConduct } from '../testConduct.model';
import { QuestionType } from '../../questions/enum-questiontype';
import { CodeSnippetTestCasesDetails } from '../code-snippet-test-cases-details.model';
import { ProgrammingLanguage } from '../programminglanguage.enum';
import { TestCodeSolutionDetails } from '../test-code-solution-details.model';
import { QuestionStatus } from '../../conduct/question_status.enum';
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
    timeTakenInSeconds: number;
    easy: number;
    medium: number;
    hard: number;
    testLogs: TestLogs;
    testLogsVisible: boolean;
    closeWindowLogVisible: boolean;
    resumeTestLog: boolean;
    numberOfQuestionsAttempted: number;
    timeTakenInHoursVisible: boolean;
    timeTakenInMinutesVisible: boolean;
    timeTakenInSecondsVisible: boolean;
    awayFromTestWindowVisible: boolean;
    numberOfCorrectOptions: number;
    isPercentileVisible: boolean;
    currentDate: Date;
    individualReportPathContent: string;
    ProgrammingLanguage = ProgrammingLanguage;
    hideSign: boolean;
    showPieChart: boolean;
    isPercentageVisible: boolean;
    isScoreVisible: boolean;
    attendeeArray: number[];
    idOfTestAttendee: number;
    totalMarksOfTest: number;
    numberOfQuestionsInTest: number;

    constructor(private reportsService: ReportService, private route: ActivatedRoute, private router: Router) {
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
        this.currentDate = new Date();
        this.isPercentileVisible = false;
        this.isPercentageVisible = false;
        this.isScoreVisible = false;
        this.attendeeArray = new Array<number>();
    }

    ngOnInit() {
        this.getTestAttendeeDetails();
        this.individualReportPathContent = this.route.snapshot.params['download'];
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
            this.hideSign = +this.testAttendee.test.incorrectMarks === 0 ? true : false;
            this.marks = this.testAttendee.report.totalMarksScored;
            this.percentage = this.testAttendee.report.percentage;
            this.timeTakenInHours = Math.floor(this.testAttendee.report.timeTakenByAttendee / 3600);
            this.timeTakenInHoursVisible = this.timeTakenInHours > 1 ? true : false;
            this.timeTakenInMinutes = Math.floor(this.testAttendee.report.timeTakenByAttendee / 60);
            this.timeTakenInMinutesVisible = this.timeTakenInMinutes > 1 ? true : false;
            this.timeTakenInSeconds = this.testAttendee.report.timeTakenByAttendee % 60;
            this.timeTakenInSecondsVisible = this.timeTakenInSeconds > 0 ? true : false;

            this.testAttendee.testLogs.fillRegistrationForm = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.fillRegistrationForm);
            this.testAttendee.testLogs.visitTestLink = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.visitTestLink);
            this.testAttendee.testLogs.startTest = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.startTest);
            this.testAttendee.testLogs.finishTest = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.finishTest);

            this.testLogsVisible = this.testAttendee.testLogs.disconnectedFromServer === null ? false : true;
            this.closeWindowLogVisible = this.testAttendee.testLogs.closeWindowWithoutFinishingTest === null ? false : true;
            this.resumeTestLog = this.testAttendee.testLogs.resumeTest === null ? false : true;
            this.awayFromTestWindowVisible = this.testAttendee.testLogs.awayFromTestWindow === null ? false : true;

            if (this.awayFromTestWindowVisible)
                this.testAttendee.testLogs.awayFromTestWindow = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.awayFromTestWindow);

            if (this.closeWindowLogVisible)
                this.testAttendee.testLogs.closeWindowWithoutFinishingTest = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.closeWindowWithoutFinishingTest);

            if (this.testLogsVisible)
                this.testAttendee.testLogs.disconnectedFromServer = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.disconnectedFromServer);

            if (this.resumeTestLog)
                this.testAttendee.testLogs.resumeTest = this.convertTestLogsDateTimetoLocalDateTime(this.testAttendee.testLogs.resumeTest);


            this.reportsService.getTotalNumberOfAttemptedQuestions(this.testAttendeeId).subscribe((response) => {
                this.numberOfQuestionsAttempted = response;
            });

            this.reportsService.getStudentPercentile(this.testAttendeeId, this.testAttendee.test.id).subscribe((response) => {
                this.percentile = response.toFixed(2);
                this.isScoreVisible = true;
                this.isPercentageVisible = true;
                this.isPercentileVisible = true;
            });

            //Gets all the answers given by the test attendee
            this.reportsService.getTestAttendeeAnswers(this.testAttendee.id).subscribe((response) => {
                this.testAnswers = response;
                //Gets all the questions present in the test
                this.reportsService.getTestQuestions(this.testAttendee.test.id).subscribe((response) => {
                    this.testQuestions = response;
                    this.numberOfQuestionsInTest = this.testQuestions.length;
                    this.totalMarksOfTest = this.numberOfQuestionsInTest * Number(this.correctMarks);
                    this.testFinishStatus();
                    this.attendeeAnswers();
                    this.questionPieChartValue();
                    this.correctPieChartValue();
                    if (this.individualReportPathContent === 'download') {
                        setTimeout(() => {
                            this.downloadIndividualReport();
                        }, 5000);
                    }
                    else
                        this.loader = false;
                });
            });
        });
    }

    /**
     * Converts the test logs utc date-time to local date-time
     * @param date contains the values of the fields of test logs
     */
    private convertTestLogsDateTimetoLocalDateTime(date: Date) {
        let testLogsDateTime = new Date(date);
        let offset = testLogsDateTime.getTimezoneOffset();
        let hoursDiff = Math.trunc(offset / 60);
        let minutesDiff = Math.trunc(offset % 60);
        let localHours = testLogsDateTime.getHours() - hoursDiff;
        let localMinutes = testLogsDateTime.getMinutes() - minutesDiff;

        if (localMinutes >= 60) {
            let hours = Math.trunc(localMinutes / 60);
            let minutes = Math.trunc(localMinutes % 60);
            localHours += hours;
            localMinutes = minutes;
        }

        date = new Date(date);
        date.setHours(localHours);
        date.setMinutes(localMinutes);
        return date;
    }

    //Sets the test finish status of the candidate
    testFinishStatus() {
        switch (this.testAttendee.report.testStatus) {
            case 0:
                this.testStatus = 'Incomplete';
                break;
            case 1:
                this.testStatus = 'Completed';
                break;
            case 2:
                this.testStatus = 'Expired';
                break;
            case 3:
                this.testStatus = 'Blocked';
                break;
            case 4:
                this.testStatus = 'Unfinished';
                break;
        }
    }

    //Calculates the no of correct and incorrect answers given by the candidate
    attendeeAnswers() {

        for (let question = 0; question < this.testQuestions.length; question++) {
            if (this.testQuestions[question].question.questionType !== 2) {
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
                            option.forEach(x => {
                                let correctOptions = option.filter(x => x.isAnswer === true);
                                this.numberOfCorrectOptions = correctOptions.length;
                            });
                            let answerCorrect = this.noOfAnswersCorrectGivenbyAttendee(option);
                            if (answerStatus === false || (answerCorrect > 0 && answerCorrect < this.numberOfCorrectOptions)) {
                                answerStatus = false;
                                break;
                            }
                            else if (answerCorrect === this.numberOfCorrectOptions)
                                answerStatus = true;
                        }
                    }
                }
                if (answerStatus === true) {
                    this.correctAnswers++;
                    this.testQuestions[question].answerStatus = 0;
                    this.difficultywiseCorrectQuestions(this.testQuestions[question].question.difficultyLevel);
                }
                else {
                    if (answerStatus === false) {
                        this.incorrectAnswers++;
                        this.testQuestions[question].answerStatus = 1;
                    }
                    else
                        this.testQuestions[question].answerStatus = 2;
                }
            }
            else {
                let value = 0;
                this.reportsService.getCodeSnippetQuestionTestCasesDetails(this.testAttendeeId, this.testQuestions[question].questionId).subscribe((response) => {
                    this.testQuestions[question].codeSnippetQuestionTestCasesDetails = response;
                    this.testQuestions[question].isCodeSnippetTestCaseDetailsVisible = this.testQuestions[question].codeSnippetQuestionTestCasesDetails === null ? false : true;
                });
                this.reportsService.getCodeSnippetQuestionMarks(this.testAttendee.id, this.testQuestions[question].questionId).subscribe((response) => {
                    this.testQuestions[question].scoreOfCodeSnippetQuestion = response;
                    if (+this.testQuestions[question].scoreOfCodeSnippetQuestion < value) {
                        this.testQuestions[question].questionStatus = 3;
                        this.testQuestions[question].isCompilationStatusVisible = false;
                    }
                    else if (this.testQuestions[question].scoreOfCodeSnippetQuestion === this.testAttendee.test.correctMarks) {
                        this.correctAnswers++;
                        this.testQuestions[question].questionStatus = 0;
                        this.difficultywiseCorrectQuestions(this.testQuestions[question].question.difficultyLevel);
                        this.testQuestions[question].isCompilationStatusVisible = true;
                        this.testQuestions[question].compilationStatus = 'Successful';
                    }
                    else {
                        this.incorrectAnswers++;
                        if (+this.testQuestions[question].scoreOfCodeSnippetQuestion === 0)
                            this.testQuestions[question].answerStatus = 1;
                        else
                            this.testQuestions[question].questionStatus = 0;
                        this.testQuestions[question].isCompilationStatusVisible = true;
                        this.testQuestions[question].compilationStatus = 'Unsuccessful';
                    }
                });
                this.reportsService.getTestCodeSolutionDetails(this.testAttendee.id, this.testQuestions[question].questionId).subscribe((response) => {
                    this.testQuestions[question].testCodeSolutionDetails = response;
                    this.testQuestions[question].isCodeSolutionDetailsVisible = this.testQuestions[question].testCodeSolutionDetails === null ? false : true;
                    if (this.testQuestions[question].testCodeSolutionDetails === null) {
                        this.testQuestions[question].language = ProgrammingLanguage.c;
                        this.testQuestions[question].totalNumberOfAttemptsMadeByAttendee = 0;
                        this.testQuestions[question].numberOfSuccessfulAttemptsByAttendee = 0;
                        this.testQuestions[question].codeSolution = ' ';
                    }
                    else {
                        this.testQuestions[question].language = this.testQuestions[question].testCodeSolutionDetails.language;
                        this.testQuestions[question].totalNumberOfAttemptsMadeByAttendee = this.testQuestions[question].testCodeSolutionDetails.totalNumberOfAttempts;
                        this.testQuestions[question].numberOfSuccessfulAttemptsByAttendee = this.testQuestions[question].testCodeSolutionDetails.numberOfSuccessfulAttempts;
                        this.testQuestions[question].codeSolution = this.testQuestions[question].testCodeSolutionDetails.codeSolution;
                        this.testQuestions[question].codeToDisplay = this.testQuestions[question].codeSolution.replace('/\\n;', '&nbsp');
                    }
                });
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
                if (this.testAnswers[option].answeredOption === options[i].id) {
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
        this.showPieChart = (this.easy === 0 && this.medium === 0 && this.hard === 0) ? false : true;
    }

    //Sets the values of the pie chart that displays the number of correct answers in each difficulty level
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
        let height = elementToPrint.offsetHeight;
        let attendeeName = this.testAttendee.firstName + this.testAttendee.lastName;
        let testName = this.testAttendee.test.testName;
        let pdf = new jsPDF('p', 'mm', [187, height]);
        let styles = {
            background: '#FFFFFF',
            pagesplit: true
        };
        pdf.internal.scaleFactor = 6.55;
        pdf.text(this.testAttendee.test.testName, 15, 15);
        pdf.addHTML(elementToPrint, 0, 20, styles, () => {
            pdf.setProperties({
                title: testName + '_' + attendeeName + '.pdf'
            });

            pdf.setFontSize(5);
            pdf.autoPrint();
            window.open(pdf.output('bloburl'), '_blank');
            this.loader = false;
        });
    }

    /**
     * Downloads the individual report of a test attendee in the form of a pdf
     * @param printSectionId is a string for getting the required element to be downloaded from the html
     */
    downloadIndividualReport() {
        this.loader = true;
        let dataToDownload = document.getElementById('printSectionId');
        let attendeeName = this.testAttendee.firstName + this.testAttendee.lastName;
        let testName = this.testAttendee.test.testName;

        let height = dataToDownload.offsetHeight;
        let doc = new jsPDF('p', 'mm', [187, height]);
        let styles = {
            background: '#FFFFFF',
            pagesplit: true
        };
        doc.text(this.testAttendee.test.testName, 15, 15);
        doc.addHTML(dataToDownload, 0, 20, styles, () => {
            doc.setProperties({
                title: testName + '_' + attendeeName + '.pdf'
            });
            doc.setFontSize(5);
            doc.save(testName + '_' + attendeeName + '.pdf');
            this.loader = false;
        });
    }

    /**
     * Navigates the user to the previous individual test report
     */
    moveToPreviousIndividualReport() {
        this.loader = true;
        this.reportsService.getAttendeeIdList(this.testId).subscribe((response) => {
            this.attendeeArray = response;
            let index = this.attendeeArray.indexOf(Number(this.testAttendeeId));
            if (index === 0) {
                index = this.attendeeArray.length - 1;
                this.idOfTestAttendee = this.attendeeArray[index];
                window.location.href = window.location.origin + '/reports/test/' + this.testId + '/individual-report/' + this.idOfTestAttendee;
            }
            else if (index > 0) {
                index = index - 1;
                this.idOfTestAttendee = this.attendeeArray[index];
                window.location.href = window.location.origin + '/reports/test/' + this.testId + '/individual-report/' + this.idOfTestAttendee;
            }
        });
    }

    /**
     * Navigates the user to the next individual test report
     */
    moveToNextIndividualReport() {
        this.loader = true;
        this.reportsService.getAttendeeIdList(this.testId).subscribe((response) => {
            this.attendeeArray = response;
            let index = this.attendeeArray.indexOf(Number(this.testAttendeeId));
            if (index === (this.attendeeArray.length - 1)) {
                this.idOfTestAttendee = this.attendeeArray[0];
                window.location.href = window.location.origin + '/reports/test/' + this.testId + '/individual-report/' + this.idOfTestAttendee;
            }
            else if (index === 0 || index < (this.attendeeArray.length - 1)) {
                index = index + 1;
                this.idOfTestAttendee = this.attendeeArray[index];
                window.location.href = window.location.origin + '/reports/test/' + this.testId + '/individual-report/' + this.idOfTestAttendee;
            }
        });
    }
}

import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TestAttendee } from '../testAttendee';
import { TestStatus } from '../enum-test-state';
import { Test } from '../../tests/tests.model';
import { ConductService } from '../../conduct/conduct.service';
import { TestInstructions } from '../../conduct/testInstructions.model';
import { ReportQuestionsCount } from './reportquestionscount';
import { TestAttendeeRank } from './testattendeerank';
import * as Excel from 'exceljs/dist/exceljs.min.js';
import { AllowTestResume } from '../../tests/enum-allowtestresume';
import { TestLogs } from '../testlogs.model';
import { MdSnackBar, MdSnackBarRef } from '@angular/material';

declare let jsPDF: any;
declare let saveAs: any;

@Component({
    moduleId: module.id,
    selector: 'test-report',
    templateUrl: 'test-report.html'
})

export class TestReportComponent implements OnInit {
    routeForIndividualTestReport: any;
    showSearchInput: boolean;
    searchString: string;
    testAttendeeArray: TestAttendee[];
    attendeeArray: TestAttendee[];
    sortedAttendeeArray: TestAttendee[];
    testId: number;
    starredCandidateArray: string[];
    testCompletionStatus: string;
    count: number;
    selectedTestStatus: TestStatus;
    headerStarStatus: string;
    rowStarStatus: string;
    activePage: number;
    starredCandidateIdList: number[];
    isAllCandidateStarred: boolean;
    showStarCandidates: boolean;
    loader: boolean;
    test: Test;
    isAnyCandidateExist: boolean;
    checkedAllCandidate: boolean;
    isAnyCandidateSelected: boolean;
    testFinishStatus: string;
    domain: string;
    reportLink: string;
    maxScore: number;
    averageTestScore: number;
    averageTimeTaken: number;
    averageCorrectAttempt: number;
    totalNoOfTestQuestions: number;
    maxDuration: number;
    testInstruction: TestInstructions;
    attendeeRank: number;
    reportQuestionDetails: ReportQuestionsCount[];
    testAttendeeRank: TestAttendeeRank[];
    testLogs: TestLogs[] = [];

    constructor(private reportService: ReportService, private route: ActivatedRoute, private conductService: ConductService, private router: Router, private snackbarRef: MdSnackBar) {
        this.testAttendeeArray = new Array<TestAttendee>();
        this.attendeeArray = new Array<TestAttendee>();
        this.sortedAttendeeArray = new Array<TestAttendee>();
        this.testCompletionStatus = '0';
        this.selectedTestStatus = TestStatus.allCandidates;
        this.searchString = '';
        this.headerStarStatus = 'star_border';
        this.rowStarStatus = 'star_border';
        this.activePage = 1;
        this.starredCandidateIdList = new Array<number>();
        this.isAllCandidateStarred = false;
        this.showStarCandidates = false;
        this.test = new Test();
        this.isAnyCandidateExist = false;
        this.loader = true;
        this.checkedAllCandidate = false;
        this.isAnyCandidateSelected = false;
        this.maxScore = 0;
        this.maxDuration = 0;
        this.totalNoOfTestQuestions = 0;
        this.attendeeRank = 0;
        this.testInstruction = new TestInstructions();
        this.reportQuestionDetails = new Array<ReportQuestionsCount>();
        this.testAttendeeRank = new Array<TestAttendeeRank>();
    }

    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestName();
        this.getAllTestCandidates();
        this.domain = window.location.origin;
    }

    /**
     * Fetches test name of particular test
     */
    getTestName() {
        this.reportService.getTestName(this.testId).subscribe((test) => {
            this.test = test;
            this.conductService.getTestInstructionsByLink(this.test.link).subscribe((response) => {
                this.testInstruction = response;
                this.totalNoOfTestQuestions = this.testInstruction.totalNumberOfQuestions;
            });
            this.reportService.getAllAttendeeMarksDetails(this.testId).subscribe(res => {
                this.reportQuestionDetails = res;
            });
        });
    }

    /**
     * Fetches all the candidates of any particular test
     */
    getAllTestCandidates() {
        this.loader = true;
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.attendeeArray = attendeeList;
            this.isAnyCandidateExist = this.attendeeArray.length === 0 ? false : true;
            this.attendeeArray.forEach(x => {
                if (x.report !== null)
                    this.testAttendeeArray.push(x);
            });
            this.attendeeArray = this.testAttendeeArray;
            [this.headerStarStatus, this.isAllCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false] : ['star', true];
            this.countAttendees();
            this.loader = false;
        });
    }

    /**
     * Sets all the candidates as starred candidates
     * @param testId
     */
    setAllCandidatesStarred() {
        let status = this.headerStarStatus === 'star_border';
        if (this.testAttendeeArray.length > 0) {
            this.reportService.setAllCandidatesStarred(status, this.searchString, this.selectedTestStatus).subscribe(
                response => {
                    this.testAttendeeArray.forEach(k => k.starredCandidate = status);
                    [this.headerStarStatus, this.isAllCandidateStarred] = status ? ['star', true] : ['star_border', false];
                }
            );
        }
    }

    /**
     * Toggles the star condition of a candidate
     * @param testAttendee
     */
    setStarredCandidate(testAttendee: TestAttendee) {
        this.reportService.setStarredCandidate(testAttendee.id).subscribe(
            response => {
                this.testAttendeeArray.find(x => x.id === testAttendee.id).starredCandidate = !testAttendee.starredCandidate;
                [this.headerStarStatus, this.isAllCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false] : ['star', true];
            }
        );
    }
    /**
     * resumes test if Allow test resume is supervised
     * @param attendee
     */
    resumeTest(attendee: TestAttendee) {
        this.reportService.createSessionForAttendee(attendee, this.test.link).subscribe(response => {
            if (response) {
                attendee.report.isTestPausedUnWillingly = false;
                this.snackbarRef.open('Test resumed successfully', 'Dismiss', {
                    duration: 4000,
                });
            }
        });
    }

    /**
     * Checks whther a candidate is satrred
     * @param attendee
     */
    isStarredCandidate(attendee: TestAttendee) {
        return attendee.starredCandidate ? 'star' : 'star_border';
    }

    /**
     * Displays only the starred candidates
     */
    showAllStarredCandidates() {
        this.showStarCandidates = !this.showStarCandidates;
        this.filterList();
    }

    /**
     * Sets the search criteria
     * @param searchString
     */
    getTestAttendeeMatchingSearchCriteria(searchString: string) {
        this.searchString = searchString;
        this.filterList();
    }

    /**
     * To determine whether search field will be visible or hidden
     */
    showStatus() {
        return this.showSearchInput = this.searchString.length > 0;
    }

    /**
     * Sets the test status of the candidate
     * @param typeOfTest
     */
    setTestStatusType(testCompletionStatus: string) {
        this.selectedTestStatus = +testCompletionStatus;
        this.filterList();
    }

    /**
     * Initiates filtering of candidates
     */
    filterList() {
        this.filter(this.selectedTestStatus, this.searchString, this.showStarCandidates);
        [this.headerStarStatus, this.isAllCandidateStarred] = (this.testAttendeeArray.some(x => !x.starredCandidate) || (this.testAttendeeArray.length === 0)) ? ['star_border', false] : ['star', true];
        this.countAttendees();
    }

    /**
     * Filters the test attendee list based on test status of the candidate and search string provided by user
     * @param selectedTestStatus
     * @param searchString
     */
    filter(selectedTestStatus: TestStatus, searchString: string, showStarCandidates: boolean) {
        let tempAttendeeArray: TestAttendee[] = [];
        searchString = searchString.toLowerCase();
        this.testAttendeeArray = [];
        let starAttendeeArray: TestAttendee[] = [];

        if (this.showStarCandidates) {
            this.attendeeArray.forEach(k => {
                if (k.starredCandidate)
                    starAttendeeArray.push(k);
            });
        }

        if (!this.showStarCandidates) {
            this.attendeeArray.forEach(k => {
                starAttendeeArray.push(k);
            });
        }

        if (selectedTestStatus === 0) {
            starAttendeeArray.forEach(x => {
                tempAttendeeArray.push(x);
            });
        }

        starAttendeeArray.forEach(x => {
            if (x.report.testStatus === selectedTestStatus) {
                tempAttendeeArray.push(x);
            }
        });

        tempAttendeeArray.forEach(x => {
            {
                if (x.email.toLowerCase().includes(searchString)
                    || x.firstName.toLowerCase().includes(searchString)
                    || x.lastName.toLowerCase().includes(searchString)) {
                    this.testAttendeeArray.push(x);
                }
            }
        });
    }

    /**
     * Counts the number of candidates
     */
    countAttendees() {
        return this.count = this.testAttendeeArray.length;
    }

    /**
     * Keeps tarck of starting index of the active page
     * @param activePage
     * @param count
     */
    startingIndexOfActivePage(activePage: number, count: number) {
        return count === 0 ? 0 : (activePage - 1) * 10 + 1;
    }

    /**
     * Keeps track of the last index of the active page
     * @param activePage
     * @param count
     */
    endingIndexOfActivePage(activePage: number, count: number) {
        let pagetracker = activePage * 10;
        return pagetracker > count ? count : pagetracker;
    }

    /**
     * Download test report in  pdf format
     */
    downloadTestReportPdf() {
        this.loader = true;
        let testName = this.test.testName;
        let reports = new Array();
        let space = ' ';
        if (!this.checkedAllCandidate) {
            this.isAnyCandidateSelected = this.testAttendeeArray.some(x => {
                return x.checkedCandidate;
            });
            if (!this.isAnyCandidateSelected) {
                this.selectAllCandidates();
            }
        }
        this.testAttendeeArray.forEach(x => {
            if (x.checkedCandidate) {
                let testDate = document.getElementById('date').innerHTML;
                let report = {
                    'name': x.firstName + space + x.lastName,
                    'email': x.email,
                    'createdDateTime': testDate,
                    'score': x.report.totalMarksScored,
                    'percentage': x.report.percentage
                };
                reports.push(report);
            }
        });
        let columns = [
            { title: 'Name', dataKey: 'name' },
            { title: 'Email', dataKey: 'email' },
            { title: 'Test Date', dataKey: 'createdDateTime' },
            { title: 'Score', dataKey: 'score' },
            { title: 'Percentage', dataKey: 'percentage' },];
        let doc = new jsPDF('p', 'pt');
        doc.autoTable(columns, reports, {
            styles: {
                pageBreak: 'auto',
                tableWidth: 'auto',
            },
            margin: { top: 50 },
            theme: 'grid',
            addPageContent: function () {
                doc.text(testName, 40, 30);
            }
        });
        doc.save(testName + '_test_report.pdf');
        this.checkedAllCandidate = false;
        this.testAttendeeArray.forEach(x => {
            x.checkedCandidate = false;
        });
        this.loader = false;
    }

    /**
     * Download test report in excel format
     */
    downloadTestReportExcel() {
        this.loader = true;
        let testName = this.test.testName;
        let space = ' ';
        let workBook = new Excel.Workbook();
        workBook.views = [ {
            x: 0, y: 0, width: 10000, height: 20000,
            firstSheet: 0, visibility: 'visible'
        }];
        let workSheet1 = workBook.addWorksheet('Test-Takers', {
            pageSetup: { paperSize: 9, orientation: 'landscape' }
        });
        let workSheet2 = workBook.addWorksheet('Test-Summary', {
            pageSetup: { paperSize: 9, orientation: 'landscape' }
        });
        let workSheet3 = workBook.addWorksheet('Marks-Scored', {
            pageSetup: { paperSize: 9, orientation: 'landscape' }
        });
        workSheet1.columns = [
            { header: 'ROLL NO', key: 'rollNo', width: 10 },
            { header: 'NAME', key: 'name', width: 30 },
            { header: 'EMAIL ID', key: 'email', width: 30 },
            { header: 'CONTACT NO', key: 'contact', width: 30 },
            { header: 'TEST DATE', key: 'testDate', width: 30 },
            { header: 'TEST TIME', key: 'testTime', width: 30 },
            { header: 'FINISH STATUS', key: 'testStatus', width: 15 },
            { header: 'OVERALL MARKS', key: 'totalMarks', width: 15 },
            { header: 'REPORT LINK', key: 'reportLink', width: 60 }
        ];
        workSheet2.columns = [
            { header: 'MAXIMUM SCORE', key: 'maxScore', width: 20 },
            { header: 'TOTAL NO OF QUESTIONS', key: 'totalQ', width: 25 },
            { header: 'MAXIMUM DURATION', key: 'maxDuration', width: 25 },
            { header: 'AVERAGE SCORE', key: 'avgScore', width: 25 },
            { header: 'AVERAGE TIME TAKEN(MIN)', key: 'avgTotalTime', width: 25 },
            { header: 'AVERAGE NO OF CORRECT ATTEMPTS', key: 'avgCorrectAttempts', width: 35 }
        ];
        workSheet3.columns = [
            { header: 'ROLL NO', key: 'rollNo', width: 15 },
            { header: 'NAME', key: 'name', width: 30 },
            { header: 'EMAIL ID', key: 'email', width: 30 },
            { header: 'EASY QUESTION ATTEMPTED', key: 'easyQ', width: 27 },
            { header: 'MEDIUM QUESTION ATTEMPTED', key: 'mediumQ', width: 30 },
            { header: 'DIFFICULT QUESTION ATTEMPTED', key: 'difficultQ', width: 30 },
            { header: 'TOTAL QUESTION ATTEMPTED', key: 'totalQ', width: 27 },
            { header: 'NO OF CORRECT ATTEMPTED', key: 'coorectQ', width: 27 },
            { header: 'TIME TAKEN(SEC) ', key: 'time', width: 20 },
            { header: 'OVERALL MARKS', key: 'totalScore', width: 20 },
            { header: 'PERCENTAGE', key: 'percentage', width: 20 },
            { header: 'PERCENTILE', key: 'percentile', width: 20 },
            { header: 'CANDIDATE RANK', key: 'rank', width: 20 },

        ];
        if (!this.checkedAllCandidate) {
            this.isAnyCandidateSelected = this.testAttendeeArray.some(x => {
                return x.checkedCandidate;
            });
            if (!this.isAnyCandidateSelected) {
                this.selectAllCandidates();
            }
        }
        for (let i = 0; i < this.testAttendeeArray.length; i++) {

            this.sortedAttendeeArray[i] = this.testAttendeeArray[i];
        }
        this.sortedAttendeeArray = this.sortedAttendeeArray.sort((a, b) => b.report.totalMarksScored - a.report.totalMarksScored);
        this.maxScore = this.sortedAttendeeArray[0].report.totalMarksScored;
        this.caculateAttendeeRank();
        this.calculateTestSummaryDetails();
        workSheet2.addRow({
            maxScore: this.maxScore, totalQ: this.totalNoOfTestQuestions, maxDuration: this.maxDuration, avgScore: this.averageTestScore,
            avgTotalTime: this.averageTimeTaken, avgCorrectAttempts: this.averageCorrectAttempt
        });
        this.testAttendeeArray.forEach(x => {
            if (x.checkedCandidate) {
                let testDate = document.getElementById('date').innerHTML;
                let datetime = new Date(x.createdDateTime);
                let hours = datetime.getHours();
                let minitues = datetime.getMinutes();
                let attendeeId = x.id;
                this.testTakerDetails(x.report.testStatus, this.testId, x.id);
                let testTakers = {
                    'rollNo': x.rollNumber,
                    'name': x.firstName + space + x.lastName,
                    'email': x.email,
                    'contact': x.contactNumber,
                    'testDate': testDate,
                    'testTime': hours + ':' + minitues,
                    'testStatus': this.testFinishStatus,
                    'totalMarks': x.report.totalMarksScored
                };
                workSheet1.addRow({
                    rollNo: testTakers.rollNo, name: testTakers.name, email: testTakers.email, contact: testTakers.contact, testDate: testTakers.testDate,
                    testTime: testTakers.testTime, testStatus: testTakers.testStatus, totalMarks: testTakers.totalMarks, reportLink: this.reportLink
                });
                let testReport = {
                    'rollNo': x.rollNumber,
                    'name': x.firstName + space + x.lastName,
                    'email': x.email,
                    'timetaken': x.report.timeTakenByAttendee,
                    'percentage': x.report.percentage,
                    'percentile': x.report.percentile,
                    'totalMarks': x.report.totalMarksScored
                };
                this.testAttendeeRank.forEach(y => {
                    if (y.attendeeId === x.id)
                        this.attendeeRank = y.attendeeRank;
                });
                this.reportQuestionDetails.filter(y => {
                    if (y.testAttendeeId === x.id) {
                        workSheet3.addRow({
                            rollNo: testReport.rollNo, name: testReport.name, email: testReport.email, easyQ: y.easyQuestionAttempted, mediumQ: y.mediumQuestionAttempted, difficultQ: y.hardQuestionAttempted, totalQ: y.noOfQuestionAttempted,
                            coorectQ: y.correctQuestionsAttempted, time: testReport.timetaken, totalScore: testReport.totalMarks, percentage: testReport.percentage,
                            percentile: y.percentile, rank: this.attendeeRank
                        });
                    }
                });
            }
        });
        this.loader = false;
        workBook.xlsx.writeBuffer(workBook).then(function (buffer: any) {
            let blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64' });
            saveAs(blob, testName + '_test_report.xlsx');
        });
        this.checkedAllCandidate = false;
        this.testAttendeeArray.forEach(x => {
            x.checkedCandidate = false;
        });
    }

    /**
     * Select and deselect all candidates of a test
     */
    selectAllCandidates() {
        if (this.checkedAllCandidate || !this.isAnyCandidateSelected) {
            this.testAttendeeArray.forEach(k => {
                k.checkedCandidate = true;
            });
            this.isAnyCandidateSelected = true;
            this.checkedAllCandidate = true;
        }
        else
            this.testAttendeeArray.forEach(k => {
                k.checkedCandidate = false;
            });
    }

    /**
     * Select individual candidate of a test
     * @param testAttendee is object of the testAttendee table
     * @param select is the checkbox value of the checked candidate
     */
    selectIndividualCandidate(testAttendee: TestAttendee, select: boolean) {
        testAttendee.checkedCandidate = select;

    }

    /**
     * Calculate testfinish status and generate individual report link for a every attendee 
     * @param testStatus status of the test
     * @param testID id of the test
     * @param testAttendeeId id of an attendee
     */

    testTakerDetails(testStatus: number, testID: number, testAttendeeId: number) {
        switch (testStatus) {
            case 1:
                this.testFinishStatus = 'Completed';
                break;
            case 2:
                this.testFinishStatus = 'Expired';
                break;
            case 3:
                this.testFinishStatus = 'Blocked';
        }
        this.reportLink = this.domain + '/reports/test/' + testID + '/individual-report/' + testAttendeeId;

    }

    /**
     * calculate testSummary details of a particular test
     */
    calculateTestSummaryDetails() {
        let totalTimeByAllAttendees = 0;
        let totalScoreOfAllAttendees = 0;
        let totalCorrectAttemptByAllAttendees = 0;
        let totalTestScore = 0;
        let totalAttendee = this.testAttendeeArray.length;
        this.testAttendeeArray.forEach(x => {
            totalTimeByAllAttendees += x.report.timeTakenByAttendee;
            totalScoreOfAllAttendees += x.report.totalMarksScored;
            if (this.maxDuration < x.report.timeTakenByAttendee)
                this.maxDuration = x.report.timeTakenByAttendee;
        });
        this.reportQuestionDetails.forEach(x => {
            totalCorrectAttemptByAllAttendees += x.correctQuestionsAttempted;
        });
        this.averageCorrectAttempt = totalCorrectAttemptByAllAttendees / this.totalNoOfTestQuestions;
        this.averageTestScore = totalScoreOfAllAttendees / totalAttendee;
        this.averageTimeTaken = totalTimeByAllAttendees / (this.test.duration * 60);
    }

    /**
     * calculate all attendee ranks based on their totalmarks
     */
    caculateAttendeeRank() {
        let rank = 1;
        let previousScore = 0;
        for (let i = 0; i < this.sortedAttendeeArray.length; i++) {
            let attendeeId = this.sortedAttendeeArray[i].id;
            let newScore = this.sortedAttendeeArray[i].report.totalMarksScored;
            if (previousScore > newScore) {
                rank += 1;
                previousScore = newScore;
            }
            else
                previousScore = newScore;
            let testRankDetails = new TestAttendeeRank();
            testRankDetails.attendeeId = attendeeId;
            testRankDetails.attendeeRank = rank;
            this.testAttendeeRank.push(testRankDetails);
        }
    }

    /**
     * navigates to the individual test report component 
     * @param testAttendeeId contains the value of the test attendee Id from the route
     */
    navigateToIndividualReportPage(testAttendeeId: number) {
        this.routeForIndividualTestReport = 'reports/test/' + this.testId;
        this.router.navigate(['/individual-report/', testAttendeeId, '/download'], { relativeTo: this.routeForIndividualTestReport });
    }
}
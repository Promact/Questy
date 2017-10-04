import { Component, OnInit, ViewChild } from '@angular/core';
import { ReportService } from '../report.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TestAttendee } from '../testAttendee';
import { TestStatus } from '../enum-test-state';
import { Test } from '../../tests/tests.model';
import { ConductService } from '../../conduct/conduct.service';
import { TestInstructions } from '../../conduct/testInstructions.model';
import { ReportQuestionsCount } from './reportquestionscount';
import { TestAttendeeRank } from './testattendeerank';
import { AllowTestResume } from '../../tests/enum-allowtestresume';
import { TestLogs } from '../testlogs.model';
import { MdSnackBar, MdSnackBarRef } from '@angular/material';
import { Report } from '../report.model';
import { ConnectionService } from '../../core/connection.service';



declare let jsPDF: any;
declare let saveAs: any;
declare let ExcelJS: any;

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
    isAnyTestResume: boolean;
    showGenerateReportButton: boolean;
    isGeneratingReport: boolean;
    testTime: string;
    maxDurationOfTest: string;
    allCount: number;
    completedTestCount: number;
    expiredTestCount: number;
    blockedTestCount: number;
    unfinishedTestCount: number;
    starredCandidateCount: number;
    noCandidateFound: boolean;
    attendees: TestAttendee[] = [];
    testAttendeeSignalR: TestAttendee;


    constructor(private reportService: ReportService, private route: ActivatedRoute, private conductService: ConductService, private router: Router, private snackbarRef: MdSnackBar, private connectionService: ConnectionService) {
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
        this.completedTestCount = 0;
        this.blockedTestCount = 0;
        this.expiredTestCount = 0;
        this.unfinishedTestCount = 0;
        this.starredCandidateCount = 0;
        this.testAttendeeSignalR = new TestAttendee();

    }

    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.loader = true;
        this.getTestName();
        this.domain = window.location.origin;
    }

    /**
     * Fetches test name of particular test
     */
    getTestName() {
        this.reportService.getTestName(this.testId).subscribe((test) => {
            this.test = test;
        });

        this.getAllTestCandidates();
        this.connectionService.recievedAttendee.subscribe(attendee => {
            let testAttendee = this.testAttendeeArray.find(x => x.id === attendee.id);
            if (testAttendee !== undefined)
                this.testAttendeeArray.splice(this.testAttendeeArray.findIndex(x => x.id === testAttendee.id), 1);
            if (attendee.report === null)
                attendee.report = new Report();
            this.testAttendeeArray.unshift(attendee);
        });
        this.connectionService.recievedAttendeeId.subscribe(attendeeId => {
            let attendee = this.testAttendeeArray.find(x => x.id === attendeeId);
            attendee.report.isTestPausedUnWillingly = true;
            this.testAttendeeArray.splice(this.testAttendeeArray.findIndex(x => x.id === attendeeId), 1);
            this.testAttendeeArray.unshift(attendee);
            this.isAnyTestResume = true;
        });
    }


    /**
     * Fetches all the candidates of any particular test
     */
    getAllTestCandidates() {
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.attendeeArray = attendeeList;
            this.isAnyCandidateExist = this.attendeeArray.length === 0 ? false : true;
            this.attendeeArray.forEach(x => {
                if (x.report !== null)
                    this.testAttendeeArray.push(x);
                else {
                    x.report = new Report();
                    x.reporNotFoundYet = true;
                    this.testAttendeeArray.push(x);
                }
            });
            this.isAnyTestResume = this.testAttendeeArray.some(x => x.report.isTestPausedUnWillingly);
            this.attendeeArray = this.testAttendeeArray;
            this.isAnyCandidateExist = this.attendeeArray.some(x => x.report !== null);
            [this.headerStarStatus, this.isAllCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false] : ['star', true];
            this.countAttendees();
            this.testStatusWiseCountAttendees();
            this.allCount = this.count;
            this.loader = false;
        });
    }

    /**
     * Sets all the candidates as starred candidates
     * @param testId: Id of a test
     */
    setAllCandidatesStarred() {
        let status = this.headerStarStatus === 'star_border';
        if (this.testAttendeeArray.length > 0) {
            this.reportService.setAllCandidatesStarred(status, this.searchString, this.selectedTestStatus).subscribe(
                response => {
                    this.testAttendeeArray.forEach(k => k.starredCandidate = status);
                    [this.headerStarStatus, this.isAllCandidateStarred] = status ? ['star', true] : ['star_border', false];
                    if (status)
                        this.starredCandidateCount = this.testAttendeeArray.length;
                    else
                        this.starredCandidateCount = 0;
                }
            );
        }
    }

    /**
     * Toggles the star condition of a candidate
     * @param testAttendee: Object of TestAttendee type 
     */
    setStarredCandidate(testAttendee: TestAttendee) {
        this.reportService.setStarredCandidate(testAttendee.id).subscribe(
            response => {
                this.testAttendeeArray.find(x => x.id === testAttendee.id).starredCandidate = !testAttendee.starredCandidate;
                [this.headerStarStatus, this.isAllCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false] : ['star', true];
                if (testAttendee.starredCandidate)
                    this.starredCandidateCount += 1;
                else
                    this.starredCandidateCount -= 1;
            }
        );
    }
    /**
     * Resumes test if Allow test resume is supervised
     * @param attendee: Object of TestAttendee type
     */
    resumeTest(attendee: TestAttendee) {
        this.reportService.createSessionForAttendee(attendee, this.test.link, false).subscribe(response => {
            if (response) {
                attendee.report.isTestPausedUnWillingly = false;
                this.connectionService.sendReport(response);
                this.isAnyTestResume = this.testAttendeeArray.some(x => x.report.isTestPausedUnWillingly);
                this.snackbarRef.open('Test resumed successfully', 'Dismiss', {
                    duration: 4000,
                });
            }
        });
    }

    /**
     * Checks whther a candidate is satrred
     * @param attendee: Object of TestAttendee type
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
     * @param searchString: String that needs to be searched 
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
     * Sets the test completion status to fliter accordingly
     * @param testCompletionStatus: Value of test-completion-status
     */
    setTestStatusType(testCompletionStatus: string) {
        this.showGenerateReportButton = false;
        if (testCompletionStatus === 'star') {
            this.selectedTestStatus = TestStatus.allCandidates;
            this.showAllStarredCandidates();
        }
        else {
            this.showStarCandidates = false;
            this.selectedTestStatus = +testCompletionStatus;
            this.filterList();
        }
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
     * @param selectedTestStatus Status selected for filtering
     * @param searchString String that to be searched
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
            this.starredCandidateCount = starAttendeeArray.length;
            if (this.starredCandidateCount === 0)
                this.noCandidateFound = true;
            else
                this.noCandidateFound = false;
        } else {
            this.attendeeArray.forEach(k => {
                starAttendeeArray.push(k);
            });
        }
        switch (selectedTestStatus) {
            case 0:
                starAttendeeArray.forEach(x => {
                    tempAttendeeArray.push(x);
                });
                if (tempAttendeeArray.length !== 0)
                    this.noCandidateFound = false;
                break;
            case 4:
                starAttendeeArray.forEach(x => {
                    if (x.report.testStatus === TestStatus.allCandidates || x.report.testStatus === TestStatus.unfinishedTest) {
                        tempAttendeeArray.push(x);
                    }
                });
                this.showGenerateReportButton = tempAttendeeArray.some(x => x.report.totalMarksScored === null);
                if (tempAttendeeArray.length === 0)
                    this.noCandidateFound = true;
                else
                    this.noCandidateFound = false;
                break;
            default:
                starAttendeeArray.forEach(x => {
                    if (x.report.testStatus === selectedTestStatus) {
                        tempAttendeeArray.push(x);
                    }
                });
                if (tempAttendeeArray.length === 0)
                    this.noCandidateFound = true;
                else
                    this.noCandidateFound = false;
        }

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
     * @param activePage: Number of the page which is active at current moment
     * @param count: Total number of attendees in a test 
     */
    startingIndexOfActivePage(activePage: number, count: number) {
        return count === 0 ? 0 : (activePage - 1) * 10 + 1;
    }

    /**
     * Keeps track of the last index of the active page
     * @param activePage: Number of the page which is active at current moment
     * @param count: Total number of attendees in a test
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
        doc.save(testName + '_Report.pdf');
        this.checkedAllCandidate = false;
        this.testAttendeeArray.forEach(x => {
            x.checkedCandidate = false;
        });
        this.loader = false;
    }

    /**
     * Get excel data for all students at the time of download
     */
    getExcelDetails() {
        this.loader = true;
        this.reportService.getAllAttendeeMarksDetails(this.testId).subscribe(res => {
            this.reportQuestionDetails = res;
            this.loader = false;
            this.downloadTestReportExcel();
        });
    }

    /**
     * Download test score report in excel format
     */
    downloadTestReportExcel() {
        let testName = this.test.testName;
        let space = ' ';
        let workBook = new ExcelJS.Workbook();
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
            { header: 'REPORT LINK', key: 'reportLink', width: 100 }
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
            { header: 'TIME TAKEN', key: 'time', width: 20 },
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
        this.calculateAttendeeRank();
        this.testAttendeeArray.forEach(x => {
            if (x.checkedCandidate) {
                let testDate = document.getElementById('date').innerHTML;
                let datetime = new Date(x.createdDateTime);
                this.calculateLocalTime(datetime);
                let attendeeId = x.id;
                this.testTakerDetails(x.report.testStatus, this.testId, x.id);
                let testTakers = {
                    'rollNo': x.rollNumber,
                    'name': x.firstName + space + x.lastName,
                    'email': x.email,
                    'contact': x.contactNumber,
                    'testDate': testDate,
                    'testTime': this.testTime,
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
                        let timeTaken = Math.trunc(testReport.timetaken / 60) + ' mins ' + Math.trunc(Math.floor(testReport.timetaken % 60)) + ' secs ';
                        workSheet3.addRow({
                            rollNo: testReport.rollNo, name: testReport.name, email: testReport.email, easyQ: y.easyQuestionAttempted, mediumQ: y.mediumQuestionAttempted, difficultQ: y.hardQuestionAttempted, totalQ: y.noOfQuestionAttempted,
                            coorectQ: x.report.totalCorrectAttempts, time: timeTaken, totalScore: testReport.totalMarks, percentage: testReport.percentage,
                            percentile: y.percentile, rank: this.attendeeRank
                        });
                    }
                    this.totalNoOfTestQuestions = y.totalTestQuestions;
                });
            }
        });
        this.calculateTestSummaryDetails();
        workSheet2.addRow({
            maxScore: this.maxScore, totalQ: this.totalNoOfTestQuestions, maxDuration: this.maxDurationOfTest, avgScore: this.averageTestScore,
            avgTotalTime: this.averageTimeTaken, avgCorrectAttempts: this.averageCorrectAttempt
        });
        workBook.xlsx.writeBuffer(workBook).then(function (buffer: any) {
            let blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64' });
            saveAs(blob, testName + '_Detailed_Report.xlsx');
        });
        this.checkedAllCandidate = false;
        this.testAttendeeArray.forEach(x => {
            x.checkedCandidate = false;
        });
    }

    /**
     * Convert utc time into localtime
     * @param dateTime: Time of test
     */
    calculateLocalTime(dateTime: Date) {
        let offset = dateTime.getTimezoneOffset();
        let hoursDiff = Math.trunc(offset / 60);
        let minutesDiff = Math.trunc(offset % 60);
        let localHours = dateTime.getHours() - hoursDiff;
        let localMinutes = dateTime.getMinutes() - minutesDiff;
        if (localMinutes >= 60) {
            let hours = Math.trunc(localMinutes / 60);
            let minutes = Math.trunc(localMinutes % 60);
            localHours += hours;
            localMinutes = minutes;
        }
        this.testTime = localHours + ' : ' + localMinutes;
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
     * @param testAttendee: Object of the testAttendee table
     * @param select: Checkbox value of the checked candidate
     */
    selectIndividualCandidate(testAttendee: TestAttendee, select: boolean) {
        testAttendee.checkedCandidate = select;

    }

    /**
     * Calculate testfinish status and generate individual report link for a every attendee 
     * @param testStatus: Status of the test
     * @param testID: Id of the test
     * @param testAttendeeId: Id of an attendee
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
        this.reportLink = '    ' + this.domain + '/reports/test/' + testID + '/individual-report/' + testAttendeeId;
    }

    /**
     * Calculate testSummary details of a particular test
     */
    calculateTestSummaryDetails() {
        let totalTimeByAllAttendees = 0;
        let totalScoreOfAllAttendees = 0;
        let totalCorrectAttemptByAllAttendees = 0;
        let totalAttendee = this.testAttendeeArray.length;
        this.testAttendeeArray.forEach(x => {
            totalTimeByAllAttendees += x.report.timeTakenByAttendee;
            totalScoreOfAllAttendees += x.report.totalMarksScored;
            if (this.maxDuration < x.report.timeTakenByAttendee) {
                this.maxDuration = x.report.timeTakenByAttendee;
                this.maxDurationOfTest = Math.trunc(this.maxDuration / 60) + ' mins ' + Math.trunc(Math.floor(this.maxDuration % 60)) + ' secs ';
            }
            totalCorrectAttemptByAllAttendees = x.report.totalCorrectAttempts;
        });
        this.averageCorrectAttempt = totalCorrectAttemptByAllAttendees / totalAttendee;
        this.averageTestScore = totalScoreOfAllAttendees / totalAttendee;
        this.averageTimeTaken = totalTimeByAllAttendees / totalAttendee;
    }

    /**
     * Calculate all attendee ranks based on their totalmarks
     */
    calculateAttendeeRank() {
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
    * Selects the search text area on clicking of the search button
    * @param $event: is of type Event and is used to call stopPropagation()
    * @param search: is of type any
    */
    selectTextArea($event: any, search: any) {
        $event.stopPropagation();
        setTimeout(() => {
            search.select();
        }, 0);
    }

    /**
     * Generate report for unfinished test
     */
    generateReportForUnfinishedTest(testAttendee?: TestAttendee) {
        let confirmation = window.confirm('Do you want to generate report of this candidate ?');
        if (confirmation) {
            this.isGeneratingReport = true;

            let isSomeChecked: boolean;

            let attendeeIdList = new Array<number>();
            if (testAttendee) {
                testAttendee.generatingReport = true;
                attendeeIdList.push(testAttendee.id);
            } else {
                isSomeChecked = this.testAttendeeArray.some(x => x.checkedCandidate);
                this.testAttendeeArray.forEach(x => {
                    if (!isSomeChecked && x.report.totalMarksScored === null) {
                        x.generatingReport = true;
                        attendeeIdList.push(x.id);
                    } else if (x.checkedCandidate && x.report.totalMarksScored === null) {
                        x.generatingReport = true;
                        attendeeIdList.push(x.id);
                    }
                });
            }

            this.reportService.generateReport(attendeeIdList).subscribe(res => {

                this.testAttendeeArray.forEach((o, i, a) => {
                    if (res.some(x => x.id === a[i].id)) {
                        a[i] = res.find(x => x.id === a[i].id);
                    }
                    if (!isSomeChecked) {
                        a[i].generatingReport = false;
                    } else if (a[i].checkedCandidate) {
                        a[i].generatingReport = false;
                        a[i].checkedCandidate = true;
                    }
                    //set report for global attendee array
                    let attendeeIndex = this.attendeeArray.findIndex(y => y.id === a[i].id);
                    this.attendeeArray[attendeeIndex] = a[i];
                });

                this.isGeneratingReport = false;
                this.showGenerateReportButton = this.testAttendeeArray.some(x => x.report.totalMarksScored === null);
            });
        }
    }

    showDownloadButton() {
        let isAllReportGenerated = !this.testAttendeeArray.some(x => x.report.totalMarksScored === null);
        return (!this.showGenerateReportButton || isAllReportGenerated);
    }

    isReportGenerated(testAttendee: TestAttendee) {
        return testAttendee.report.testStatus !== TestStatus.allCandidates;
    }

    /**
     * Count Test attendees based on teststatus
     */
    testStatusWiseCountAttendees() {
        this.attendeeArray.forEach(attendee => {
            let teststatus = attendee.report.testStatus;
            switch (teststatus) {
                case 1:
                    this.completedTestCount += 1;
                    break;
                case 2:
                    this.expiredTestCount += 1;
                    break;
                case 3:
                    this.blockedTestCount += 1;
                    break;
                default:
                    this.unfinishedTestCount += 1;
            }
            if (attendee.starredCandidate)
                this.starredCandidateCount += 1;
        });
    }
}
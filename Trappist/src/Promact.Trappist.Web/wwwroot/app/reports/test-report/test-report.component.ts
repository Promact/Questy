import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TestAttendee } from '../testAttendee';
import { TestStatus } from '../enum-test-state';
import { Test } from '../../tests/tests.model';

declare var jsPDF: any;

@Component({
    moduleId: module.id,
    selector: 'test-report',
    templateUrl: 'test-report.html'
})

export class TestReportComponent implements OnInit {
    showSearchInput: boolean;
    searchString: string;
    testAttendeeArray: TestAttendee[];
    attendeeArray: TestAttendee[];
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
    checkedAllCandidate: boolean;

    constructor(private reportService: ReportService, private route: ActivatedRoute) {
        this.testAttendeeArray = new Array<TestAttendee>();
        this.attendeeArray = new Array<TestAttendee>();
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
        this.loader = true;
        this.checkedAllCandidate = false;
    }

    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestName();
        this.getAllTestCandidates();
    }

    /**
     * Fetches test name of particular test
     */
    getTestName() {
        this.reportService.getTestName(this.testId).subscribe((test) => {
            this.test = test;
        });
    }

    /**
     * Fetches all the candidates of any particular test
     */
    getAllTestCandidates() {
        this.loader = true;
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.attendeeArray = attendeeList;
            this.testAttendeeArray = this.attendeeArray;
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
    downloadTestReportPdf()
    {
        let testName = this.test.testName;
        let reports = new Array();
        let space = ' ';
        this.testAttendeeArray.forEach(x => {
                if (x.checkedCandidate) {
                    let report = {
                        'name': x.firstName + space + x.lastName,
                        'email': x.email,
                        'createdDateTime': x.createdDateTime,
                        'score': x.report.totalMarksScored,
                        'percentage': x.report.percentage
                    }
                    reports.push(report);
                }
            });
        let columns = [
                    { title: 'Name', dataKey: 'name' },
                    { title: 'Email', dataKey: 'email' },
                    { title: 'Test Date', dataKey: 'createdDateTime' },
                    { title: 'Score', dataKey: 'score' },
                    { title: 'Percentage', dataKey: 'percentage' },];
        var doc = new jsPDF('p', 'pt');
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
        doc.save('TestReport.pdf');
    }

    /**
     * Select and deselect all candidates of a test
     */
    selectAllChandidates() {
          if(this.checkedAllCandidate){ 
             this.testAttendeeArray.forEach(k => {
                k.checkedCandidate = true;
                });
            }
            else
            {
              this.testAttendeeArray.forEach(k => {
                   k.checkedCandidate = false;
              });

            }
        }

    /**
     * Select individual candidate of a test
     * @param testAttendee is object of the testAttendee table
     * @param select is the checkbox value of the checked candidate
     */
    checkIfAllSelected(testAttendee: TestAttendee, select: boolean) {
        testAttendee.checkedCandidate = select;
      
    }
        
}
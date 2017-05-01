import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TestAttendee } from '../testAttendee';
import { TestState } from '../enum-test-state';

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
    typeOfTest: string;
    count: number;
    testState: TestState;
    selectedTestState: TestState;
    headerStarStatus: string;
    rowStarStatus: string;
    activePage: number;
    starredCandidateIdList: number[];
    selectedCandidateIdList: number[];
    isCandidateStarred: boolean;
    isAllCandidateStarred: boolean;

    constructor(private reportService: ReportService, private route: ActivatedRoute) {
        this.testAttendeeArray = new Array<TestAttendee>();
        this.attendeeArray = new Array<TestAttendee>();
        this.typeOfTest = '0';
        this.selectedTestState = TestState.allCandidates;
        this.searchString = '';
        //this.starredCandidateArray = new Array<string>();
        this.headerStarStatus = 'star_border';
        this.rowStarStatus = 'star_border';
        this.activePage = 1;
        this.starredCandidateIdList = new Array<number>();
        this.selectedCandidateIdList = new Array<number>();
        this.isAllCandidateStarred = false;
        this.isCandidateStarred = false;
    }
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getAllTestAttendees();
    }

    getAllTestAttendees() {
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.attendeeArray = attendeeList;
            this.testAttendeeArray = this.attendeeArray;
            [this.headerStarStatus, this.isAllCandidateStarred, this.isCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false, false] : ['star', true, true];
            this.countAttendees();
        });
    }

    setAllCandidatesStarred(testId: number) {
        for (let i = 0; i < this.testAttendeeArray.length; i++) {
            this.starredCandidateIdList[i] = this.testAttendeeArray[i].id;
        }
        let status = this.headerStarStatus === 'star_border' ? true : false;
        this.reportService.setAllCandidatesStarred(this.testId, status, this.starredCandidateIdList).subscribe(
            response => {
                this.testAttendeeArray.forEach(k => k.starredCandidate = status);
            },
            err => {

            }
        );
        [this.headerStarStatus, this.isAllCandidateStarred, this.isCandidateStarred] = status ? ['star', true, true] : ['star_border', false, false];
    }

    setStarredCandidate(testAttendee: TestAttendee) {
        this.reportService.setStarredCandidate(testAttendee.id).subscribe(
            response => {
                this.testAttendeeArray.find(x => x.id === testAttendee.id).starredCandidate = !testAttendee.starredCandidate;
                [this.headerStarStatus, this.isAllCandidateStarred, this.isCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false, false] : ['star', true, true];
            },
            err => {
            }
        );
    }
    isStarredCandidate(attendee: TestAttendee) {
        [this.rowStarStatus, this.isCandidateStarred] = attendee.starredCandidate ? ['star', true] : ['star_border', false];
        return this.rowStarStatus;
    }

    getTestAttendeeMatchingSearchCriteria(searchString: string) {
        this.searchString = searchString;
        this.filterAttendeeList();
    }

    setTypeOfTest(typeOfTest: string) {
        this.selectedTestState = +typeOfTest;
        this.filterAttendeeList();
    }

    filter(selectedTestState: TestState, searchString: string) {
        let tempAttendeeArray: TestAttendee[] = [];
        searchString = searchString.toLowerCase();
        this.testAttendeeArray = [];

        if (selectedTestState === 0) {
            this.attendeeArray.forEach(x => {
                tempAttendeeArray.push(x);
            });
        }

        this.attendeeArray.forEach(x => {
            if (x.testState === selectedTestState) {
                tempAttendeeArray.push(x);
            }
        });

        tempAttendeeArray.forEach(x => {
            if (x.email.toLowerCase().includes(searchString)
                || x.firstName.toLowerCase().includes(searchString)
                || x.lastName.toLowerCase().includes(searchString)) {
                this.testAttendeeArray.push(x);
            }
        });
    }

    filterAttendeeList() {
        this.filter(this.selectedTestState, this.searchString);
        [this.headerStarStatus, this.isAllCandidateStarred, this.isCandidateStarred] = this.testAttendeeArray.some(x => !x.starredCandidate) ? ['star_border', false, false] : ['star', true, true];
        this.countAttendees();
    }

    countAttendees() {
        return this.count = this.testAttendeeArray.length;
    }

    pageHandling(activePage: number, count: number) {

        return count === 0 ? 0 : (activePage - 1) * 10 + 1;
    }

    pageHandling2(activePage: number, count: number) {
        let pagetracker = activePage * 10;
        return pagetracker > count ? count : pagetracker;
    }
}
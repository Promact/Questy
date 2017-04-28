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
    globalStarStatus: string;
    activePage: number;

    constructor(private reportService: ReportService, private route: ActivatedRoute) {
        this.testAttendeeArray = new Array<TestAttendee>();
        this.attendeeArray = new Array<TestAttendee>();
        this.typeOfTest = '0';
        this.selectedTestState = TestState.allCandidates;
        this.searchString = '';
        //this.starredCandidateArray = new Array<string>();
        this.globalStarStatus = 'star_border';
        this.activePage = 1;
    }
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getAllTestAttendees();
    }

    getAllTestAttendees() {
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.attendeeArray = attendeeList;
            this.testAttendeeArray = this.attendeeArray;
            this.globalStarStatus = this.testAttendeeArray.some(x => !x.starredCandidate) ? 'star_border' : 'star';
            this.countAttendees();
        });
    }

    setAllCandidatesStarred(testId: number) {
        let status = this.globalStarStatus === 'star_border' ? true : false;
        this.reportService.setAllCandidatesStarred(this.testId, status).subscribe(
            response => {
                this.attendeeArray.forEach(k => k.starredCandidate = status);
            },
            err => {

            }
        );
        this.globalStarStatus = status ? 'star' : 'star_border';
    }

    setStarredCandidate(testAttendee: TestAttendee) {
        this.reportService.setStarredCandidate(testAttendee.id).subscribe(
            response => {
                this.testAttendeeArray.find(x => x.id === testAttendee.id).starredCandidate = !testAttendee.starredCandidate;
                this.globalStarStatus = this.testAttendeeArray.some(x => !x.starredCandidate) ? 'star_border' : 'star';
            },
            err => {
            }
        );
    }
    isStarredCandidate(attendee: TestAttendee) {
        return attendee.starredCandidate ? 'star' : 'star_border';
    }

    getTestAttendeeMatchingSearchCriteria(searchString: string) {
        this.searchString = searchString;
        this.filterAttendeeList(this.selectedTestState, this.searchString);
    }

    setTypeOfTest(typeOfTest: string) {
        this.selectedTestState = +typeOfTest;
        this.filterAttendeeList(this.selectedTestState, this.searchString);
    }

    filterAttendeeList(selectedTestState: TestState, searchString: string) {
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

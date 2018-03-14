import { ComponentFixture } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { TestBed, async, fakeAsync } from '@angular/core/testing';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MaterialModule, MdSnackBar, MdSnackBarRef } from '@angular/material';
import { CoreModule } from '../../core/core.module';
import { FormsModule } from '@angular/forms';
import { TestReportComponent } from './test-report.component';
import { ReportService } from '../report.service';
import { Test } from '../../tests/tests.model';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { TestAttendee } from '../testAttendee';
import { Report } from '../report';
import { NgModule, DebugElement } from '@angular/core';
import { Md2DataTableModule } from 'md2';
import { ConductService } from '../../conduct/conduct.service';
import { Observable } from 'rxjs/Rx';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReportQuestionsCount } from './reportquestionscount';
import { ConnectionService } from '../../core/connection.service';
import { DatePipe } from '@angular/common';

class RouterStub {
    navigateByUrl(url: string) { return url; };
    navigate() { return true; };
    isActive() { return true; };
}

class ElementRef {
    nativeElement: any;
}

describe('Testing of test-report component:-', () => {
    let fixture: ComponentFixture<TestReportComponent>;
    let testReportComponent: TestReportComponent;
    let router: Router;
    let debug: DebugElement;

    let test = new Test();
    test.id = 1;
    test.testName = 'Report Testing';
    test.link = '1Pu48OQy6d';
    test.endDate = '0001-01-01T00:00:01.000';

    let attendee1 = new TestAttendee();
    attendee1.id = 1;
    attendee1.firstName = 'suparna';
    attendee1.lastName = 'Acharya';
    attendee1.email = 'suparna@promactinfo.com';
    attendee1.starredCandidate = false;
    attendee1.report.totalMarksScored = 20;
    attendee1.report.totalCorrectAttempts = 4;
    attendee1.report.isTestPausedUnWillingly = false;
    attendee1.report.testStatus = 3;
    attendee1.report.timeTakenByAttendee = 148;

    let attendee2 = new TestAttendee();
    attendee2.id = 2;
    attendee2.firstName = 'vijay kumar';
    attendee2.lastName = 'gupta';
    attendee2.email = 'vj@gmail.com';
    attendee2.starredCandidate = false;
    attendee2.report.totalMarksScored = 30;
    attendee2.report.totalCorrectAttempts = 6;
    attendee2.report.isTestPausedUnWillingly = false;
    attendee2.report.testStatus = 2;
    attendee2.report.timeTakenByAttendee = 125;

    let attendee3 = new TestAttendee();
    attendee3.firstName = 'ritu';
    attendee3.lastName = 'gupta';
    attendee3.email = 'rg@gmail.com';
    attendee3.report = null;
    attendee3.starredCandidate = true;

    let attendee4 = new TestAttendee();
    attendee4.id = 4;
    attendee4.firstName = 'megha';
    attendee4.lastName = 'shah';;
    attendee4.email = 'meghu@promactinfo.com';
    attendee4.starredCandidate = false;
    attendee4.report.totalMarksScored = 25;
    attendee4.report.totalCorrectAttempts = 5;
    attendee4.report.isTestPausedUnWillingly = false;
    attendee4.report.testStatus = 1;
    attendee4.report.timeTakenByAttendee = 150;

    let attendee5 = new TestAttendee();
    attendee5.id = 5;
    attendee5.firstName = 'ritu';
    attendee5.lastName = 'shah';;
    attendee5.email = 'ritu@promactinfo.com';
    attendee5.report.totalMarksScored = null;
    attendee5.reporNotFoundYet = true;
    attendee5.report.testStatus = 4;

    let attendees = new Array<TestAttendee>();
    attendees.push(attendee1);
    attendees.push(attendee2);
    attendees.push(attendee3);
    attendees.push(attendee4);
    attendees.push(attendee5);

    let excelDetails1 = new ReportQuestionsCount();
    excelDetails1.easyQuestionAttempted = 1;
    excelDetails1.noOfQuestionAttempted = 2;
    excelDetails1.testAttendeeId = 1;
    excelDetails1.totalTestQuestions = 5;

    let excelDetailsList = new Array<ReportQuestionsCount>();
    excelDetailsList.push(excelDetails1);

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                TestReportComponent
            ],
            providers: [
                ReportService,
                ConductService,
                ConnectionService,
                { provide: Router, useClass: RouterStub },
                { provide: ActivatedRoute, useclass: ActivatedRoute },
                DatePipe
            ],
            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule, Md2DataTableModule, BrowserAnimationsModule],
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TestReportComponent);
        testReportComponent = fixture.componentInstance;
        router = TestBed.get(Router);
        testReportComponent.testAttendeeArray.push(attendee1);
        testReportComponent.testAttendeeArray.push(attendee2);
        testReportComponent.testAttendeeArray.push(attendee4);
        testReportComponent.attendeeArray.push(attendee1);
        testReportComponent.attendeeArray.push(attendee2);
        testReportComponent.attendeeArray.push(attendee3);
        testReportComponent.attendeeArray.push(attendee4);
        testReportComponent.attendeeArray.push(attendee5);
    });

    it('should return the test name', () => {
        spyOn(ReportService.prototype, 'getTestName').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            return Observable.of(attendees);
        });
        testReportComponent.getTestName();
        expect(testReportComponent.test.testName).toBe('Report Testing');
    });

    it('should return all the attendees of a test', () => {
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            return Observable.of(attendees);;
        });
        testReportComponent.getAllTestCandidates();
        expect(testReportComponent.isAnyCandidateExist).toBeTruthy();
    });

    it('should return true if report of any candidate is not found', () => {
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            return Observable.of(attendees);
        });
        testReportComponent.getAllTestCandidates();
        expect(testReportComponent.attendeeArray.some(x => x.reporNotFoundYet)).toBeTruthy();
    });

    it('should set all candidate as starred candidates', () => {
        spyOn(ReportService.prototype, 'setAllCandidatesStarred').and.callFake(() => {
            return Observable.of(true);
        });
        testReportComponent.setAllCandidatesStarred();
        expect(testReportComponent.isAllCandidateStarred).toBeTruthy();
        expect(testReportComponent.headerStarStatus).toBe('star');
        expect(testReportComponent.starredCandidateCount).toBe(3);
    });

    it('should set all candidate as unstarred candidates', () => {
        testReportComponent.headerStarStatus = 'star';
        spyOn(ReportService.prototype, 'setAllCandidatesStarred').and.callFake(() => {
            return Observable.of(false);
        });
        testReportComponent.setAllCandidatesStarred();
        expect(testReportComponent.headerStarStatus).toBe('star_border');
        expect(testReportComponent.starredCandidateCount).toBe(0);
    });

    it('should set some candidates as starred candidates', () => {
        spyOn(ReportService.prototype, 'setStarredCandidate').and.callFake(() => {
            return Observable.of(attendee1.id).merge(attendee4.id);
        });
        testReportComponent.starredCandidateCount = 0;
        testReportComponent.testAttendeeArray.forEach(x => x.starredCandidate = false);
        testReportComponent.headerStarStatus = 'star-border';
        testReportComponent.setStarredCandidate(attendee1);
        testReportComponent.setStarredCandidate(attendee4);
        expect(testReportComponent.starredCandidateCount).toBe(2);
    });

    it('should set one candidates as unstarred candidates', () => {
        attendee1.starredCandidate = false;
        testReportComponent.testAttendeeArray.find(x => x.id === attendee1.id).starredCandidate = false;
        spyOn(ReportService.prototype, 'setStarredCandidate').and.callFake(() => {
            return Observable.of(attendee1);
        });
        testReportComponent.setStarredCandidate(attendee1);
        expect(testReportComponent.isAllCandidateStarred).toBeFalsy();
        expect(testReportComponent.headerStarStatus).toBe('star_border');
        expect(testReportComponent.starredCandidateCount).toBe(1);
    });

    it('should send a request to resume the test', () => {
        spyOn(ReportService.prototype, 'createSessionForAttendee').and.callFake(() => {
            return Observable.of(attendee2);
        });
        testReportComponent.resumeTest(attendee2);
        expect(testReportComponent.isAnyTestResume).toBeFalsy();
    });

    it('should return attendee matched with the search string', () => {
        testReportComponent.getTestAttendeeMatchingSearchCriteria('gupta');
        expect(testReportComponent.testAttendeeArray.length).toBe(2);
    });

    it('should return true if serachString length is not zero', () => {
        testReportComponent.searchString = 'Suparna';
        testReportComponent.showStatus();
        expect(testReportComponent.showSearchInput).toBeTruthy();
    });

    it('should return number of starred candidates if filter is of type starred candidate', () => {
        testReportComponent.attendeeArray.find(x => x.id === attendee1.id).starredCandidate = true;
        testReportComponent.attendeeArray.find(x => x.id === attendee2.id).starredCandidate = false;
        testReportComponent.attendeeArray.find(x => x.id === attendee3.id).starredCandidate = false;
        testReportComponent.attendeeArray.find(x => x.id === attendee4.id).starredCandidate = true;
        testReportComponent.setTestStatusType('star');
        expect(testReportComponent.selectedTestStatus).toBe(0);
        expect(testReportComponent.starredCandidateCount).toBe(2);
    });

    it('should return number of candidates if filter types are test completeion status', () => {
        testReportComponent.setTestStatusType('1');
        expect(testReportComponent.selectedTestStatus).toBe(1);
        expect(testReportComponent.noCandidateFound).toBeFalsy();
    });

    it('should return the filtered list of candidates when test-status is all candidate', () => {
        testReportComponent.filter(0, '', false);
        expect(testReportComponent.noCandidateFound).toBeFalsy();
    });

    it('should return the filtered list of candidates if filter criteria is showStarCandidate', () => {
        spyOn(ReportService.prototype, 'setAllCandidatesStarred').and.callFake(() => {
            return Observable.of(true);
        });
        testReportComponent.setAllCandidatesStarred();
        testReportComponent.filter(0, '', true);
        expect(testReportComponent.starredCandidateCount).toBe(3);
        expect(testReportComponent.noCandidateFound).toBeFalsy();
    });

    it('should return the filtered list of candidates if filter criteria as per searching', () => {
        testReportComponent.filter(0, 'promact', false);
        expect(testReportComponent.testAttendeeArray.length).toBe(3);
    });

    it('should return the filtered list of candidates if filter criteria as per searching', () => {
        testReportComponent.filter(4, 'promact', false);
        expect(testReportComponent.testAttendeeArray.length).toBe(1);
    });

    it('should return the total number of attendees', () => {
        testReportComponent.countAttendees();
        expect(testReportComponent.count).toBe(3);
    });

    it('should return all excel deatils and call the download excel function', () => {
        spyOn(testReportComponent, 'downloadTestReportExcel');
        spyOn(ReportService.prototype, 'getAllAttendeeMarksDetails').and.callFake(() => {
            return Observable.of(excelDetailsList);
        });
        testReportComponent.getExcelDetails();
        expect(testReportComponent.reportQuestionDetails.length).toBe(1);
    });

    it('should return the test time in local time format', () => {
        let testCreateTime = '2017-10-12T06:02:00.4463941Z';
        let testDateTime = new Date(testCreateTime);
        let offset = testDateTime.getTimezoneOffset();
        let hoursDiff = Math.trunc(offset / 60);
        let minutesDiff = Math.trunc(offset % 60);
        let localHours = testDateTime.getHours() - hoursDiff;
        let localMinutes = testDateTime.getMinutes() - minutesDiff;
        if (localMinutes >= 60) {
            let hours = Math.trunc(localMinutes / 60);
            let minutes = Math.trunc(localMinutes % 60);
            localHours += hours;
            localMinutes = minutes;
        }
        let time = localHours + ' : ' + localMinutes;
        testReportComponent.calculateLocalTime(testDateTime);
        expect(testReportComponent.testTime).toBe(time);
    });

    it('should select all candiate at a time', () => {
        testReportComponent.checkedAllCandidate = true;
        testReportComponent.selectAllCandidates();
        expect(testReportComponent.testAttendeeArray.every(x => x.checkedCandidate === true)).toBeTruthy();
    });

    it('should deselect all candiate at a time', () => {
        testReportComponent.checkedAllCandidate = false;
        testReportComponent.isAnyCandidateSelected = true;
        testReportComponent.selectAllCandidates();
        expect(testReportComponent.testAttendeeArray.every(x => x.checkedCandidate === false)).toBeTruthy();
    });

    it('should select single candiate at a time', () => {
        testReportComponent.selectIndividualCandidate(attendee1, true);
        expect(testReportComponent.testAttendeeArray.find(x => x.id === attendee1.id).checkedCandidate).toBeTruthy();
    });

    it('should deselect single candiate at a time', () => {
        testReportComponent.selectIndividualCandidate(attendee1, false);
        expect(testReportComponent.testAttendeeArray.find(x => x.id === attendee1.id).checkedCandidate).toBeFalsy();
    });

    it('should return testfinish status and testreportlink for excel report', () => {
        testReportComponent.domain = 'http://localhost:50805';
        testReportComponent.testTakerDetails(attendee2.report.testStatus, 1001, attendee2.id);
        expect(testReportComponent.testFinishStatus).toBe('Expired');
        expect(testReportComponent.reportLink).toBe('    http://localhost:50805/reports/test/1001/individual-report/2');
        testReportComponent.testTakerDetails(attendee4.report.testStatus, 1001, attendee4.id);
        expect(testReportComponent.testFinishStatus).toBe('Completed');
        testReportComponent.testTakerDetails(attendee1.report.testStatus, 1001, attendee1.id);
        expect(testReportComponent.testFinishStatus).toBe('Blocked');
    });

    it('should return the maximum duration of a test for excel report ', () => {
        testReportComponent.calculateTestSummaryDetails();
        expect(testReportComponent.maxDuration).toBe(150);
    });

    it('should return average time taken in a test for excel report ', () => {
        testReportComponent.calculateTestSummaryDetails();
        expect(testReportComponent.averageTimeTaken).toBe(2);
    });

    it('should return highest ranked candiate of a test ', () => {
        testReportComponent.sortedAttendeeArray.push(attendee2);
        testReportComponent.sortedAttendeeArray.push(attendee4);
        testReportComponent.sortedAttendeeArray.push(attendee1);
        testReportComponent.calculateAttendeeRank();
        expect(testReportComponent.testAttendeeRank[0].attendeeId).toBe(2);
    });

    it('should calculate test candidates as per test status ', () => {
        testReportComponent.testStatusWiseCountAttendees();
        expect(testReportComponent.blockedTestCount).toBe(1);
        expect(testReportComponent.expiredTestCount).toBe(1);
        expect(testReportComponent.completedTestCount).toBe(1);
    });

    it('should select the text area', () => {
        let event: any = {};
        let search: any = {};
        event.stopPropagation = function () { };
        search.select = function () { };
        spyOn(event, 'stopPropagation');
        testReportComponent.selectTextArea(event, search);
        expect(event.stopPropagation).toHaveBeenCalled();
    });   
});
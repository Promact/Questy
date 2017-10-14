import { ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { TestBed, async, fakeAsync } from "@angular/core/testing";
import { Router, ActivatedRoute, RouterModule } from "@angular/router";
import { MaterialModule } from "@angular/material";
import { CoreModule } from "../../core/core.module";
import { FormsModule } from "@angular/forms";
import { TestReportComponent } from "./test-report.component";
import { ReportService } from "../report.service";
import { Test } from "../../tests/tests.model";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { TestAttendee } from "../testAttendee";
import { Report } from "../report";
import { NgModule } from "@angular/core";
import { Md2DataTableModule } from 'md2';
import { ConductService } from "../../conduct/conduct.service";

class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate() { return true; }
    isActive() { return true; }
}

describe('testting of test report', () => {
    let fixture: ComponentFixture<TestReportComponent>;
    let testReportComponent: TestReportComponent;
    let router: Router;

    let test = new Test()
    {
        test.id = 1,
            test.testName = 'Report Testing',
            test.link = '1Pu48OQy6d'
    };

    let attendee1 = new TestAttendee()
    {
        attendee1.id = 1,
            attendee1.firstName = 'suparna',
            attendee1.lastName = 'Acharya',
            attendee1.email = 'suparna@promactinfo.com',
            attendee1.starredCandidate = false;
        attendee1.report = new Report()
        {
            attendee1.report.testStatus = 1,
                attendee1.report.totalMarksScored = 20,
                attendee1.report.totalCorrectAttempts = 4
        }
    }
    let attendee2 = new TestAttendee()
    {
        attendee2.id = 2,
            attendee2.firstName = 'vijay kumar',
            attendee2.lastName = 'gupta',
            attendee2.email = 'vj@gmail.com',
            attendee2.starredCandidate = false,
            attendee2.report = new Report()
        {
            attendee2.report.testStatus = 1,
                attendee2.report.totalMarksScored = 30,
                attendee2.report.totalCorrectAttempts = 6
        }
    }
    let attendee3 = new TestAttendee()
    {
        attendee3.firstName = 'ritu',
            attendee3.lastName = 'gupta',
            attendee3.email = 'rg@gmail.com',
            attendee3.report = null
    }
    let attendees = new Array<TestAttendee>();
    attendees.push(attendee1);
    attendees.push(attendee2);
    attendees.push(attendee3);

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                TestReportComponent,
            ],
            providers: [
                ReportService,
                ConductService,
                { provide: Router, useClass: RouterStub },
                { provide: ActivatedRoute, useclass: ActivatedRoute },
            ],
            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule, Md2DataTableModule.forRoot()],
        }).compileComponents();
    }));
    beforeEach(() => {
        fixture = TestBed.createComponent(TestReportComponent);
        testReportComponent = fixture.componentInstance;
        router = TestBed.get(Router);
        testReportComponent.testAttendeeArray.push(attendee1);
        testReportComponent.testAttendeeArray.push(attendee2);

    });

    it('should return the test name', () => {
        spyOn(ReportService.prototype, 'getTestName').and.callFake(() => {
            let testObject = new BehaviorSubject(test);
            return testObject.asObservable();
        });
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            let attendeesObject = new BehaviorSubject(attendees);
            return attendeesObject.asObservable();
        })
        testReportComponent.getTestName();
        expect(testReportComponent.test.testName).toBe('Report Testing');
    })

    it('should return all the attendees of a test', () => {
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            let attendeesObject = new BehaviorSubject(attendees);
            return attendeesObject.asObservable();
        });
        testReportComponent.getAllTestCandidates();
        expect(testReportComponent.isAnyCandidateExist).toBeTruthy();
    })

    it('should return true if report of any candidate is not found', () => {
        spyOn(ReportService.prototype, 'getAllTestAttendees').and.callFake(() => {
            let attendeesObject = new BehaviorSubject(attendees);
            return attendeesObject.asObservable();
        });
        testReportComponent.getAllTestCandidates();
        expect(testReportComponent.attendeeArray.some(x => x.reporNotFoundYet)).toBeTruthy();
    })

    it('should set all candidate as starred candidates', () => {

        spyOn(ReportService.prototype, 'setAllCandidatesStarred').and.callFake(() => {
            let starredCandidate = new BehaviorSubject(true);
            return starredCandidate.asObservable();
        });
        testReportComponent.setAllCandidatesStarred();
        expect(testReportComponent.isAllCandidateStarred).toBeTruthy();
        expect(testReportComponent.starredCandidateCount).toBe(2);
    })

    it('should set some candidates as starred candidates', () => {
        spyOn(ReportService.prototype, 'setStarredCandidate').and.callFake(() => {
            let onestarredCandidate = new BehaviorSubject(attendee1.id);
            return onestarredCandidate.asObservable();
        });
        testReportComponent.starredCandidateCount = 0;
        testReportComponent.setStarredCandidate(attendee1);
        expect(testReportComponent.isAllCandidateStarred).toBeFalsy();
        expect(testReportComponent.starredCandidateCount).toBe(1);
    })
})
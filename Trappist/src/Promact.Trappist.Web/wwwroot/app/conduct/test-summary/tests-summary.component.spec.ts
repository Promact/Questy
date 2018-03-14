import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { TestSummaryComponent } from './test-summary.component';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import { MockTestData, MockTestAttendee } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { ConductService } from '../conduct.service';
import { APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { ReportService } from '../../reports/report.service';
import { TestConductFooterComponent } from '../shared/test-conduct-footer/test-conduct-footer.component';
import { TestConductHeaderComponent } from '../shared/test-conduct-header/test-conduct-header.component';
import { ConnectionService } from '../../core/connection.service';


class MockActivatedRoute {
    params = Observable.of({});
}

describe('Test Summary', () => {
    let fixture: ComponentFixture<TestSummaryComponent>;
    let testSummary: TestSummaryComponent;
    let mockTestAttendee: any;
    let routTo: any[];

    beforeEach(async(() => {

        TestBed.configureTestingModule({
            declarations: [TestSummaryComponent, TestConductFooterComponent, TestConductHeaderComponent],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule],
            providers: [ConnectionService , ConductService, HttpService, ReportService, ConductService,
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
            ]
        }).compileComponents();

    }));

    beforeEach(() => {
        spyOn(Router.prototype, 'navigate').and.callFake((route: any[]) => {
            routTo = route;
        });
        mockTestAttendee = JSON.parse(JSON.stringify(MockTestAttendee));
        fixture = TestBed.createComponent(TestSummaryComponent);
        testSummary = fixture.componentInstance;
        //spyOn(ConductService.prototype, 'getTestSummary').and.returnValue(Observable.of());
        spyOn(ConductService.prototype, 'getTestAttendeeByTestId').and.returnValue(Observable.of(MockTestAttendee));

    });

    it('sendRequestForResume', () => {
        spyOn(ReportService.prototype, 'updateCandidateInfo').and.returnValue(Observable.of(true));
        spyOn(MdSnackBar.prototype, 'open').and.callThrough();
        testSummary.testAttendee = mockTestAttendee;
        testSummary.sendRequestForResume();
        expect(MdSnackBar.prototype.open).toHaveBeenCalledTimes(1);
        expect(testSummary.testAttendee.report.isTestPausedUnWillingly).toBe(true);
    });

    it('getCandidateInfoToResumeTest', () => {
        mockTestAttendee.report.isAllowResume = true;
        spyOn(ReportService.prototype, 'getInfoResumeTest').and.returnValue(Observable.of(mockTestAttendee.report));
        testSummary.getCandidateInfoToResumeTest();
        expect(testSummary.isAllowed).toBe(true);
    });

    it('endYourTest', () => {
        spyOn(ReportService.prototype, 'createSessionForAttendee').and.returnValue(Observable.of(true));
        testSummary.endYourTest();
        expect(routTo[0]).toBe('test-end');
    });
});
import { TestsDashboardComponent } from './tests-dashboard.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router } from '@angular/router';
import { FilterPipe } from './test-dashboard.pipe';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule } from '@angular/http';
import { TestService } from '../tests.service';
import { inject } from '@angular/core/testing';
import { Test } from '../tests.model';
import { testsRouting } from '../tests.routing';
import { TestServicesMock, MockQuestionService } from '../../Mock_Services/test-services.mock';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { HttpService } from '../../core/http.service';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { tick } from '@angular/core/testing';
import { Location, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { fakeAsync } from '@angular/core/testing';
import { DeleteTestDialogComponent } from './delete-test-dialog.component';
import { DuplicateTestDialogComponent } from './duplicate-test-dialog.component';




class MockRouter {
    navigate() {
        return true;
    }

    isActive() {
        return true;
    }

    navigateByUrl(url: string) { return url; }
}

class MockDialog {
    open() {
        return true;
    }

    close() {
        return true;
    }
}

describe('Test Dashboard Component', () => {

    let testDashboard: TestsDashboardComponent;
    let fixture: ComponentFixture<TestsDashboardComponent>;

    let test = new Test();
    test.id = 1;
    test.numberOfTestAttendees = 2;
    test.testName = 'History';
    test.isEditTestEnabled = true;
    let router: Router;
    let urls: any[];

    beforeEach(async(() => {

        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [TestCreateDialogComponent, DeleteTestDialogComponent, DuplicateTestDialogComponent]
            }
        });


        TestBed.configureTestingModule({

            declarations: [TestsDashboardComponent, FilterPipe, TestCreateDialogComponent, DeleteTestDialogComponent, DuplicateTestDialogComponent],


            providers: [
                { provide: QuestionsService, useClass: MockQuestionService },
                TestService,
                HttpService,
                { provide: Router, useClass: MockRouter },
                { provide: MdDialogRef, useClass: MockDialog }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, HttpModule, BrowserAnimationsModule, MdDialogModule]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TestsDashboardComponent);
        testDashboard = fixture.componentInstance;
    });

    it('getTest', fakeAsync(() => {
        spyOn(TestService.prototype, 'getTests').and.callFake(() => {
            return Observable.of(MockTestData);
        });
        tick();
        testDashboard.getAllTests();
        console.log(testDashboard.tests);
        expect(testDashboard.tests.length).toBe(2);
    }));

    it('should edit test on number of attendees for a particular test is 0', fakeAsync(() => {
        spyOn(TestService.prototype, 'isTestAttendeeExist').and.callFake(() => {
            return Observable.of(false);
        });
        router = TestBed.get(Router);
        spyOn(router, 'navigate').and.callFake(function (url: any[]) {
            urls = url;
        });
        testDashboard.editTest(test);
        expect(urls[0]).toBe('/tests/' + test.id + '/sections');
    }));

    it('should not edit the test when attendees exist for a particular test', () => {
        spyOn(TestService.prototype, 'isTestAttendeeExist').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(testDashboard, 'editTest').and.callThrough();
        testDashboard.editTest(test);
        expect(testDashboard.editTest).toHaveBeenCalled();
    });

    it('should check whether any attendee has taken a particular test', () => {
        testDashboard.tests[0] = test;
        testDashboard.disableEditForTheTestsIfAttendeesExist();
        expect(testDashboard.tests[0].isEditTestEnabled).toBe(false);
    });

    it('should open delete test dialog on call of deleteTestDialog with a message informing test cannot be deleted', () => {
        spyOn(TestService.prototype, 'isTestAttendeeExist').and.callFake(() => {
            let result = new BehaviorSubject(true);
            return result.asObservable();
        });
        spyOn(testDashboard.dialog, 'open').and.callThrough();
        testDashboard.deleteTestDialog(test);
        expect(testDashboard.isDeleteAllowed).toBe(false);
        expect(testDashboard.deleteTestDialogData.testToDelete).toBe(test);
        expect(testDashboard.deleteTestDialogData.testArray).toBe(testDashboard.tests);
        expect(testDashboard.dialog.open).toHaveBeenCalled();
    });

    it('should open delete test dialog on call of deleteTestDialog with a message confirming if the user wants to delete the test', () => {
        spyOn(TestService.prototype, 'isTestAttendeeExist').and.callFake(() => {
            let result = new BehaviorSubject(false);
            return result.asObservable();
        });
        spyOn(testDashboard.dialog, 'open').and.callThrough();
        testDashboard.deleteTestDialog(test);
        expect(testDashboard.isDeleteAllowed).toBe(true);
        expect(testDashboard.deleteTestDialogData.testToDelete).toBe(test);
        expect(testDashboard.deleteTestDialogData.testArray).toBe(testDashboard.tests);
        expect(testDashboard.dialog.open).toHaveBeenCalled();
    });

    it('should open duplicate test dialog on call of duplicateTestDialog and display test name as testName_copy_5 [number of times the test has been copied]', () => {
        spyOn(TestService.prototype, 'setTestCopiedNumber').and.callFake(() => {
            let result = new BehaviorSubject(5);
            return result.asObservable();
        });
        spyOn(testDashboard.dialog, 'open').and.callThrough();
        testDashboard.duplicateTestDialog(test);
        expect(testDashboard.count).toBe(5);
        expect(testDashboard.duplicateTestDialogData.testName).toBe(test.testName + '_copy_' + testDashboard.count);
        expect(testDashboard.duplicateTestDialogData.testArray).toBe(testDashboard.tests);
        expect(testDashboard.duplicateTestDialogData.testToDuplicate).toBe(test);
        expect(testDashboard.dialog.open).toHaveBeenCalled();
    });

    it('should open duplicate test dialog on call of duplicateTestDialog and display test name as testName_copy', () => {
        spyOn(TestService.prototype, 'setTestCopiedNumber').and.callFake(() => {
            let result = new BehaviorSubject(1);
            return result.asObservable();
        });
        spyOn(testDashboard.dialog, 'open').and.callThrough();
        testDashboard.duplicateTestDialog(test);
        expect(testDashboard.count).toBe(1);
        expect(testDashboard.duplicateTestDialogData.testName).toBe(test.testName + '_copy');
        expect(testDashboard.duplicateTestDialogData.testArray).toBe(testDashboard.tests);
        expect(testDashboard.duplicateTestDialogData.testToDuplicate).toBe(test);
        expect(testDashboard.dialog.open).toHaveBeenCalled();
    });

});


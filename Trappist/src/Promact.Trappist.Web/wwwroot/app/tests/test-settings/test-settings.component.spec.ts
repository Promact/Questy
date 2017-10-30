import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule, FormGroup } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog, MdSnackBar, MdSnackBarRef } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule, ResponseOptions } from '@angular/http';
import { TestService } from '../tests.service';
import { inject } from '@angular/core/testing';
import { Test } from '../tests.model';
import { testsRouting } from '../tests.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpService } from '../../core/http.service';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/from';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { tick } from '@angular/core/testing';
import { Location, LocationStrategy, APP_BASE_HREF } from '@angular/common';
import { NgModule, Input, Output, EventEmitter } from '@angular/core';
import { fakeAsync } from '@angular/core/testing';
import { PopoverModule } from 'ngx-popover';
import { ClipboardModule } from 'ngx-clipboard';
import { Md2AccordionModule } from 'md2';
import { Category } from '../../questions/category.model';
import { QuestionBase } from '../../questions/question';
import { QuestionType } from '../../questions/enum-questiontype';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { TestSettingsComponent } from './test-settings.component';
import { CreateTestHeaderComponent } from '../shared/create-test-header/create-test-header.component';
import { CreateTestFooterComponent } from '../shared/create-test-footer/create-test-footer.component';
import { IncompleteTestCreationDialogComponent } from './incomplete-test-creation-dialog.component';
import { TestIPAddress } from '../test-IPAdddress';


class MockRouter {
    navigate() {
        return true;
    }

    isActive() {
        return true;
    }

    navigateByUrl(url: string) { return url; }
}

class MockError {
    json(): Observable<any> {
        return Observable.of({ 'error': ['Settings cannot be updated'] });
    }
}

describe('Test Settings Component', () => {

    let testSettings: TestSettingsComponent;
    let fixture: ComponentFixture<TestSettingsComponent>;

    let question = new QuestionBase();
    question.question.id = 3;
    question.question.categoryID = 3;
    question.question.questionDetail = 'Who was the father of Akbar?';
    question.question.questionType = QuestionType.singleAnswer;
    question.question.difficultyLevel = DifficultyLevel.Easy;
    question.singleMultipleAnswerQuestion = null;
    question.codeSnippetQuestion = null;
    question.question.isSelect = true;

    let category = new Category();
    category.id = 3;
    category.categoryName = 'history';
    category.isSelect = true;
    category.numberOfSelectedQuestion = 0;
    category.questionList[0] = question;
    category.isAccordionOpen = false;
    category.isAlreadyClicked = false;

    let testIpAddress = new TestIPAddress();
    testIpAddress.id = 1;
    testIpAddress.ipAddress = '101.20.57.45';
    testIpAddress.testId = 3;

    let test = new Test();
    test.id = 3;
    test.numberOfTestAttendees = 2;
    test.testName = 'History';
    test.link = 'a6thsjk8';
    test.duration = 10;
    test.warningTime = 5;
    test.startDate = '2017-10-16T06:51:49.4283026Z';
    test.endDate = '2017-10-17T06:51:49.4283026Z';
    test.testIpAddress[0] = testIpAddress;

    let urls: any[];
    let route: ActivatedRoute;
    let router: Router;

    class MockDialog {
        open() {
            return true;
        }

        close() {
            return true;
        }
    }

    beforeEach(async(() => {

        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [IncompleteTestCreationDialogComponent]
            }
        });

        TestBed.configureTestingModule({

            declarations: [TestSettingsComponent, CreateTestHeaderComponent, CreateTestFooterComponent, IncompleteTestCreationDialogComponent],

            providers: [
                TestService,
                HttpService, 
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: APP_BASE_HREF, useValue: '/'},
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule.forRoot([]), HttpModule, BrowserAnimationsModule, PopoverModule, ClipboardModule, Md2AccordionModule.forRoot(), MdDialogModule]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TestSettingsComponent);
        testSettings = fixture.componentInstance;
    });

    it('should get test details by id', () => {
        spyOn(TestService.prototype, 'getTestById').and.callFake(() => {
            return Observable.of(test);
        });
        testSettings.getTestById(test.id);
        expect(testSettings.testNameReference).toBe(test.testName);
        expect(testSettings.testLink).toContain(test.link);
    });

    it('should check whether the warning time is valid', () => {
        spyOn(TestService.prototype, 'getTestById').and.callFake(() => {
            return Observable.of(test);
        });
        testSettings.isWarningTimeValid();
        expect(testSettings.validTime).toBe(false);
        testSettings.testDetails.warningTime = 25;
        testSettings.testDetails.duration = 5;
        testSettings.isWarningTimeValid();
        expect(testSettings.validTime).toBe(true);
    });

    it('should check end date is valid', () => {
        testSettings.testDetails.startDate = test.startDate;
        testSettings.isEndDateValid(test.endDate);
        expect(testSettings.validEndDate).toBe(false);
    });

    it('should check end date is invalid', () => {
        testSettings.testDetails.startDate = '2017-10-21T06:51:49.4283026Z';
        testSettings.isEndDateValid(test.endDate);
        expect(testSettings.validEndDate).toBe(true);
        expect(testSettings.validStartDate).toBe(false);
    });

    it('should check start date is valid', () => {
        testSettings.testDetails.startDate = test.startDate;
        testSettings.isStartDateValid();
        expect(testSettings.validStartDate).toBe(true);
        expect(testSettings.validEndDate).toBe(false);
        testSettings.testDetails.startDate = '2017-12-16T06:51:49.4283026Z';
        testSettings.testDetails.endDate = test.endDate;
        testSettings.isStartDateValid();
        expect(testSettings.validStartDate).toBe(false);
        expect(testSettings.validEndDate).toBe(true);
    });

    it('should check ip address is added', () => {
        testSettings.IpAddressAdded(testIpAddress.ipAddress);
        expect(testSettings.isIpAddressAdded).toBe(true);
    });

    it('should check ip address is not added', () => {
        testIpAddress.ipAddress = '';
        testSettings.IpAddressAdded(testIpAddress.ipAddress);
        expect(testSettings.isIpAddressAdded).toBe(false);
    });

    it('should show error message', () => {
        testIpAddress.ipAddress = '';
        testSettings.showErrorMessage(testIpAddress);
        expect(testIpAddress.isErrorMessageVisible).toBe(true);
        testIpAddress.ipAddress = undefined;
        testSettings.showErrorMessage(testIpAddress);
        expect(testIpAddress.isErrorMessageVisible).toBe(true);
    });

    it('should update the test settings', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(testSettings.snackbarRef, 'open').and.callThrough();
        testSettings.saveTestSettings(test.id, test);
        router = TestBed.get(Router);
        spyOn(router, 'navigate').and.callFake(function (url: any[]) {
            urls = url;
            expect(urls[0]).toBe('/tests');
        });
        expect(testSettings.snackbarRef.open).toHaveBeenCalled();
    });

    it('should open the incomplete test creation dialog box asking the user to select sections for the test', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(testSettings.dialog, 'open').and.callThrough();
        testSettings.testDetails = test;
        testSettings.launchTestDialog(test.id, test, test.isLaunched);
        expect(testSettings.testDetails.isQuestionMissing).toBe(false);
        expect(testSettings.dialog.open).toHaveBeenCalled();
    });

    it('should open the incomplete test creation dialog box asking the user to select questions for the test', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(testSettings.dialog, 'open').and.callThrough();
        test.categoryAcList[0] = category;
        testSettings.testDetails = test;
        testSettings.launchTestDialog(test.id, test, test.isLaunched);
        expect(testSettings.testDetails.isQuestionMissing).toBe(true);
        expect(testSettings.dialog.open).toHaveBeenCalled();
    });

    it('should launch the test', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(TestService.prototype, 'getTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(testSettings.snackbarRef, 'open').and.callThrough();
        category.numberOfSelectedQuestion = 1;
        test.categoryAcList[0] = category;
        testSettings.testDetails = test;
        let isTestLaunched: boolean;
        testSettings.launchTestDialog(test.id, test, isTestLaunched);
        testSettings.getTestById(test.id);
        expect(testSettings.testDetails.isLaunched).toBe(true);
        expect(testSettings.snackbarRef.open).toHaveBeenCalled();
    });

    it('should not get test details by id', () => {
        spyOn(TestService.prototype, 'getTestById').and.callFake(() => {
            return Observable.throw(Error);
        });
        router = TestBed.get(Router);
        spyOn(router, 'navigate').and.callFake(function (url: any) {
            urls = url;
        });
        spyOn(testSettings.snackbarRef, 'open').and.callThrough();
        testSettings.getTestById(test.id);
        expect(testSettings.loader).toBe(false);
        expect(urls[0]).toBe('/tests');
        expect(testSettings.snackbarRef.open).toHaveBeenCalled();
    });

    it('should not update the changes made in the settings of a test', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.throw(new MockError());
        });
        spyOn(testSettings.snackbarRef, 'open').and.callThrough();
        testSettings.saveTestSettings(test.id, test);
        expect(testSettings.loader).toBe(false);
        expect(testSettings.snackbarRef.open).toHaveBeenCalled();
    });

    it('should call showTooltipMessage()', () => {
        let event: any = {};
        event.stopPropagation = function() { };
        let element: any = {};
        element.select = function() { };
        testSettings.showTooltipMessage(event, element);
        expect(testSettings.tooltipMessage).toBe('Copied');
    });

    it('should change the tooltip message', () => {
        testSettings.changeTooltipMessage();
        expect(testSettings.tooltipMessage).toBe('Copy to Clipboard');
    });

    it('should not launch the test', () => {
        spyOn(TestService.prototype, 'updateTestById').and.callFake(() => {
            return Observable.throw(new MockError());
        });
        spyOn(TestService.prototype, 'getTestById').and.callFake(() => {
            return Observable.of(test);
        });
        spyOn(testSettings.snackbarRef, 'open').and.callThrough();
        category.numberOfSelectedQuestion = 1;
        test.categoryAcList[0] = category;
        testSettings.testDetails = test;
        let isTestLaunched: boolean;
        testSettings.launchTestDialog(test.id, test, isTestLaunched);
        testSettings.getTestById(test.id);
        expect(testSettings.snackbarRef.open).toHaveBeenCalled();
    });
});
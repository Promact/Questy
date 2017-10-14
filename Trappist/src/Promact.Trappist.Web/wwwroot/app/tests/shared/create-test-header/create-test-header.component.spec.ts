import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule, FormGroup } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog, MdSnackBar, MdSnackBarRef } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { Http, HttpModule } from '@angular/http';
import { TestService } from '../../tests.service';
import { inject } from '@angular/core/testing';
import { Test } from '../../tests.model';
import { testsRouting } from '../../tests.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
import { CreateTestHeaderComponent } from './create-test-header.component';
import { TestSettingsComponent } from '../../test-settings/test-settings.component';
import { HttpService } from '../../../core/http.service';
import { CreateTestFooterComponent } from '../create-test-footer/create-test-footer.component';
import { IncompleteTestCreationDialogComponent } from '../../test-settings/incomplete-test-creation-dialog.component';
import * as screenfull from 'screenfull';
import { MockTestData } from '../../../Mock_Data/test_data.mock';



class MockRouter {
    navigate() {
        return true;
    }

    isActive() {
        return true;
    }

    navigateByUrl(url: string) { return url; }
}

class MockActivatedRoute {
    constructor() {
        this._paramsValue = { id: 1, name: "matchString" };
        this._queryParamsValue = { id: 1 };
    }
    // ActivatedRoute.params is Observable
    private subject = new BehaviorSubject(this.testParams);
    params = this.subject.asObservable();

    // Test parameters
    private _testParams: {};
    private _paramsValue: { id: number, name: string };
    private _queryParamsValue: { id: number };

    get testParams() { return this._testParams; }
    set testParams(params: {}) {
        this._testParams = params;
        this.subject.next(params);
    }
    // ActivatedRoute.snapshot.params
    get snapshot() {
        return { params: this._paramsValue, queryParams: this._queryParamsValue };
    }
    set snapshot(param: { params: { id: number, name: string }, queryParams: { id: number } }) {
        this._paramsValue = param.params;
        this._queryParamsValue = param.queryParams;
    }
}

describe('Create Test Header Component', () => {

    let createTestHeaderComponent: CreateTestHeaderComponent;
    let fixture: ComponentFixture<CreateTestHeaderComponent>;
    

    let test = new Test();
    test.id = 3;
    test.numberOfTestAttendees = 2;
    test.testName = 'History';
    test.link = 'a6thsjk8';
    test.duration = 10;
    test.warningTime = 5;
    test.startDate = '2017-10-16T06:51:49.4283026Z';
    test.endDate = '2017-10-17T06:51:49.4283026Z';

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
                entryComponents: [TestSettingsComponent, CreateTestHeaderComponent ]
            }
        });

        TestBed.configureTestingModule({

            declarations: [TestSettingsComponent, CreateTestHeaderComponent, CreateTestFooterComponent, IncompleteTestCreationDialogComponent ],

            providers: [
                TestService,
                HttpService,
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: MockActivatedRoute }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule.forRoot([]), HttpModule, BrowserAnimationsModule, PopoverModule, ClipboardModule, Md2AccordionModule.forRoot(), MdDialogModule ]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CreateTestHeaderComponent);
        createTestHeaderComponent = fixture.componentInstance;
    });

    it('should get the test link', () => {
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.getTestLink();
        expect(createTestHeaderComponent.testLink).toContain(test.link);
    });

    it('should hide the edit button and make the check and close buttons visible', () => {
        createTestHeaderComponent.hideEditButton();
        expect(createTestHeaderComponent.isLabelVisible).toBe(false);
    });

    it('should make the edit button visible along with the valid test name being displayed in the label for test name', () => {
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.testNameReference = test.testName;
        createTestHeaderComponent.showEditButton(test.testName);
        expect(createTestHeaderComponent.isLabelVisible).toBe(true);
        expect(createTestHeaderComponent.nameOfTest).toBe(test.testName);
        expect(createTestHeaderComponent.testDetails.testName).toBe(test.testName);
    });

    it('should display valid test name when test name is entered null', () => {
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.testNameReference = test.testName;
        createTestHeaderComponent.showEditButton('');
        expect(createTestHeaderComponent.testDetails.testName).toBe(test.testName);
        expect(createTestHeaderComponent.isTestNameExist).toBe(false);
    });

    it('should display valid test name when test name already exists', () => {
        createTestHeaderComponent.isTestNameExist = true;
        createTestHeaderComponent.testNameReference = test.testName;
        test.testName = 'English';
        createTestHeaderComponent.testDetails = test;
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.updateTestName(test.id, test);
        createTestHeaderComponent.showEditButton(test.testName);
        expect(createTestHeaderComponent.testDetails.testName).toBe('English');
        expect(createTestHeaderComponent.nameOfTest).toBe('English');
    });

    it('should update test name on pressing enter button if test name is valid', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.testId = test.id;
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.isButtonClicked = false;
        createTestHeaderComponent.onEnter('Computer');
        expect(TestService.prototype.updateTestName).toHaveBeenCalledTimes(1);
    });

    it('should not update test name on pressing enter button if test name is invalid', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.testId = test.id;
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.isButtonClicked = true;
        createTestHeaderComponent.onEnter('Computer');
        expect(TestService.prototype.updateTestName).toHaveBeenCalledTimes(0);
    });

    it('should update the test name', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        test.testName = 'G.K';
        createTestHeaderComponent.testId = test.id;
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.updateTestName(test.id, test);
        expect(createTestHeaderComponent.isButtonClicked).toBe(false);
        expect(createTestHeaderComponent.testName).toBe(test.testName);
        expect(createTestHeaderComponent.editedTestName).toBe(test.testName);
        expect(createTestHeaderComponent.isLabelVisible).toBe(true);
    });

    it('should not update the test name when test name is not unique', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(false);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.testId = test.id;
        createTestHeaderComponent.testDetails = test;
        createTestHeaderComponent.updateTestName(test.id, test);
        expect(createTestHeaderComponent.isButtonClicked).toBe(false);
        expect(createTestHeaderComponent.isTestNameExist).toBe(true);
        expect(createTestHeaderComponent.isLabelVisible).toBe(false);
    });

    it('should display updated test name', () => {
        createTestHeaderComponent.isTestNameExist = true;
        createTestHeaderComponent.testNameReference = test.testName;
        test.testName = 'English';
        createTestHeaderComponent.testDetails = test;
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.updateTestName(test.id, test);
        createTestHeaderComponent.hideEditButton();
        expect(createTestHeaderComponent.testDetails.testName).toBe('English');
    });

    it('should not update test name on getting an error', () => {
        createTestHeaderComponent.isTestNameExist = true;
        createTestHeaderComponent.testNameReference = test.testName;
        test.testName = 'English';
        createTestHeaderComponent.testDetails = test;
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.throw(Error);
        });
        spyOn(createTestHeaderComponent, 'openSnackBar').and.callThrough();
        createTestHeaderComponent.updateTestName(test.id, test);
        expect(createTestHeaderComponent.isButtonClicked).toBe(false);
        expect(createTestHeaderComponent.openSnackBar).toHaveBeenCalled();
    });

    it('should display the updated test name along with the edit button', () => {
        createTestHeaderComponent.testNameReference = test.testName;
        test.testName = 'English';
        createTestHeaderComponent.testDetails = test;
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'updateTestName').and.callFake(() => {
            return Observable.of(test);
        });
        createTestHeaderComponent.updateTestName(test.id, test);
        createTestHeaderComponent.showEditButton(test.testName);
        expect(createTestHeaderComponent.testDetails.testName).toBe('English');
    });

    it('should change the error message', () => {
        createTestHeaderComponent.changeErrorMessage();
        expect(createTestHeaderComponent.isTestNameExist).toBe(false);
        expect(createTestHeaderComponent.isWhiteSpaceError).toBe(false);
    });

    it('should change the tooltip message', () => {
        createTestHeaderComponent.changeTooltipMessage();
        expect(createTestHeaderComponent.tooltipMessage).toBe('Copy to Clipboard');
    });

    it('should call ngOnInit()', () => {
        createTestHeaderComponent.ngOnInit();
        expect(createTestHeaderComponent.testId).toBe(1);
    });

    it('should call showTooltipMessage()', () => {
        let event: any = { };
        event.stopPropagation = function() { };
        let element: any = {};
        element.select = function() { };
        createTestHeaderComponent.showTooltipMessage(event, element);
        expect(createTestHeaderComponent.tooltipMessage).toBe('Copied');
    });

    it('should call selectAllContent()', () => {
        let event: any = {};
        event.target = function() { };
        event.target.select = function() { };
        spyOn(createTestHeaderComponent, 'selectAllContent');
        createTestHeaderComponent.selectAllContent(event);
        expect(createTestHeaderComponent.selectAllContent).toHaveBeenCalled();
    });

});
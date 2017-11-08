import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule, FormGroup } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog, MdSnackBar, MdSnackBarRef } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
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
import { CreateTestHeaderComponent } from '../create-test-header/create-test-header.component';
import { TestSettingsComponent } from '../../test-settings/test-settings.component';
import { HttpService } from '../../../core/http.service';
import { CreateTestFooterComponent } from '../create-test-footer/create-test-footer.component';
import { IncompleteTestCreationDialogComponent } from '../../test-settings/incomplete-test-creation-dialog.component';
import { MockRouteService } from '../../../questions/questions-single-multiple-answer/mock-route.service';


class MockRouter {
    navigate() {
        return true;
    }

    isActive() {
        return true;
    }

    navigateByUrl(url: string) { return url; }
}

describe('Create Test Footer Component', () => {

    let createTestFooterComponent: CreateTestFooterComponent;
    let fixture: ComponentFixture<CreateTestFooterComponent>;
    let saveTestSettings: any = new EventEmitter();
    let launchTestDialog: any = new EventEmitter();
    let saveExit: any = new EventEmitter();
    let saveNext: any = new EventEmitter();
    let pauseTest: any = new EventEmitter();
    let resumeTest: any = new EventEmitter();
    let saveCategory: any = new EventEmitter();

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
                entryComponents: [TestSettingsComponent]
            }
        });

        TestBed.configureTestingModule({

            declarations: [TestSettingsComponent, CreateTestHeaderComponent, CreateTestFooterComponent, IncompleteTestCreationDialogComponent],

            providers: [
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: APP_BASE_HREF, useValue: '/' },
                TestService,
                HttpService,
                MockRouteService
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule.forRoot([]), HttpModule, BrowserAnimationsModule, PopoverModule, ClipboardModule, Md2AccordionModule.forRoot(), MdDialogModule]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CreateTestFooterComponent);
        createTestFooterComponent = fixture.componentInstance;
        saveTestSettings.emit = function () { };
        launchTestDialog.emit = function () { };
        saveExit.emit = function () { };
        saveNext.emit = function () { };
        pauseTest.emit = function () { };
        resumeTest.emit = function () { };
        saveCategory.emit = function () { };
        createTestFooterComponent.testId = test.id;
    });



    it('should should call the getComponent()', () => {
        let url = '/';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        createTestFooterComponent.getComponent();
        expect(createTestFooterComponent.isTestSection).toBe(false);
        expect(createTestFooterComponent.isTestQuestion).toBe(false);
        expect(createTestFooterComponent.isTestSettings).toBe(false);
    });

    it('should load the test sections component', () => {
        let url = '/tests/3/sections';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        createTestFooterComponent.getComponent();
        expect(createTestFooterComponent.isTestSection).toBe(true);
        expect(createTestFooterComponent.isTestQuestion).toBe(false);
        expect(createTestFooterComponent.isTestSettings).toBe(false);
    });

    it('should load the test questions component', () => {
        let url = '/tests/3/questions';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        createTestFooterComponent.getComponent();
        expect(createTestFooterComponent.isTestSection).toBe(false);
        expect(createTestFooterComponent.isTestQuestion).toBe(true);
        expect(createTestFooterComponent.isTestSettings).toBe(false);
    });

    it('should load the test settings component', () => {
        let url = '/tests/3/settings';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        createTestFooterComponent.getComponent();
        expect(createTestFooterComponent.isTestSection).toBe(false);
        expect(createTestFooterComponent.isTestQuestion).toBe(false);
        expect(createTestFooterComponent.isTestSettings).toBe(true);
    });

    it('should emit the event saveTestSettings', () => {
        spyOn(saveTestSettings, 'emit');
        createTestFooterComponent.saveTestSettings = saveTestSettings;
        createTestFooterComponent.updateTestSettings();
        expect(saveTestSettings.emit).toHaveBeenCalled();
    });

    it('should emit the event launchTestDialog', () => {
        spyOn(launchTestDialog, 'emit');
        createTestFooterComponent.launchTestDialog = launchTestDialog;
        createTestFooterComponent.launchTestDialogBox();
        expect(createTestFooterComponent.isTestLaunched).toBe(true);
        expect(launchTestDialog.emit).toHaveBeenCalled();
    });

    it('should emit the event saveExit in test-questions component', () => {
        spyOn(saveExit, 'emit');
        createTestFooterComponent.saveExit = saveExit;
        createTestFooterComponent.saveAndExit();
        expect(saveExit.emit).toHaveBeenCalled();
    });

    it('should emit the event addTestQuestion in test-questions component', () => {
        spyOn(saveNext, 'emit');
        createTestFooterComponent.saveNext = saveNext;
        createTestFooterComponent.addTestQuestions();
        expect(saveNext.emit).toHaveBeenCalled();
    });

    it('should emit the event pauseTest', () => {
        spyOn(pauseTest, 'emit');
        createTestFooterComponent.pauseTest = pauseTest;
        createTestFooterComponent.pausTest();
        expect(pauseTest.emit).toHaveBeenCalled();
    });

    it('should emit the event resumeTest', () => {
        spyOn(resumeTest, 'emit');
        createTestFooterComponent.resumeTest = resumeTest;
        createTestFooterComponent.resumTest();
        expect(resumeTest.emit).toHaveBeenCalled();
    });

    it('should emit the event saveSelectedCategoryAndExit', () => {
        spyOn(saveCategory, 'emit');
        createTestFooterComponent.SaveCategory = saveCategory;
        createTestFooterComponent.saveSelectedCategoryAndExit();
        expect(createTestFooterComponent.isSelectButton).toBe(false);
        expect(saveCategory.emit).toHaveBeenCalled();
    });

    it('should emit the event saveSelectedCategoryandMoveNext', () => {
        spyOn(saveCategory, 'emit');
        createTestFooterComponent.SaveCategory = saveCategory;
        createTestFooterComponent.saveSelectedCategoryAndMoveNext();
        expect(createTestFooterComponent.isSelectButton).toBe(true);
        expect(saveCategory.emit).toHaveBeenCalled();
    });

});
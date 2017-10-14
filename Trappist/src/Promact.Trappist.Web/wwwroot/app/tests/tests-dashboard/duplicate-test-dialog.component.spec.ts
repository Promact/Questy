import { TestsDashboardComponent } from './tests-dashboard.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog, MdSnackBar } from '@angular/material';
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

class MockSnackbar {
    open() {
        return true;
    }
}

describe('Duplicate Test Dialog Component', () => {

    let duplicateTestDialogComponent: DuplicateTestDialogComponent;
    let fixture: ComponentFixture<DuplicateTestDialogComponent>;
    let router: Router;
    let dialog: MdDialog;
    let urls: any[];
    let test = new Test();
    test.id = 1;
    test.numberOfTestAttendees = 2;
    test.testName = 'History';
    test.isEditTestEnabled = true;

    beforeEach(async(() => {

        TestBed.configureTestingModule({

            declarations: [DuplicateTestDialogComponent],

            providers: [
                TestService,
                HttpService, MdDialogModule,
                { provide: Router, useClass: MockRouter },
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: MdDialog, useClass: MockDialog },
                { provide: MdSnackBar, useClass: MockSnackbar }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, HttpModule, BrowserAnimationsModule]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(DuplicateTestDialogComponent);
        duplicateTestDialogComponent = fixture.componentInstance;
    });

    it('should duplicate the test', fakeAsync((() => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(TestService.prototype, 'duplicateTest').and.callFake(() => {
            return Observable.of(MockTestData[0]);
        });
        router = TestBed.get(Router);
        spyOn(router, 'navigate').and.callFake(function (url: any) {
            urls = url;
        });
        duplicateTestDialogComponent.testArray[0] = test;
        duplicateTestDialogComponent.testToDuplicate = test;
        duplicateTestDialogComponent.duplicateTest();
        tick();
        expect(duplicateTestDialogComponent.testArray.length).toBe(2);
        expect(urls[0]).toBe('tests/' + MockTestData[0].id + '/sections');
    })));

    it('should not make the error value true by not duplicating the test', fakeAsync((() => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return Observable.of(false);
        });
        duplicateTestDialogComponent.testToDuplicate = test;
        duplicateTestDialogComponent.duplicateTest();
        tick();
        expect(duplicateTestDialogComponent.error).toBe(true);
    })));

    it('should make the error value false', () => {
        duplicateTestDialogComponent.onErrorChange();
        expect(duplicateTestDialogComponent.error).toBe(false);
    });

});


import {throwError as observableThrowError, of as observableOf,  Observable ,  BehaviorSubject } from 'rxjs';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { HttpService } from '../../core/http.service';
import { MockTestData } from '../../Mock_Data/test_data.mock';
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
            return observableOf(true);
        });
        spyOn(TestService.prototype, 'duplicateTest').and.callFake(() => {
            return observableOf(MockTestData[0]);
        });
        router = TestBed.get(Router);
        spyOn(router, 'navigate').and.callFake(function (url: any) {
            urls = url;
        });
        spyOn(duplicateTestDialogComponent.snackBar, 'open').and.callThrough();
        spyOn(duplicateTestDialogComponent.dialog, 'close').and.callThrough();
        duplicateTestDialogComponent.testArray[0] = test;
        duplicateTestDialogComponent.testToDuplicate = test;
        duplicateTestDialogComponent.duplicateTest();
        tick();
        expect(duplicateTestDialogComponent.testArray.length).toBe(2);
        expect(urls[0]).toBe('tests/' + MockTestData[0].id + '/sections');
        expect(duplicateTestDialogComponent.snackBar.open).toHaveBeenCalled();
        expect(duplicateTestDialogComponent.dialog.close).toHaveBeenCalled();
    })));

    it('should not make the error value true by not duplicating the test', fakeAsync((() => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return observableOf(false);
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

    it('should not duplicate the test on getting error', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return observableThrowError(Error);
        });
        spyOn(TestService.prototype, 'duplicateTest').and.callFake(() => {
            return observableOf(MockTestData[0]);
        });
        duplicateTestDialogComponent.testArray[0] = test;
        duplicateTestDialogComponent.testToDuplicate = test;
        duplicateTestDialogComponent.duplicateTest();
        expect(duplicateTestDialogComponent.testArray.length).toBe(1);
        expect(duplicateTestDialogComponent.loader).toBe(false);
        expect(TestService.prototype.duplicateTest).toHaveBeenCalledTimes(0);
    });

    it('should not duplicate the test on getting error from duplicate test service', () => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.callFake(() => {
            return observableOf(true);
        });
        spyOn(TestService.prototype, 'duplicateTest').and.callFake(() => {
            return observableThrowError(Error);
        });
        spyOn(duplicateTestDialogComponent.snackBar, 'open').and.callThrough();
        spyOn(duplicateTestDialogComponent.dialog, 'close').and.callThrough();
        duplicateTestDialogComponent.testArray[0] = test;
        duplicateTestDialogComponent.testToDuplicate = test;
        duplicateTestDialogComponent.duplicateTest();
        expect(duplicateTestDialogComponent.testArray.length).toBe(1);
        expect(duplicateTestDialogComponent.loader).toBe(false);
        expect(duplicateTestDialogComponent.snackBar.open).toHaveBeenCalledTimes(0);
        expect(duplicateTestDialogComponent.dialog.close).toHaveBeenCalledTimes(0);
    });

});

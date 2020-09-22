
import {throwError as observableThrowError, of as observableOf,  Observable ,  BehaviorSubject } from 'rxjs';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { HttpService } from '../../core/http.service';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { tick } from '@angular/core/testing';
import { Location, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { fakeAsync } from '@angular/core/testing';
import { DeleteTestDialogComponent } from './delete-test-dialog.component';
import { AppComponent } from '../../app.component';
import { SharedModule } from '../../shared/shared.module';
import { MdSnackBar } from '@angular/material';
import { MockRouteService } from '../../questions/questions-single-multiple-answer/mock-route.service';




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

    close() {
        return true;
    }
}

class MockSnackBar {
    open() {
        return true;
    }
}


describe('Delete Test Dialog Component', () => {

    let deleteTestDialogComponent: DeleteTestDialogComponent;
    let fixture: ComponentFixture<DeleteTestDialogComponent>;
    let test = new Test();
    test.id = 1;
    test.testName = 'History';
    test.warningTime = 5;
    test.numberOfTestAttendees = 0;
    let router: Router;
    let snackbar;
    let urls: any[];

    beforeEach(async(() => {

        TestBed.configureTestingModule({

            declarations: [DeleteTestDialogComponent],

            providers: [
                TestService,
                HttpService, MdDialogModule,
                { provide: Router, useClass: MockRouter },
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: MdSnackBar, useClass: MockSnackBar },
                MockRouteService
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, HttpModule, BrowserAnimationsModule]
        }).compileComponents();

    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(DeleteTestDialogComponent);
        deleteTestDialogComponent = fixture.componentInstance;
    });

    it('should delete the test', (() => {
        let url = '/tests/1/view';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        spyOn(TestService.prototype, 'deleteTest').and.callFake(() => {
            return observableOf('');
        });
        spyOn(deleteTestDialogComponent.snackBar, 'open').and.callThrough();
        spyOn(deleteTestDialogComponent.dialog, 'close').and.callThrough();
        spyOn(Router.prototype, 'navigate').and.callFake(function (url: any[]) {
            urls = url;
            expect(urls[0]).toBe('/tests');
        });
        deleteTestDialogComponent.testToDelete = test;
        deleteTestDialogComponent.testArray[0] = test;
        deleteTestDialogComponent.deleteTest();
        expect(deleteTestDialogComponent.testArray.length).toBe(0);
        expect(deleteTestDialogComponent.snackBar.open).toHaveBeenCalled();
        expect(deleteTestDialogComponent.dialog.close).toHaveBeenCalled();
    }));

    it('should not delete the test on getting error', () => {
        spyOn(TestService.prototype, 'deleteTest').and.callFake(() => {
            return observableThrowError(Error);
        });
        spyOn(deleteTestDialogComponent.snackBar, 'open').and.callThrough();
        spyOn(deleteTestDialogComponent.dialog, 'close').and.callThrough();
        deleteTestDialogComponent.testToDelete = test;
        deleteTestDialogComponent.testArray[0] = test;
        deleteTestDialogComponent.deleteTest();
        expect(deleteTestDialogComponent.testArray.length).toBe(1);
        expect(deleteTestDialogComponent.snackBar.open).toHaveBeenCalled();
        expect(deleteTestDialogComponent.dialog.close).toHaveBeenCalledTimes(0);
    });

    it('should not be in test view page', () => {
        let url = '/';
        spyOn(MockRouteService.prototype, 'getCurrentUrl').and.returnValue(url);
        spyOn(TestService.prototype, 'deleteTest').and.callFake(() => {
            return observableOf('');
        });
        spyOn(Router.prototype, 'navigate').and.callFake(() => { });
        deleteTestDialogComponent.testToDelete = test;
        deleteTestDialogComponent.testArray[0] = test;
        deleteTestDialogComponent.deleteTest();
        expect(Router.prototype.navigate).toHaveBeenCalledTimes(0);
    });

});
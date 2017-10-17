import { TestsDashboardComponent } from './tests-dashboard.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, OverlayRef, MdSnackBar } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { FilterPipe } from './test-dashboard.pipe';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { TestService } from '../tests.service';
import { inject, tick } from '@angular/core/testing';
import { Test } from '../tests.model';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { fakeAsync } from '@angular/core/testing';



class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate(url: any[]) { return url; }
}

describe('Test Dashboard Component', () => {
    let testDashboard: TestsDashboardComponent;
    let router: Router;
    let dialog: MdDialog;
    let fixtureDashboard: ComponentFixture<TestsDashboardComponent>;
    let http: HttpService;
    let dialogRef: MdDialogRef<TestCreateDialogComponent>;
    let mockData: any[] = [];
    let urlRedirect: any[] = [];

    beforeEach(async(() => {
        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [TestCreateDialogComponent]
            }

        });
        TestBed.configureTestingModule({
            declarations: [TestsDashboardComponent, FilterPipe, TestCreateDialogComponent],
            providers: [
                QuestionsService,
                TestService,
                HttpService,
                { provide: Router, useClass: RouterStub }

            ],
            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, HttpModule, BrowserAnimationsModule, MdDialogModule]
        }).compileComponents();
    }));
    beforeEach(() => {
        mockData = JSON.parse(JSON.stringify(MockTestData));
        router = TestBed.get(Router);
        fixtureDashboard = TestBed.createComponent(TestsDashboardComponent);
        testDashboard = fixtureDashboard.componentInstance;
        spyOn(Router.prototype, 'navigate').and.callFake((url: any[]) => {
            urlRedirect = url;
        });
    });

    it('getAllTest', async(() => {
        spyOn(TestService.prototype, 'getTests').and.callFake(() => {
            let result = new BehaviorSubject(mockData);
            return result.asObservable();
        });
        testDashboard.getAllTests();
        expect(testDashboard.tests).toBe(mockData);
    }));

    it('createTestDialog ', () => {
        spyOn(testDashboard.dialog, 'open').and.callThrough();
        spyOn(MdDialogRef.prototype, 'afterClosed').and.returnValue(Observable.of(mockData[0]));
        testDashboard.createTestDialog();
        expect(testDashboard.dialog.open).toHaveBeenCalled();
        expect(testDashboard.tests.length).toBe(1);
    });
});
import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { TestsDashboardComponent } from '../tests-dashboard/tests-dashboard.component';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { RouterModule, Router, ActivatedRoute, Routes, Params } from '@angular/router';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestService } from '../tests.service';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { FilterPipe } from '../tests-dashboard/test-dashboard.pipe';
import { CreateTestFooterComponent } from '../shared/create-test-footer/create-test-footer.component';
import { CreateTestHeaderComponent } from '../shared/create-test-header/create-test-header.component';
import { Md2AccordionModule } from 'md2';
import { PopoverModule } from 'ngx-popover';
import { ClipboardModule } from 'ngx-clipboard';
import { Observable } from 'rxjs/Observable';
import { APP_BASE_HREF } from '@angular/common';
import { TestSectionsComponent } from './test-sections.component';
import { DeselectCategoryComponent } from './deselect-category.component';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

describe('Test Section', () => {
    let fixtureSection: ComponentFixture<TestSectionsComponent>;
    let testSection: TestSectionsComponent;
    let router: Router;
    let routeTo: any[];
    let mockData: any[] = [];


    beforeEach(async(() => {
        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [DeselectCategoryComponent]
            }
        });
        TestBed.configureTestingModule({
            declarations: [TestSectionsComponent, FilterPipe, CreateTestFooterComponent, CreateTestHeaderComponent, TestsDashboardComponent, DeselectCategoryComponent],
            providers: [
                QuestionsService,
                { provide: ActivatedRoute, useValue: { params: Observable.of({}) } },
                TestService,
                HttpService,
                MdSnackBar,
                { provide: APP_BASE_HREF, useValue: '/' }
            ],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule, Md2AccordionModule.forRoot(), PopoverModule, ClipboardModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        mockData = JSON.parse(JSON.stringify(MockTestData));
        router = TestBed.get(Router);
        fixtureSection = TestBed.createComponent(TestSectionsComponent);
        testSection = fixtureSection.componentInstance;
        spyOn(TestService.prototype, 'getTestById').and.callFake((id: number) => {
            let test = mockData.find(x => x.id === id);
            if (test === undefined)
                return Observable.throw('Not found');
            return Observable.of(mockData.find(x => x.id === id));
        });
        spyOn(TestService.prototype, 'deselectCategory').and.callFake(function (categoryId: number, testId: number) {
            let test = MockTestData.find(x => x.id === testId);
            let category = test.categoryAcList.find(x => x.id === categoryId);
            return Observable.of(category.questionList !== null);
        });
        spyOn(router, 'navigate').and.callFake((route: any[]) => {
            routeTo = route;
        });
        spyOn(MdSnackBar.prototype, 'open').and.callThrough();
    });

    it('getTestById', () => {
        testSection.getTestById(MockTestData[0].id);
        expect(testSection.testDetails).toBe(mockData[0]);
        expect(testSection.testDetails.testName).toBe(MockTestData[0].testName);
    });

    it('getTestById error handling', () => {
        testSection.getTestById(5);
        expect(MdSnackBar.prototype.open).toHaveBeenCalled();
        expect(routeTo[0]).toBe('/tests');
    });


    it('onSelect', () => {
        spyOn(testSection.dialog, 'open').and.callThrough();
        testSection.isEditTestEnabled = true;
        testSection.getTestById(MockTestData[0].id);
        testSection.onSelect(MockTestData[0].categoryAcList[0]);
        expect(testSection.dialog.open).toHaveBeenCalled();
    });

    it('saveCategoryToExitOrMoveNext', () => {
        spyOn(TestService.prototype, 'addTestCategories').and.returnValue(Observable.of(true));
        testSection.getTestById(MockTestData[0].id);
        testSection.testId = MockTestData[0].id;
        testSection.saveCategoryToExitOrMoveNext(true);
        expect(routeTo[0]).toBe('/tests/' + MockTestData[0].id + '/questions');
        testSection.saveCategoryToExitOrMoveNext(false);
        expect(routeTo[0]).toBe('/tests');
    });

    it('saveCategoryToExitOrMoveNext error handling', () => {
        spyOn(TestService.prototype, 'addTestCategories').and.returnValue(Observable.throw('error'));
        testSection.getTestById(MockTestData[0].id);
        testSection.testId = MockTestData[0].id;
        testSection.isEditTestEnabled = true;
        testSection.saveCategoryToExitOrMoveNext(true);
        expect(MdSnackBar.prototype.open).toHaveBeenCalled();
    });


    it('isAttendeeExist ', () => {
        spyOn(TestService.prototype, 'isTestAttendeeExist').and.callFake((testId: number) => {
            let test = MockTestData.find(x => x.id === testId);
            return Observable.of({ response: test.numberOfTestAttendees !== 0 });
        });
        testSection.testId = MockTestData[0].id;
        testSection.isTestAttendeeExist();
        expect(testSection.isEditTestEnabled).toBe(false);
    });
});
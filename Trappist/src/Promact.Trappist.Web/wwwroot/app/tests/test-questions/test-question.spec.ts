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
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { TestQuestionsComponent } from './test-questions.component';
import { FilterPipe } from '../tests-dashboard/test-dashboard.pipe';
import { CreateTestFooterComponent } from '../shared/create-test-footer/create-test-footer.component';
import { CreateTestHeaderComponent } from '../shared/create-test-header/create-test-header.component';
import { Md2AccordionModule } from 'md2';
import { PopoverModule } from 'ngx-popover';
import { ClipboardModule } from 'ngx-clipboard';
import { Observable } from 'rxjs/Observable';
import { APP_BASE_HREF } from '@angular/common';
import { Test } from '../tests.model';



class MockActivatedRoute {

    params = Observable.of({ 'id': MockTestData[0].id });
}

describe('Test question Component', () => {
    let router: Router;
    let testQuestion: TestQuestionsComponent;
    let fixture: ComponentFixture<TestQuestionsComponent>;
    let routeTo: any[];
    let mockdata: any[] = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TestQuestionsComponent, FilterPipe, CreateTestFooterComponent, CreateTestHeaderComponent, TestsDashboardComponent],
            providers: [
                QuestionsService,
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                TestService,
                HttpService,
                MdSnackBar,
                { provide: APP_BASE_HREF, useValue: '/' }
            ],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule, Md2AccordionModule.forRoot(), PopoverModule, ClipboardModule]
        }).compileComponents();

    }));
    beforeEach(() => {
        mockdata = JSON.parse(JSON.stringify(MockTestData));
        fixture = TestBed.createComponent(TestQuestionsComponent);
        testQuestion = fixture.componentInstance;
        router = TestBed.get(Router);
        spyOn(TestService.prototype, 'getTestById').and.callFake(function (id: number) {
            console.log(id);
            return Observable.of(mockdata.find(x => x.id === id));
        });
        spyOn(TestService.prototype, 'getQuestions').and.returnValue(Observable.of(MockTestData[0].categoryAcList[0].questionList));
        spyOn(router, 'navigate').and.callFake(function (route: any[]) {
            routeTo = route; 
        });
    });


    it('getTestDetails ', () => {
        testQuestion.getTestDetails();
        console.log('original data');
        console.log(MockTestData);
        expect(testQuestion.testDetails).toBe(mockdata[0]);
        expect(testQuestion.isAnyCategorySelectedForTest).toBe(true);
    });

    it('getAllQuestions ', () => {
        testQuestion.getTestDetails();
        testQuestion.getAllquestions(mockdata[0].categoryAcList[0], 0);
    });
    it('selectQuestion ', () => {
        let questionToSelect = mockdata[0].categoryAcList[0].questionList[0];
        testQuestion.getTestDetails();
        testQuestion.getAllquestions(mockdata[0].categoryAcList[0], 0);
        testQuestion.selectQuestion(questionToSelect, mockdata[0].categoryAcList[0]);
        expect(testQuestion.testDetails.categoryAcList[0].selectAll).toBe(true);
    });
    it('save next ', () => {
        spyOn(TestService.prototype, 'addTestQuestions').and.returnValue(Observable.of({ message: 'question Added' }));
        testQuestion.getTestDetails();
        testQuestion.isEditTestEnabled = true;
        testQuestion.testId = MockTestData[0].id;
        testQuestion.saveNext();
        testQuestion.isEditTestEnabled = false;
        testQuestion.saveNext();
        expect(routeTo[0]).toBe('tests/' + MockTestData[0].id + '/settings');
    });

    it('svae exit', () => {
        testQuestion.saveExit();
        expect(routeTo[0]).toBe('/tests');
    });
});
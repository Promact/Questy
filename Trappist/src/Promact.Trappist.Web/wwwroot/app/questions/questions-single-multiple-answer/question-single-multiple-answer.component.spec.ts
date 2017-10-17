import { CategoryService } from '../categories.service';
import { QuestionsService } from '../questions.service';
import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import { APP_BASE_HREF } from '@angular/common';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { SingleMultipleAnswerQuestionComponent } from './questions-single-multiple-answer.component';
import { Observable } from 'rxjs/Observable';
import { TinymceModule } from 'angular2-tinymce';
import { QuestionBase } from '../question';

class MockActivatedRoute {
    params = Observable.of({ 'id': MockTestData[0].id });
}

describe('Single multiple questions', () => {
    let singleMultipleFixture: ComponentFixture<SingleMultipleAnswerQuestionComponent>;
    let singleMultipleComponent: SingleMultipleAnswerQuestionComponent;
    let mockData: any[] = [];
    let routeTo: any[] = [];

    //let mockDialogRef = new MdDialogRef(new OverlayRef(null, null, null, null, null), null);
    beforeEach(async(() => {
        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: []
            }
        });
        TestBed.configureTestingModule({

            declarations: [SingleMultipleAnswerQuestionComponent],
            providers: [
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                QuestionsService,
                CategoryService,
                HttpService
            ],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule, TinymceModule]
        }).compileComponents();


    }));
    beforeEach(() => {
        spyOn(Router.prototype, 'navigate').and.callFake((route: any[]) => {
            routeTo = route;
        });
        mockData = JSON.parse(JSON.stringify(MockTestData));
        singleMultipleFixture = TestBed.createComponent(SingleMultipleAnswerQuestionComponent);
        singleMultipleComponent = singleMultipleFixture.componentInstance;
    });
    it('getQuestionById', () => {
        singleMultipleComponent.categoryArray = mockData[0].categoryAcList;
        let question = mockData[0].categoryAcList[0].questionList[0];
        spyOn(QuestionsService.prototype, 'getQuestionById').and.returnValue(Observable.of(question));
        singleMultipleComponent.getQuestionById(question.id);
        expect(singleMultipleComponent.categoryName).toBe(mockData[0].categoryAcList[0].categoryName);
        expect(singleMultipleComponent.noOfOptionShown).toBe(2);
        singleMultipleComponent.addOption(2);
        expect(singleMultipleComponent.noOfOptionShown).toBe(3);
        singleMultipleComponent.removeOption(2);
        expect(singleMultipleComponent.noOfOptionShown).toBe(2);


        //let optionName = mockData[0].categoryAcList[0].questionList[0].singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].option;
        //singleMultipleComponent.isTwoOptionsSame(optionName, 1);

    });

    it('isTwoOptinsSame', () => {
        singleMultipleComponent.categoryArray = mockData[0].categoryAcList;
        let question = mockData[0].categoryAcList[0].questionList[0];
        spyOn(QuestionsService.prototype, 'getQuestionById').and.returnValue(Observable.of(question));
        singleMultipleComponent.getQuestionById(question.id);

        let optionName = mockData[0].categoryAcList[0].questionList[0].singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].option;
        singleMultipleComponent.isTwoOptionsSame(optionName, 0);
        expect(singleMultipleComponent.isTwoOptionSame).toBe(true);
    });

    it('save question', () => {
        spyOn(QuestionsService.prototype, 'addSingleMultipleAnswerQuestion').and.returnValue(Observable.of(true));
        spyOn(QuestionsService.prototype, 'updateSingleMultipleAnswerQuestion').and.returnValue(Observable.of(true));
        let question: QuestionBase;
        question = new QuestionBase();
        question.question = mockData[0].categoryAcList[0].questionList[0].question;
        question.singleMultipleAnswerQuestion = mockData[0].categoryAcList[0].questionList[0].singleMultipleAnswerQuestion;
        singleMultipleComponent.indexOfOptionSelected = 0;
        singleMultipleComponent.isEditQuestion = false;
        singleMultipleComponent.categoryName = mockData[0].categoryAcList[0].categoryName;
        singleMultipleComponent.difficultyLevelSelected = mockData[0].categoryAcList[0].questionList[0].question.difficultyLevel.toString();
        singleMultipleComponent.saveSingleMultipleAnswerQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2]).toBe('questions/dashboard/' + mockData[0].categoryAcList[0].categoryName + '/0');
        singleMultipleComponent.isEditQuestion = true;
        singleMultipleComponent.saveSingleMultipleAnswerQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2]).toBe('questions/dashboard/' + mockData[0].categoryAcList[0].categoryName + '/0');
    });

});
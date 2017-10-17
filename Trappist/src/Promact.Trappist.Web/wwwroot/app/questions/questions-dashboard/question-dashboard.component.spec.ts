import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule, XHRBackend } from '@angular/http';
import { QuestionsDashboardComponent } from './questions-dashboard.component';
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router';
import { CategoryService } from '../categories.service';
import { QuestionsService } from '../questions.service';
import { AddCategoryDialogComponent } from './add-category-dialog.component';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import { InfiniteScrollModule } from 'angular2-infinite-scroll';
import { APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { UpdateCategoryDialogComponent } from './update-category-dialog.component';
import { QuestionDisplay } from '../question-display';
import { DeleteCategoryDialogComponent } from './delete-category-dialog.component';
import { DeleteQuestionDialogComponent } from './delete-question-dialog.component';
import { QuestionCount } from '../numberOfQuestion';



class MockActivatedRoute {
    params = Observable.of({ 'id': MockTestData[0].id });
}

describe('Question dashboard', () => {
    let questionFixture: ComponentFixture<QuestionsDashboardComponent>;
    let questionComponent: QuestionsDashboardComponent;
    let mockData: any[] = [];
    let routeTo: any[] = [];

    //let mockDialogRef = new MdDialogRef(new OverlayRef(null, null, null, null, null), null);


    beforeEach(async(() => {

        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [AddCategoryDialogComponent, UpdateCategoryDialogComponent, DeleteCategoryDialogComponent, DeleteQuestionDialogComponent]
            }
        });
        TestBed.configureTestingModule({

            declarations: [QuestionsDashboardComponent, AddCategoryDialogComponent, UpdateCategoryDialogComponent, DeleteCategoryDialogComponent, DeleteQuestionDialogComponent],
            providers: [
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                QuestionsService,
                CategoryService,
                HttpService
            ],

            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, InfiniteScrollModule, HttpModule, BrowserAnimationsModule, MdDialogModule]


        }).compileComponents();

    }));
    beforeEach(() => {
        mockData = JSON.parse(JSON.stringify(MockTestData));
        questionFixture = TestBed.createComponent(QuestionsDashboardComponent);
        questionComponent = questionFixture.componentInstance;
        spyOn(Router.prototype, 'navigate').and.callFake((route: any[]) => {
            routeTo = route;
        });

    });

    it('Add CategoryDialog', () => {
        spyOn(MdDialogRef.prototype, 'afterClosed').and.returnValue(Observable.of(mockData[0].categoryAcList[0]));
        spyOn(questionComponent.dialog, 'open').and.callThrough();
        questionComponent.addCategoryDialog();
        expect(questionComponent.dialog.open).toHaveBeenCalled();
        expect(questionComponent.categoryArray.length).toBe(1);
    });

    it('Update Category', () => {
        let question = new QuestionDisplay();
        question.category = mockData[0].categoryAcList[0];
        question.id = mockData[0].categoryAcList[0].questionList[0].id;
        question.questionType = 2;
        questionComponent.question.push(question);
        questionComponent.addCategoryDialog();
        mockData[0].categoryAcList[0].categoryName = 'updated category';
        questionComponent.updateCategoryDialog(mockData[0].categoryAcList[0]);
        expect(questionComponent.question[0].category.categoryName).toBe('updated category');

    });

    it('delete category', () => {
        spyOn(MdDialogRef.prototype, 'afterClosed').and.returnValue(Observable.of(mockData[0].categoryAcList[0]));
        questionComponent.categoryArray = mockData[0].categoryAcList;
        expect(questionComponent.categoryArray.length).toBe(2);
        questionComponent.deleteCategoryDialog(mockData[0].categoryAcList[1]);
        expect(questionComponent.categoryArray.length).toBe(1);
    });

    it('delete question dialog', () => {
        let question = new QuestionDisplay();
        question.category = mockData[0].categoryAcList[0];
        question.id = mockData[0].categoryAcList[0].questionList[0].id;
        question.questionType = 2;
        questionComponent.question.push(question);
        expect(questionComponent.question.length).toBe(1);
        let questionCount = new QuestionCount();
        spyOn(MdDialogRef.prototype, 'afterClosed').and.returnValue(Observable.of(mockData[0].categoryAcList[0].questionList[0]));
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.returnValue(Observable.of(questionCount));
        questionComponent.deleteQuestionDialog(mockData[0].categoryAcList[0].questionList[0]);
        expect(questionComponent.question.length).toBe(0);
    });

    it('edit question', () => {
        let question = new QuestionDisplay();
        question.id = mockData[0].categoryAcList[0].questionList[0].question.id;
        question.questionType = 2;
        questionComponent.editQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2]).toBe('questions/' + 'programming/' + question.id);
    });


    it('duplicate question', () => {
        let question = new QuestionDisplay();
        question.id = mockData[0].categoryAcList[0].questionList[0].question.id;
        question.questionType = 2;
        questionComponent.duplicateQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/' + 'programming/' + 'duplicate/' + question.id);
        question.questionType = 1;
        questionComponent.duplicateQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/' + 'multiple-answers/' + 'duplicate/' + question.id);
        question.questionType = 0;
        questionComponent.duplicateQuestion(question);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/' + 'single-answer/' + 'duplicate/' + question.id);

    });
});
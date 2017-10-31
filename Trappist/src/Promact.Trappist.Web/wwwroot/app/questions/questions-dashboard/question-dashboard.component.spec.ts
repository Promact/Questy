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
import { SingleMultipleAnswerQuestionOption } from '../single-multiple-answer-question-option.model';
import { SingleMultipleAnswerQuestion } from '../single-multiple-question';
import { QuestionBase } from '../question';
import { Question } from '../question.model';
import { BehaviorSubject } from 'rxjs/Rx';
import { QuestionType } from '../enum-questiontype';
import { DifficultyLevel } from '../enum-difficultylevel';
import { Category } from '../category.model';

class MockActivatedRoute {
    constructor() {
        this._paramsValue = { matchString: 'Search' };
        this._queryParamsValue = { matchString: 'Search' };
    }
    // ActivatedRoute.params is Observable
    private subject = new BehaviorSubject(this.testParams);
    params = this.subject.asObservable();

    // Test parameters
    private _testParams: {};
    private _paramsValue: { matchString: string };
    private _queryParamsValue: { matchString: string };

    get testParams() { return this._testParams; }
    set testParams(params: {}) {
        this._testParams = params;
        this.subject.next(params);
    }
    // ActivatedRoute.snapshot.params
    get snapshot() {
        return { params: this._paramsValue, queryParams: this._queryParamsValue };
    }
    set snapshot(param: { params: { matchString: string }, queryParams: { matchString: string } }) {
        this._paramsValue = param.params;
        this._queryParamsValue = param.queryParams;
    }
}

describe('Testing of question-dashboard component:-', () => {
    let questionFixture: ComponentFixture<QuestionsDashboardComponent>;
    let questionComponent: QuestionsDashboardComponent;
    let mockData: any[] = [];
    let routeTo: any[] = [];
    let category1 = new Category();
    let category2 = new Category();
    let categoryList = new Array<Category>();
    let questionCount = new QuestionCount();
    let optionList1 = new Array<SingleMultipleAnswerQuestionOption>();
    let optionList2 = new Array<SingleMultipleAnswerQuestionOption>();
    questionCount.easyCount = 2;
    questionCount.mediumCount = 1;
    questionCount.hardCount = 0;

    //category list 
    category1.id = 1;
    category1.categoryName = 'Verbal';
    category2.id = 2;
    category2.categoryName = 'Quantitive Aptitude';

    //question answer options
    let questionOption1 = new SingleMultipleAnswerQuestionOption();
    questionOption1.id = 1;
    questionOption1.option = 'blue';
    questionOption1.isAnswer = true;
    questionOption1.singleMultipleAnswerQuestionId = 1001;
    let questionOption2 = new SingleMultipleAnswerQuestionOption();
    questionOption2.id = 2;
    questionOption2.option = 'green';
    questionOption2.isAnswer = false;
    questionOption2.singleMultipleAnswerQuestionId = 1001;
    optionList1.push(questionOption1);
    optionList1.push(questionOption2);

    let questionOption3 = new SingleMultipleAnswerQuestionOption();
    questionOption1.id = 3;
    questionOption1.option = '2';
    questionOption1.isAnswer = false;
    questionOption1.singleMultipleAnswerQuestionId = 1002;
    let questionOption4 = new SingleMultipleAnswerQuestionOption();
    questionOption2.id = 4;
    questionOption2.option = '4';
    questionOption2.isAnswer = true;
    questionOption2.singleMultipleAnswerQuestionId = 1002;
    let questionOption5 = new SingleMultipleAnswerQuestionOption();
    questionOption1.id = 5;
    questionOption1.option = '3';
    questionOption1.isAnswer = false;
    questionOption1.singleMultipleAnswerQuestionId = 1002;
    optionList2.push(questionOption3);
    optionList2.push(questionOption4);
    optionList2.push(questionOption5);

    //single multiple answer question created
    let singleMultipleAnswerQuestion1 = new SingleMultipleAnswerQuestion();
    singleMultipleAnswerQuestion1.id = 1001;
    singleMultipleAnswerQuestion1.singleMultipleAnswerQuestionOption = optionList1;

    let singleMultipleAnswerQuestion2 = new SingleMultipleAnswerQuestion();
    singleMultipleAnswerQuestion2.id = 1002;
    singleMultipleAnswerQuestion2.singleMultipleAnswerQuestionOption = optionList2;

    //question is created
    let question1 = new Question();
    question1.id = 1001;
    question1.questionDetail = 'what is your Favourite color?';
    question1.questionType = 0;
    question1.difficultyLevel = 1;
    question1.category = category1;
    question1.categoryID = category1.id;

    let question2 = new Question();
    question2.id = 1002;
    question2.questionDetail = 'how many colors are there in indian flag?';
    question2.questionType = 1;
    question2.difficultyLevel = 1;
    question2.category = category2;
    question2.categoryID = category2.id;

    //question base class created
    let questionBase1 = new QuestionBase();
    questionBase1.question = question1;
    questionBase1.singleMultipleAnswerQuestion = singleMultipleAnswerQuestion1;
    questionBase1.codeSnippetQuestion = null;

    let questionBase2 = new QuestionBase();
    questionBase2.question = question2;
    questionBase2.singleMultipleAnswerQuestion = singleMultipleAnswerQuestion2;
    questionBase2.codeSnippetQuestion = null;

    let questionBaseList1 = new Array<QuestionBase>();
    questionBaseList1.push(questionBase1);
    let questionBaseList2 = new Array<QuestionBase>();
    questionBaseList2.push(questionBase2);
    category2.questionList = questionBaseList2;

    //question display list createds
    let questionDisplay1 = new QuestionDisplay();
    questionDisplay1.id = 1001;
    questionDisplay1.questionType = QuestionType.singleAnswer;
    questionDisplay1.difficultyLevel = DifficultyLevel.Medium;
    questionDisplay1.questionDetail = 'what is your Favourite color?';
    questionDisplay1.category = category1;
    questionDisplay1.singleMultipleAnswerQuestion = singleMultipleAnswerQuestion1;

    let questionDisplay2 = new QuestionDisplay();
    questionDisplay2.id = 1002;
    questionDisplay2.questionType = QuestionType.multipleAnswer;
    questionDisplay2.difficultyLevel = DifficultyLevel.Medium;
    questionDisplay2.questionDetail = 'how many colors are there in indian flag?';
    questionDisplay2.category = category2;
    questionDisplay2.singleMultipleAnswerQuestion = singleMultipleAnswerQuestion2;

    let questionDisplayList = new Array<QuestionDisplay>();
    questionDisplayList.push(questionDisplay1);
    questionDisplayList.push(questionDisplay2);

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
        categoryList.push(category1);
        categoryList.push(category2);
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

    it('should return selected category and difficultylevel ', () => {
        categoryList.push(category1);
        categoryList.push(category2);
        spyOn(questionComponent, 'categoryWiseFilter');
        questionComponent.categoryArray = categoryList;
        questionComponent.selectedCategoryName = 'Verbal';
        questionComponent.SelectedDifficultyLevel = 'Medium';
        questionComponent.SelectCategoryDifficulty('Medium', 'Verbal');
        expect(questionComponent.selectedCategoryId).toBe(1);
        expect(questionComponent.categoryWiseFilter).toHaveBeenCalled();
    });

    it('should return all the categories', () => {
        spyOn(CategoryService.prototype, 'getAllCategories').and.callFake(() => {
            return Observable.of(categoryList);
        });
        spyOn(questionComponent, 'SelectCategoryDifficulty');
        questionComponent.selectedCategoryName = 'Verbal';
        questionComponent.SelectedDifficultyLevel = 'Medium';
        questionComponent.categoryArray = categoryList;
        questionComponent.getAllCategories();
        expect(questionComponent.isCategoryPresent).toBeTruthy();
        expect(questionComponent.SelectCategoryDifficulty).toHaveBeenCalledWith('Medium', 'Verbal');
        questionComponent.selectedCategoryName = undefined;
        questionComponent.getAllCategories();
        expect(questionComponent.SelectCategoryDifficulty).toHaveBeenCalledWith('Medium', 'AllCategory');
        questionComponent.SelectedDifficultyLevel = undefined;
        questionComponent.selectedCategoryName = 'Verbal';
        questionComponent.getAllCategories();
        expect(questionComponent.SelectCategoryDifficulty).toHaveBeenCalledWith('All', 'Verbal');
    });

    it('should return all the questions', () => {
        spyOn(QuestionsService.prototype, 'getQuestions').and.callFake(() => {
            return Observable.of(questionDisplayList);
        });
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.callFake(() => {
            return Observable.of();
        });
        questionComponent.getAllQuestions();
        expect(questionComponent.questionDisplay.length).toBe(2);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2]).toBe('questions/dashboard' + '/AllCategory' + '/All');
        questionComponent.matchString = 'Search';
        questionComponent.getAllQuestions();
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/dashboard' + '/AllCategory' + '/All' + '/Search');
    });

    it('should return the sorted category array', () => {
        questionComponent.categoryArray = categoryList;
        questionComponent.sortCategory();
        expect(questionComponent.categoryArray[0].categoryName).toBe('Quantitive Aptitude');
    });

    it('should filter questions as category wise', () => {
        let categoryName: string;
        let categoryId: number;
        let difficulty: string;
        categoryName = 'AllCategory';
        difficulty = 'All';
        spyOn(QuestionsService.prototype, 'getQuestions').and.callFake(() => {
            return Observable.of(questionDisplayList);
        });
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.callFake(() => {
            return Observable.of();
        });
        questionComponent.categoryWiseFilter(categoryId, categoryName, difficulty);
        expect(questionComponent.showName).toBe('All Questions');
        expect(questionComponent.questionDisplay.length).toBe(2);
        expect(questionComponent.selectedCategory.categoryName).toBe('AllCategory');
        categoryName = 'Verbal';
        questionComponent.categoryWiseFilter(categoryId, categoryName, difficulty);
        expect(questionComponent.isAllQuestionsSectionSelected).toBeFalsy();
    });

    it('should filter questions as difficulty wise', () => {
        let difficulty: string;
        difficulty = 'Easy';
        let list = new Array<QuestionDisplay>();
        questionComponent.selectedCategory.categoryName = undefined;
        spyOn(QuestionsService.prototype, 'getQuestions').and.callFake(() => {
            return Observable.of(list);
        });
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.callThrough();
        questionComponent.difficultyWiseSearch(difficulty);
        expect(questionComponent.questionDisplay.length).toBe(0);
        difficulty = 'Medium';
        questionComponent.selectedCategory.categoryName = 'Verbal';
        questionComponent.selectedCategoryName = 'Verbal';
        list.push(questionDisplay1);
        questionComponent.difficultyWiseSearch(difficulty);
        expect(questionComponent.isAllQuestionsSectionSelected).toBeFalsy();
    });

    it('should filter questions as searched', () => {
        let searchString = 'how';
        questionComponent.selectedCategoryName = undefined;
        let list = new Array<QuestionDisplay>();
        list.push(questionDisplay2);
        spyOn(QuestionsService.prototype, 'getQuestions').and.callFake(() => {
            return Observable.of(list);
        });
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.callFake(() => {
            return Observable.of();
        });
        questionComponent.getQuestionsMatchingSearchCriteria(searchString);
        expect(routeTo[0] + '/' + routeTo[1]).toBe('question/search' + '/how');
        questionComponent.selectedCategory.categoryName = undefined;
        questionComponent.selectedCategoryName = 'AllCategory';
        questionComponent.SelectedDifficultyLevel = 'Medium';
        questionComponent.getQuestionsMatchingSearchCriteria(searchString);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/dashboard' + '/AllCategory' + '/Medium' + '/how');
        questionComponent.selectedCategory.categoryName = 'Verbal';
        questionComponent.selectedCategoryName = 'Verbal';
        questionComponent.getQuestionsMatchingSearchCriteria(searchString);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3]).toBe('questions/dashboard' + '/Verbal' + '/Medium' + '/how');
        expect(questionComponent.questionDisplay.length).toBe(1);
        searchString = '';
        list.pop();
        questionComponent.selectedCategory.categoryName = 'AllCategory';
        questionComponent.difficultyLevel = 'Medium';
        questionComponent.getQuestionsMatchingSearchCriteria(searchString);
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2]).toBe('questions/dashboard' + '/AllCategory' + '/Medium');
        expect(questionComponent.questionDisplay.length).toBe(0);
        questionComponent.selectedCategory.categoryName = 'AllCategory';
        searchString = 'Suparna';
        questionComponent.getQuestionsMatchingSearchCriteria(searchString);
        expect(questionComponent.questionDisplay.length).toBe(0);
    });

    it('should return true if searching text length is not zero', () => {
        questionComponent.matchString = 'how';
        questionComponent.showStatus();
        expect(questionComponent.showSearchInput).toBeTruthy();
    });

    it('should return difficultywise total number of questions in a category', () => {
        spyOn(QuestionsService.prototype, 'countTheQuestion').and.callFake(() => {
            return Observable.of(questionCount);
        });
        questionComponent.categroyId = 2;
        questionComponent.matchString = '';
        questionComponent.countTheQuestion();
        expect(questionComponent.numberOfQuestions.easyCount).toBe(2);
        expect(questionComponent.numberOfQuestions.hardCount).toBe(0);
    });

    it('should select route as per question type', () => {
        questionComponent.selectedCategory.categoryName = 'Verbal';
        questionComponent.selectedDifficulty = 1;
        questionComponent.selectSelectionAndDifficultyType('single-answer');
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3] + '/' + routeTo[4]).toBe('questions/' + 'single-answer/' + 'add/' + category1.categoryName + '/Medium');
        questionComponent.selectSelectionAndDifficultyType('programming');
        expect(routeTo).toContain('programming');
        questionComponent.selectedCategory.categoryName = undefined;
        questionComponent.selectedDifficulty = 1;
        questionComponent.selectSelectionAndDifficultyType('multiple-answer');
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3] + '/' + routeTo[4]).toBe('questions/' + 'multiple-answer/' + 'add/' + 'AllCategory/' + 'Medium');
        questionComponent.selectedCategory.categoryName = 'Verbal';
        questionComponent.selectedDifficulty = 1;
        questionComponent.selectSelectionAndDifficultyType('programming');
        expect(routeTo[0] + '/' + routeTo[1] + '/' + routeTo[2] + '/' + routeTo[3] + '/' + routeTo[4]).toBe('questions/' + 'programming/' + 'add/' + 'Verbal/' + 'Medium');
    });

    it('should select the text area when search icon is clicked', () => {
        let event: any = {};
        event.stopPropagation = function () { };
        let search: any = {};
        search.select = function () { };
        let searchString = 'how';
        spyOn(event, 'stopPropagation');
        spyOn(search, 'select');
        questionComponent.selectedCategoryName = undefined;
        questionComponent.SelectedDifficultyLevel = undefined;
        questionComponent.selectTextArea(event, search, searchString);
        expect(routeTo[0] + '/' + routeTo[1]).toBe('question/search' + '/how');
    });

});
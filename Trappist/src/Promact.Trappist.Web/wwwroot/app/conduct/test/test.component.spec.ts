import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule, FormGroup } from '@angular/forms';
import { MaterialModule, MdDialogRef, OverlayRef, MdDialogModule, MdDialog, MdSnackBar, MdSnackBarRef } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule, ResponseOptions, ResponseType } from '@angular/http';
import { inject } from '@angular/core/testing';
import { TestQuestion } from '../../tests/tests.model';
import { testsRouting } from '../../tests/tests.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpService } from '../../core/http.service';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/from';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Component, OnInit, ChangeDetectorRef, ViewChild, AfterViewInit, Input, DebugElement } from '@angular/core';
import { Location, APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs/Rx';
import { Test } from '../../tests/tests.model';
import { TestPreviewComponent } from '../../tests/test-preview/test-preview.compponent';
import { TestQuestions } from '../test_conduct.model';
import { TestOrder } from '../../tests/enum-testorder';
import { SingleMultipleAnswerQuestionOption } from '../../questions/single-multiple-answer-question-option.model';
import { ConductService } from '../conduct.service';
import { QuestionStatus } from '../question_status.enum';
import { QuestionType } from '../../questions/enum-questiontype';
import { QuestionBase } from '../../questions/question';
import { TestAttendee } from '../test_attendee.model';
import { TestAnswer } from '../test_answer.model';
import { TestStatus } from '../teststatus.enum';
import { TestsProgrammingGuideDialogComponent } from './tests-programming-guide-dialog.component';
import { AceEditorComponent } from 'ng2-ace-editor';
import 'brace';
import 'brace/theme/cobalt';
import 'brace/theme/monokai';
import 'brace/theme/eclipse';
import 'brace/theme/solarized_light';
import 'brace/mode/java';
import 'brace/mode/c_cpp';
import { TestLogs } from '../../reports/testlogs.model';
import { AllowTestResume } from '../../tests/enum-allowtestresume';
import { CodeResponse } from '../code.response.model';
declare let screenfull: any;
declare let alea: any;
import { Subscription } from 'rxjs/Subscription';
import { TestService } from '../../tests/tests.service';
import { PageNotFoundComponent } from '../../page-not-found/page-not-found.component';
import { TestComponent } from './test.component';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { PopoverModule } from 'ngx-popover';
import { ClipboardModule } from 'ngx-clipboard';
import { Md2AccordionModule } from 'md2';
import { ChartsModule } from 'ng2-charts';
import {
    FakeTest, FakeAttendee, FakeTestQuestions,
    FakeTestLogs, FakeCodeResponse, FakeResumeData
} from '../../Mock_Data/conduct_data.mock';

class MockRouter {
    navigate() {
        return true;
    }

    isActive() {
        return true;
    }

    navigateByUrl(url: string) { return url; }
}

class MockWindow {
    location: {
        href: ''
    };
}

describe('Test Component', () => {

    let testComponent: TestComponent;
    let fixture: ComponentFixture<TestComponent>;

    let urls: any[];
    let router: Router;
    let module: NodeModule;

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
                entryComponents: [TestComponent, PageNotFoundComponent, TestPreviewComponent, TestsProgrammingGuideDialogComponent, AceEditorComponent]
            },

        });

        TestBed.configureTestingModule({

            declarations: [TestComponent, PageNotFoundComponent, TestPreviewComponent, TestsProgrammingGuideDialogComponent, AceEditorComponent],

            providers: [
                TestService,
                HttpService,
                ConductService,
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: window, useClass: MockWindow }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule.forRoot([]), HttpModule, BrowserAnimationsModule, PopoverModule, ClipboardModule, Md2AccordionModule.forRoot(), MdDialogModule, ChartsModule]
        }).compileComponents();        
    }));

    beforeEach(() => {        
        fixture = TestBed.createComponent(TestComponent);
        testComponent = fixture.componentInstance;

        spyOn(Window.prototype, 'addEventListener').and.callFake(() => { console.log('listener'); });
        spyOn(ConductService.prototype, 'getElapsedTime').and.callFake(() => {
            return Observable.of(4.5);
        });
        //spyOn(TestComponent.prototype, 'getClockInterval').and.callFake(() => {
        //    return;
        //});
    });

    afterEach(() => {
        fixture.destroy();
    });

    it('should get the elapsed time', () => {
        testComponent.getElapsedTime();
        expect(testComponent.isTestReady).toBe(true);
    });

    it('should resume test', () => {
        spyOn(ConductService.prototype, 'getAnswer').and.callFake(() => {
            return Observable.of(FakeResumeData);
        });
        spyOn(TestComponent.prototype, 'navigateToQuestionIndex').and.callFake(() => {
            return;
        });
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.resumeTest();
        expect(testComponent.isInitializing).toBe(false);
    });

    it('should NOT resume test', () => {
        spyOn(ConductService.prototype, 'getAnswer').and.callFake(() => {
            return Observable.throw(Error);
        });
        spyOn(TestComponent.prototype, 'navigateToQuestionIndex').and.callFake(() => {
            return;
        });
        spyOn(ConductService.prototype, 'setElapsedTime').and.callFake(() => {
            return Observable.of('OK');
        });

        testComponent.resumeTest();
        expect(testComponent.isInitializing).toBe(false);

        //Hack: Private function call
        testComponent['timeOut']();
        expect(ConductService.prototype.setElapsedTime).toHaveBeenCalled();
    });
    
    it('should get Test Attendee by Id', () => {
        spyOn(ConductService.prototype, 'getTestAttendeeByTestId').and.callFake(() => {
            return Observable.of(FakeAttendee);
        });
        spyOn(TestComponent.prototype, 'getTestQuestion').and.callFake(() => {
            return;
        });

        testComponent.getTestAttendee(2002, false);
        expect(testComponent.testAttendee.id).toBe(1);
    });

    it('should get Test Question', () => {
        spyOn(ConductService.prototype, 'getQuestions').and.callFake(() => {
            return Observable.of(FakeTestQuestions);
        });
        spyOn(ConductService.prototype, 'getTestByLink').and.callFake(() => {
            return Observable.of(FakeTest);
        });
        spyOn(TestComponent.prototype, 'getClockInterval').and.callFake(() => {
            return;
        });

        testComponent.test = JSON.parse(JSON.stringify(FakeTest));
        testComponent.testAttendee = JSON.parse(JSON.stringify(FakeAttendee));
        testComponent.getTestQuestion(2002);
        expect(testComponent.testQuestions.length).toBe(2);
    });

    it('should get Test status', () => {
        spyOn(ConductService.prototype, 'getTestStatus').and.callFake(() => {
            return Observable.of(0);
        });
        spyOn(TestComponent.prototype, 'resumeTest').and.callFake(() => {
            return Observable.of();
        });

        testComponent.getTestStatus(1);
        expect(testComponent.resumeTest).toHaveBeenCalled();
    });

    it('should run code', () => {
        spyOn(ConductService.prototype, 'execute').and.callFake(() => {
            return Observable.of(FakeCodeResponse);
        });
        spyOn(TestComponent.prototype, 'openDialog').and.callFake(() => {
            return Observable.of();
        });

        testComponent.questionIndex = 0;
        testComponent.testTypePreview = false;
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.runCode();
        expect(testComponent.codeResult).toBe('Success');
    });

    it('should add answer', () => {
        spyOn(ConductService.prototype, 'addAnswer').and.callFake(() => {
            return Observable.of('');
        });
        spyOn(TestComponent.prototype, 'markAsAnswered').and.callFake(() => {
            return;
        });


        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.testTypePreview = false;
        testComponent.addAnswer(testComponent.testQuestions[0]);
        expect(testComponent.isTestReady).toBe(true);

        testComponent.addAnswer(testComponent.testQuestions[1]);
        expect(testComponent.isTestReady).toBe(true);

        testComponent.testQuestions[1].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer = true;
        testComponent.addAnswer(testComponent.testQuestions[1]);
        expect(testComponent.isTestReady).toBe(true);
    });

    it('should mark question for review', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.markAsReview(0);
        expect(testComponent.testQuestions[0].questionStatus).toBe(QuestionStatus.review);
    });

    it('should mark question for review part 2', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.questionStatus = QuestionStatus.review;
        testComponent.markAsReview(0);
        expect(testComponent.testQuestions[0].questionStatus).toBe(QuestionStatus.selected);
    });

    it('should mark question for review part 3', () => {
        spyOn(ConductService.prototype, 'addAnswer').and.callFake(() => {
            return Observable.of('');
        });

        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.questionStatus = QuestionStatus.review;
        testComponent.addAnswer(testComponent.testQuestions[0]);
        testComponent.testAnswers[0].code.result = 'res';
        testComponent.markAsReview(0);
        expect(testComponent.testQuestions[0].questionStatus).toBe(QuestionStatus.selected);
    });

    it('should mark question as answered', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.markAsAnswered(0);
        expect(testComponent.testQuestions[0].questionStatus).toBe(QuestionStatus.answered);
    });

    it('should clear response', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.clearResponse(0);
        expect(testComponent.codeAnswer).toContain('public static void main');//Java
    });

    it('should select option (Single Option Type)', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.selectOption(1,0,true);
        expect(testComponent.testQuestions[1].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer).toBeTruthy();
    });

    it('should select option (Multiple Option Type)', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.selectOption(1, 0, false);
        expect(testComponent.testQuestions[1].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer).toBeTruthy();
    });

    it('should select option (Multiple Option Type) part 2', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.testQuestions[1].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer = true
        testComponent.selectOption(1, 0, false);
        expect(testComponent.testQuestions[1].question.singleMultipleAnswerQuestion.singleMultipleAnswerQuestionOption[0].isAnswer).toBeFalsy();
    });

    it('should navigate to other question', () => {
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.questionIndex = -1;
        testComponent.navigateToQuestionIndex(0);
        testComponent.navigateToQuestionIndex(1);
        expect(testComponent.isTestReady).toBeTruthy;
    });

    it('should get question status', () => {
        testComponent.isCodeProcessing = true;
        let ReturnedClass = testComponent.getQuestionStatus(QuestionStatus.answered);
        expect(ReturnedClass).toBe('answered cursor-not-allowed');
    });

    it('should run support functions', () => {
        spyOn(TestComponent.prototype, 'getClockInterval').and.callFake(() => {
            return;
        });

        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));

        testComponent.questionIndex = 1;
        let isLastQuestion = testComponent.isLastQuestion();
        expect(isLastQuestion).toBeTruthy();

        testComponent.ifOnline();
        expect(testComponent.isTestReady).toBeFalsy();

        testComponent.goOnline();
        expect(testComponent.getClockInterval).toHaveBeenCalled();

        testComponent.openDialog('congratulation', '');
        expect(testComponent.count).toBe(0);

        testComponent.openDialog('some', '');
        expect(testComponent.count).toBe(1);

        testComponent.openDialog(null, 'Err');
        testComponent.openDialog(null, 'Err');
        testComponent.openDialog(null, 'Err');
        testComponent.openDialog(null, 'Err');
        expect(testComponent.count).toBe(0);

        testComponent.openProgramGuide();

    });

    it('should get color code', () => {
        testComponent.codeResult = 'congratulation';
        let ret = testComponent.getColorCode();
        expect(ret).toBe('pass');

        testComponent.codeResult = 'some';
        ret = testComponent.getColorCode();
        expect(ret).toBe('partial-fail');

        testComponent.codeResult = 'processing';
        ret = testComponent.getColorCode();
        expect(ret).toBe('');

        testComponent.codeResult = 'anything';
        ret = testComponent.getColorCode();
        expect(ret).toBe('fail');
    });

    it('should increment focus lost counter', () => {
        spyOn(ConductService.prototype, 'setAttendeeBrowserToleranceValue').and.callFake(() => {
            return Observable.of(1);
        });
        spyOn(TestComponent.prototype, 'addAnswer').and.callFake(() => {
            return;
        });
        spyOn(ConductService.prototype, 'getTestAttendeeByTestId').and.callFake(() => {
            return Observable.of(FakeAttendee);
        });
        spyOn(ConductService.prototype, 'addTestLogs').and.callFake(() => {
            return Observable.of(FakeTestLogs);
        });
        spyOn(TestComponent.prototype, 'getTestQuestion').and.callFake(() => {
            return;
        });
        testComponent.test = JSON.parse(JSON.stringify(FakeTest));
        testComponent.questionIndex = 0;
        testComponent.questionStatus = QuestionStatus.review;
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));

        testComponent.getTestAttendee(2002, false);
        
        testComponent.windowFocusLost(null);
        testComponent.windowFocusLost(null);
        expect(testComponent.istestEnd).toBeTruthy();
    });

    it('should get Test setting by Test link', () => {        
        spyOn(ConductService.prototype, 'getTestByLink').and.callFake(() => {
            return Observable.of(FakeTest);
        });
        spyOn(TestComponent.prototype, 'getTestAttendee').and.callFake(() => {
            return;
        });
        
        testComponent.testTypePreview = false;
        testComponent.getTestByLink('');
        expect(testComponent.test.link).toBe('hjxJ4cQ2fI');        
    });

    it('should count down', () => {
        spyOn(ConductService.prototype, 'setElapsedTime').and.callFake(() => {
            return Observable.of('OK');
        });
        spyOn(ConductService.prototype, 'setAttendeeBrowserToleranceValue').and.callFake(() => {
            return Observable.of(1);
        });
        spyOn(TestComponent.prototype, 'addAnswer').and.callFake(() => {
            return;
        });
        spyOn(ConductService.prototype, 'addTestLogs').and.callFake(() => {
            return Observable.of(FakeTestLogs);
        }); 
        spyOn(ConductService.prototype, 'setTestStatus').and.callFake(() => {
            return Observable.of(1);
        });

        testComponent.test = JSON.parse(JSON.stringify(FakeTest));
        testComponent.questionIndex = 0;
        testComponent.questionStatus = QuestionStatus.answered;
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));

        //Hack: Calling private method 
        testComponent['countDown']();
        expect(ConductService.prototype.setAttendeeBrowserToleranceValue).toHaveBeenCalled();
    });

    it('should change editor language', () => {
        spyOn(ConductService.prototype, 'addAnswer').and.callFake(() => {
            return Observable.of('');
        });

        testComponent.questionIndex = 0;
        testComponent.testQuestions = JSON.parse(JSON.stringify(FakeTestQuestions));
        testComponent.addAnswer(testComponent.testQuestions[0]);

        fixture.whenStable().then(() => {
            testComponent.changeLanguage('c');
            expect(testComponent.editor._mode).toBe('c');
        });
    });
});
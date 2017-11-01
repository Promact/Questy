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
import { Component, OnInit, ChangeDetectorRef, ViewChild, AfterViewInit, Input } from '@angular/core';
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

    //let test = new Test();
    //test.id = 4;
    //test.numberOfTestAttendees = 2;
    //test.testName = 'History';
    //test.link = 'a6thsjk8';
    //test.duration = 10;
    //test.warningTime = 5;
    //test.startDate = '2017-10-16T06:51:49.4283026Z';
    //test.endDate = '2017-10-17T06:51:49.4283026Z';
    //test.correctMarks = '3';
    //test.incorrectMarks = '1';

    //let testLogs = new TestLogs();
    //testLogs.visitTestLink = new Date('Wed Oct 11 2017 06:53:13 GMT+0530 (India Standard Time)');
    //testLogs.fillRegistrationForm = new Date('Wed Oct 11 2017 06:53:13 GMT+0530 (India Standard Time)');
    //testLogs.startTest = new Date('Wed Oct 11 2017 06:53:15 GMT+0530 (India Standard Time)');
    //testLogs.finishTest = new Date('Tue Oct 17 2017 09:08:45 GMT+0530 (India Standard Time)');
    //testLogs.resumeTest = new Date('Wed Oct 11 2017 06:57:04 GMT+0530 (India Standard Time)');
    //testLogs.awayFromTestWindow = new Date('Wed Oct 11 2017 07:00:31 GMT+0530 (India Standard Time)');

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
        spyOn(Window.prototype, 'addEventListener').and.callFake(() => { console.log('listener');});
    });

    afterEach(() => {
        fixture.destroy();
    });

    it('should get the elapsed time', () => {        
        spyOn(ConductService.prototype, 'getElapsedTime').and.callFake(() => {
            return Observable.of(4.5);
        });
        testComponent.getElapsedTime();
        expect(testComponent.isTestReady).toBe(true);
    });

    //it('should.... Ummm..initialize', () => {
    //    //Activate spy network
    //    spyOn(ConductService.prototype, 'getTestByLink').and.callFake(() => {
    //        return Observable.of(FakeTest);
    //    });
    //    spyOn(ConductService.prototype, 'getTestAttendeeByTestId').and.callFake(() => {
    //        return Observable.of(FakeAttendee);
    //    });
    //    spyOn(ConductService.prototype, 'getQuestions').and.callFake(() => {
    //        return Observable.of(FakeTestQuestions);
    //    });
    //    spyOn(ConductService.prototype, 'getTestStatus').and.callFake(() => {
    //        return Observable.of(0);
    //    });
    //    spyOn(ConductService.prototype, 'getAnswer').and.callFake(() => {
    //        //Respond with error
    //        return Observable.throw(new Error('Test error'));
    //    });
    //    spyOn(ConductService.prototype, 'execute').and.callFake(() => {
    //        return Observable.of(FakeCodeResponse);
    //    });

    //    testComponent.ngOnInit();
    //    expect(testComponent.test.link).toBe('hjxJ4cQ2fI');
    //    expect(testComponent.testAttendee.id).toBe(1);
    //    expect(testComponent.testQuestions.length).toBe(2);

    //    //Navigation check
    //    testComponent.navigateToQuestionIndex(1);
    //    expect(testComponent.questionIndex).toBe(1);

    //    //Check code run
    //    testComponent.runCode();
    //    expect(testComponent.codeResult).toBe('Success');
    //});

    it('should resume test', () => {
        spyOn(ConductService.prototype, 'getAnswer').and.callFake(() => {
            return Observable.of(FakeResumeData);
        });
        spyOn(TestComponent.prototype, 'getElapsedTime').and.callFake(() => {
            return Observable.of();
        });
        spyOn(TestComponent.prototype, 'navigateToQuestionIndex').and.callFake(() => {
            return Observable.of();
        });
        testComponent.resumeTest();
        expect(testComponent.isInitializing).toBe(false);
    });

    it('should NOT resume test', () => {
        spyOn(ConductService.prototype, 'getAnswer').and.callFake(() => {
            return Observable.throw(Error);
        });
        spyOn(TestComponent.prototype, 'getElapsedTime').and.callFake(() => {
            return Observable.of();
        });
        spyOn(TestComponent.prototype, 'navigateToQuestionIndex').and.callFake(() => {
            return Observable.of();
        });
        testComponent.resumeTest();
        expect(testComponent.isInitializing).toBe(false);
    });
    
    it('should get Test Attendee by Id', () => {
        spyOn(ConductService.prototype, 'getTestAttendeeByTestId').and.callFake(() => {
            return Observable.of(FakeAttendee);
        });
        spyOn(TestComponent.prototype, 'getTestQuestion').and.callFake(() => {
            return Observable.of();
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
    });

    it('should get Test setting by Test link', () => {
        spyOn(ConductService.prototype, 'getTestByLink').and.callFake(() => {
            return Observable.of(FakeTest);
        });
        spyOn(TestComponent.prototype, 'getTestAttendee').and.callFake(() => {
            return Observable.of();
        });
        fixture.whenStable().then(() => {
            testComponent.testTypePreview = false;
            testComponent.getTestByLink('');
            expect(testComponent.test.link).toBe('hjxJ4cQ2fI');
        });        
    });   
});
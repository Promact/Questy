import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
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
import { Observable } from 'rxjs/Observable';
import { APP_BASE_HREF } from '@angular/common';
import { TestCreateDialogComponent } from './test-create-dialog.component';


class MockMdDialogRef {
    close(data: any) { return data; }
}

describe('Create Test', () => {
    let fixture: ComponentFixture<TestCreateDialogComponent>;
    let createDialog: TestCreateDialogComponent;
    let routTo: any[];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TestCreateDialogComponent],
            providers: [
                QuestionsService,
                { provide: ActivatedRoute, useValue: {} },
                { provide: MD_DIALOG_DATA },
                { provide: MdDialogRef, useClass: MockMdDialogRef },
                TestService,
                HttpService,
                MdSnackBar,
                { provide: APP_BASE_HREF, useValue: '/' }
            ],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        spyOn(TestService.prototype, 'IsTestNameUnique').and.returnValue(Observable.of(true));
        fixture = TestBed.createComponent(TestCreateDialogComponent);
        createDialog = fixture.componentInstance;
    });

    it('addTest', () => {
        spyOn(TestService.prototype, 'addTests').and.returnValue(Observable.of(MockTestData[0]));
        spyOn(createDialog.dialogRef, 'close').and.callThrough();
        spyOn(Router.prototype, 'navigate').and.callFake((route: any[]) => {
            routTo = route;
        });
        createDialog.addTest('Hello');
        expect(createDialog.dialogRef.close).toHaveBeenCalledTimes(1);
        expect(routTo[0]).toBe('tests/' + MockTestData[0].id + '/sections');
    });

    it('addTest error handling', () => {
        spyOn(TestService.prototype, 'addTests').and.returnValue(Observable.throw('error'));
        spyOn(MdSnackBar.prototype, 'open').and.callThrough();
        createDialog.addTest('Hello');
        expect(MdSnackBar.prototype.open).toHaveBeenCalled();
    });
});
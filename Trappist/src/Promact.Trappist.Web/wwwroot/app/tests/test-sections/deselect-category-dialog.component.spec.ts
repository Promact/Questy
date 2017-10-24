import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { MockTestData } from '../../Mock_Data/test_data.mock';
import { HttpService } from '../../core/http.service';
import { RouterModule, Router, ActivatedRoute, Routes, Params } from '@angular/router';
import { QuestionsService } from '../../questions/questions.service';
import { Http, HttpModule, XHRBackend, ResponseOptions } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestService } from '../tests.service';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import { DeselectCategoryComponent } from './deselect-category.component';
import { Observable } from 'rxjs/Observable';
import { APP_BASE_HREF } from '@angular/common';


class MockMdDialogRef {
    close(data: any) { return data; }
}


describe('Deselect category', () => {
    let fixture: ComponentFixture<DeselectCategoryComponent>;
    let deslectComponent: DeselectCategoryComponent;


    beforeEach(async (() => { 
    TestBed.configureTestingModule({
        declarations: [DeselectCategoryComponent],
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
        fixture = TestBed.createComponent(DeselectCategoryComponent);
        deslectComponent = fixture.componentInstance;
    });

    it('yesDeselectCategory', () => {
        spyOn(TestService.prototype, 'removeDeselectedCategory').and.returnValue(Observable.of(true));
        spyOn(deslectComponent.dialogRef, 'close').and.callThrough();
        deslectComponent.yesDeselectCategory();
        expect(deslectComponent.dialogRef.close).toHaveBeenCalledTimes(1);
    });

    it('yesDeselectCategory error check', () => {
        spyOn(TestService.prototype, 'removeDeselectedCategory').and.returnValue(Observable.throw('error'));
        spyOn(deslectComponent.snackbarRef, 'open').and.callThrough();
        deslectComponent.yesDeselectCategory();
        expect(deslectComponent.snackbarRef.open).toHaveBeenCalledTimes(1);
    });

});





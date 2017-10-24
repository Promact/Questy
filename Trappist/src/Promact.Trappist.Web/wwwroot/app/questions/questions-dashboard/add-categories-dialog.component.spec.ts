import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { async, fakeAsync } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule, XHRBackend, ResponseOptions } from '@angular/http';
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router';
import { MaterialModule, MdDialogModule, MdDialog, MdDialogRef, MdSnackBar, MD_DIALOG_DATA, OverlayRef } from '@angular/material';
import { QuestionsService } from '../questions.service';
import { HttpService } from '../../core/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddCategoryDialogComponent } from './add-category-dialog.component';
import { CategoryService } from '../categories.service';
import { Observable } from 'rxjs/Observable';
import { mockCategory } from '../../Mock_Data/test_data.mock';
import { Category } from '../category.model';

class MockMdDialogRef {
    close(data: any) { return data; }
}
class MockResponse {
    json(): Observable<any> {
        return Observable.of({'error':['Internal server error']});
    }
}

describe('Add-Category-Dialog', () => {

    let fixture: ComponentFixture<AddCategoryDialogComponent>;
    let addCategoryComponent: AddCategoryDialogComponent;
    let dialogRef: MdDialogRef<AddCategoryDialogComponent>;
    beforeEach(async(() => {

        TestBed.configureTestingModule({

            declarations: [AddCategoryDialogComponent],
            providers: [
                { provide: MdDialogRef, useClass: MockMdDialogRef },
                { provide: Response, useClass: MockResponse },
                CategoryService,
                HttpService
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, MdDialogModule]

        }).compileComponents();

    }));
    beforeEach(() => {
        fixture = TestBed.createComponent(AddCategoryDialogComponent);
        addCategoryComponent = fixture.componentInstance;
    });

    it('addCategory', () => {
        let category: any;
        category = mockCategory;
        spyOn(CategoryService.prototype, 'addCategory').and.returnValue(Observable.of(mockCategory));
        spyOn(MdSnackBar.prototype, 'open').and.callThrough();
        addCategoryComponent.addCategory(category);
        expect(addCategoryComponent.responseObject).toBe(category);
        expect(MdSnackBar.prototype.open).toHaveBeenCalled();
    });

    it('addCategory Error Handling', () => { 
        let category: any;
        category = mockCategory;
        spyOn(CategoryService.prototype, 'addCategory').and.returnValue(Observable.throw(new MockResponse()));
        addCategoryComponent.addCategory(category);
        expect(addCategoryComponent.isCategoryNameExist).toBe(true);
        expect(addCategoryComponent.isButtonClicked).toBe(false);
    });

});

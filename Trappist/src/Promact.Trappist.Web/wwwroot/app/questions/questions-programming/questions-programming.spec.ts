import { ComponentFixture, async, TestBed } from '@angular/core/testing';
import { QuestionsProgrammingComponent } from './questions-programming.component';
import { BrowserModule } from '@angular/platform-browser';
import { QuestionsService } from '../questions.service';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { CategoryService } from '../categories.service';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { HttpModule } from '@angular/http';
import { HttpService } from '../../core/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { APP_BASE_HREF } from '@angular/common';
import { Category } from '../category.model';
import { TinymceModule } from 'angular2-tinymce';
import { CKEditorModule } from 'ng2-ckeditor';
import { Md2AccordionModule } from 'md2';

describe('Testing of questions-programming component:-', () => {
    let programmingFixture: ComponentFixture<QuestionsProgrammingComponent>;
    let programmingComponent: QuestionsProgrammingComponent;
    let routeTo: any[] = [];
    let category1 = new Category();
    let category2 = new Category();
    let categoryList = new Array<Category>();

    category1.id = 1;
    category1.categoryName = 'Verbal';
    category2.id = 2;
    category2.categoryName = 'Quantitive Aptitude';
    categoryList.push(category1);
    categoryList.push(category2);

    beforeEach(async(() => {
        TestBed.configureTestingModule({

            declarations: [QuestionsProgrammingComponent],
            providers: [
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: ActivatedRoute },
                QuestionsService,
                CategoryService,
                HttpService
            ],
            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, TinymceModule, CKEditorModule, Md2AccordionModule.forRoot()]
        }).compileComponents();


    }));
    beforeEach(() => {
        spyOn(Router.prototype, 'navigate').and.callFake((route: any[]) => {
            routeTo = route;
        });
        programmingFixture = TestBed.createComponent(QuestionsProgrammingComponent);
        programmingComponent = programmingFixture.componentInstance;

    });

    it('should return pre-selected category and difficultylevel', () => {
        programmingComponent.categoryList = categoryList;
        programmingComponent.isCategorySelected = false;
        programmingComponent.selectedDifficulty = 'Easy';
        programmingComponent.showPreSelectedCategoryAndDifficultyLevel('AllCategory', 'All');
        expect(programmingComponent.isCategorySelected).toBeFalsy();
        expect(programmingComponent.selectedDifficulty).toBe('Easy');
        programmingComponent.selectedCategory = 'Verbal';
        programmingComponent.showPreSelectedCategoryAndDifficultyLevel('Verbal', 'Easy');       
        expect(programmingComponent.isCategorySelected).toBeTruthy();
        expect(programmingComponent.selectedDifficulty).toBe('Easy');
        programmingComponent.showPreSelectedCategoryAndDifficultyLevel('AllCategory', 'Hard');
        expect(programmingComponent.isCategorySelected).toBeFalsy();
        expect(programmingComponent.selectedDifficulty).toBe('Hard');
        programmingComponent.selectedCategory = 'Quantitive Aptitude';
        programmingComponent.showPreSelectedCategoryAndDifficultyLevel('Quantitive Aptitude', 'All');
        expect(programmingComponent.isCategorySelected).toBeTruthy();
        expect(programmingComponent.questionModel.question.categoryID).toBe(2);     
    });
});
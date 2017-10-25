//import { ComponentFixture, async, TestBed } from '@angular/core/testing';
//import { SingleMultipleAnswerQuestionComponent } from './questions-single-multiple-answer.component';
//import { CategoryService } from '../categories.service';
//import { QuestionsService } from '../questions.service';
//import { BrowserModule } from '@angular/platform-browser';
//import { Observable } from "rxjs/Observable";
//import { FormsModule } from "@angular/forms";
//import { RouterModule } from "@angular/router";
//import { MaterialModule, MdDialogModule } from "@angular/material";
//import { HttpModule } from "@angular/http";
//import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
//import { TinymceModule } from 'angular2-tinymce';

//describe('testing of Single multiple questions:-', () => {
//    let fixture: ComponentFixture<SingleMultipleAnswerQuestionComponent>;
//    let singleMultipleComponent: SingleMultipleAnswerQuestionComponent;

//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            declarations: [
//                SingleMultipleAnswerQuestionComponent
//            ],
//            providers: [
//                QuestionsService,
//                CategoryService
//            ],
//            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, HttpModule, BrowserAnimationsModule, TinymceModule]
//        }).compileComponents();
//    }));
//    beforeEach(() => {
//        fixture = TestBed.createComponent(SingleMultipleAnswerQuestionComponent);
//        singleMultipleComponent = fixture.componentInstance;      
//    });

//    it('Should return all the categories', () => {
//        spyOn(CategoryService.prototype, 'getAllCategories').and.callFake(() => {
//            return Observable.of();
//        });
//    });
//});
  
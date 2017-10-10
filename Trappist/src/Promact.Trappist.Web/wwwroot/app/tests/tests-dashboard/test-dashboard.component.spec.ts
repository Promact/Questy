import { TestsDashboardComponent } from './tests-dashboard.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { async } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import {
    BrowserDynamicTestingModule, platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { RouterModule, Router } from '@angular/router';
import { FilterPipe } from './test-dashboard.pipe';
import { QuestionsService } from "../../questions/questions.service";
import { Http, HttpModule } from "@angular/http";
import { TestService } from "../tests.service";
import { inject } from "@angular/core/testing";
import { Test } from "../tests.model";
import { testsRouting } from '../tests.routing';
import { TestServicesMock, MockQuestionService } from "../../Mock_Services/test-services.mock";



class RouterStub {
    navigateByUrl(url: string) { return url; }
}

describe('Test Dashboard Com[ponent', () => {
    let testDashboard: TestsDashboardComponent;
    let fixture: ComponentFixture<TestsDashboardComponent>;


    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TestsDashboardComponent, FilterPipe],
            providers: [
                { provide: QuestionsService, useClass: MockQuestionService },
                { provide: TestService, useClass: TestServicesMock },
                { provide: Router, useClass: RouterStub }
            ],
            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, HttpModule]
        }).compileComponents();
    }));
    beforeEach(() => {
        fixture = TestBed.createComponent(TestsDashboardComponent);
        testDashboard = fixture.componentInstance;
    });

    it('getTest', () => {
        testDashboard.getAllTests();
        expect(testDashboard.tests.length).toBe(2);
    });
})
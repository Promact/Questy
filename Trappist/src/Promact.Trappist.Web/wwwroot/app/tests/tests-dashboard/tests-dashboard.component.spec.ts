declare var describe, it, beforeEach, expect, spyOn;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog } from '@angular/material';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { DeleteTestDialogComponent } from './delete-test-dialog.component';
import { TestService } from '../tests.service';
import { Test } from '../tests.model';
import { TestSettingsComponent } from '../../tests/test-settings/test-settings.component';
import { TestSettingService } from '../testsetting.service';
import { TestsModule } from '../tests.module';
import { TestsDashboardComponent } from '../tests-dashboard/tests-dashboard.component';
import { ActivatedRoute, RouterModule, Routes, Router } from '@angular/router';
import { MockTestService } from '../../shared/mocks/tests/mock.tests.service';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

class MockMdDialog { }

describe('test for dashboard component', () => {
    const routes: Routes = [];  
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [TestsModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: TestService, useClass: MockTestService },
                { provide: MdDialog, useClass: MockMdDialog },
            ]
        }).compileComponents();

    }));

    it('should be defined TestsDashboardComponent', () => {
        let fixture = TestBed.createComponent(TestsDashboardComponent);
        let testsDashboardComponent = fixture.componentInstance;
        expect(testsDashboardComponent).toBeDefined();
    });

    it('should be defined TestsDashboardComponent', () => {
        let fixture = TestBed.createComponent(TestsDashboardComponent);
        let mocktest = new Test();
        mocktest.id = 1;
        mocktest.testName = 'test';
        let testsDashboardComponent = fixture.componentInstance;
        let testService = fixture.debugElement.injector.get(TestService);
        spyOn(testService, 'getTests').and.returnValue(new BehaviorSubject(mocktest).asObservable());
        testsDashboardComponent.getAllTests();
        expect(testsDashboardComponent.Tests).not.toBeNull();
    });
});
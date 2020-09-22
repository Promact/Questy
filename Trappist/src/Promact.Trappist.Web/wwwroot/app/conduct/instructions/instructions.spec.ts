
import {of as observableOf,  BehaviorSubject ,  Observable } from 'rxjs';
import { InstructionsComponent } from './instructions.component';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { ConductService } from '../conduct.service';
import { MaterialModule } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { TestInstructions } from '../testInstructions.model';
import { TestConductHeaderComponent } from '../shared/test-conduct-header/test-conduct-header.component';
import { TestConductFooterComponent } from '../shared/test-conduct-footer/test-conduct-footer.component';
import { CoreModule } from '../../core/core.module';

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

describe('Testing of conduct-instruction component:-', () => {
    let fixture: ComponentFixture<InstructionsComponent>;
    let componentInstruction: InstructionsComponent;
    let testLink = '1Pu48OQy6d';

    let testInstructions = new TestInstructions();
    testInstructions.duration = 5;
    testInstructions.browserTolerance = 7;
    testInstructions.correctMarks = 3;
    testInstructions.incorrectMarks = 1;
    testInstructions.totalNumberOfQuestions = 10;
    testInstructions.categoryNameList = ['Computer', 'Aptitude', 'Verbal'];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                InstructionsComponent,
                TestConductHeaderComponent,
                TestConductFooterComponent],

            providers: [
                ConductService,
                { provide: Router, useClass: RouterStub },
                { provide: ActivatedRoute, useclass: ActivatedRoute }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(InstructionsComponent);
        componentInstruction = fixture.componentInstance;
    });


    it('should return all instructions by test link', () => {
        spyOn(ConductService.prototype, 'getTestInstructionsByLink').and.callFake(() => {
            return observableOf(testInstructions);
        });
        componentInstruction.getTestInstructionsByLink(testLink);
        expect(componentInstruction.testInstructions.duration).toBe(5);
        expect(componentInstruction.testInstructions.categoryNameList).toContain('Aptitude');
    });

    it('should display negativeSign if incorrect marks is not zero', () => {
        spyOn(ConductService.prototype, 'getTestInstructionsByLink').and.callFake(() => {
            return observableOf(testInstructions);
        });
        componentInstruction.getTestInstructionsByLink(testLink);
        expect(componentInstruction.negativeSign).toBe('-');
    });

    it('should not display negativeSign if incorrect marks is zero', () => {
        testInstructions.incorrectMarks = 0;
        spyOn(ConductService.prototype, 'getTestInstructionsByLink').and.callFake(() => {
            return observableOf(testInstructions);
        });
        componentInstruction.getTestInstructionsByLink(testLink);
        expect(componentInstruction.negativeSign).toBe(undefined);
    });

});
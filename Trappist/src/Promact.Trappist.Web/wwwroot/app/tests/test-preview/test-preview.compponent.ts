import { Component, ViewChild} from '@angular/core';
import { TestComponent } from '../../conduct/test/test.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TestService } from "../tests.service";

@Component({
    moduleId: module.id,
    selector: 'test-preview',
    templateUrl: 'test-preview.html'
})

export class TestPreviewComponent {
    @ViewChild(TestComponent) testPreview: TestComponent;
    testLink: string;

    constructor(private route: ActivatedRoute, private _router: Router, private testService: TestService) {
        this.testService.isTestPreviewIsCalled.next(true);
        this.testLink = this.route.snapshot.params['link'];        
    }
}
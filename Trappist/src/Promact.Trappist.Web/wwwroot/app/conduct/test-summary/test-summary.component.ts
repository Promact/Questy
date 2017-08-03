import { Component, OnInit } from '@angular/core';
import { ConductService } from '../conduct.service';
import { ActivatedRoute,Router } from '@angular/router';
import { TestSummary } from '../testsummary.model';

@Component({
    moduleId: module.id,
    selector: 'test-summary',
    templateUrl: 'test-summary.html',
})
export class TestSummaryComponent implements OnInit{
    magicString: string;
    testSummaryObject : TestSummary;

    constructor(private conductService: ConductService, private route: ActivatedRoute, private router: Router) {
        this.testSummaryObject = new TestSummary();
    }

    ngOnInit() {
        let url = window.location.pathname;
        this.magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/test-summary'));
        this.getTestSummaryDetailsByLink(this.magicString);
    }

    getTestSummaryDetailsByLink(testLink: string) {
        this.conductService.getTestSummary(testLink).subscribe((response) => {
            this.testSummaryObject = response;
        });
    }
}

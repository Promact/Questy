import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService } from '../report.service';
import { TestAttendee } from '../testattendee.model';

@Component({
    moduleId: module.id,
    selector: 'test-report',
    templateUrl: 'test-report.html'
})

export class TestReportComponent {
    testId: number;
    showSearchInput: boolean;
    testAttendeeArray: TestAttendee[] = new Array<TestAttendee>();
    constructor(private route: ActivatedRoute, private reportsService: ReportService, private router: Router) {
        this.testId = this.route.snapshot.params['id'];
        console.log(this.testId);
        this.testReport();

    }

    testReport() {
        this.reportsService.getAllTestAttendees(this.testId).subscribe((response) => {
            console.log(response);
            this.testAttendeeArray = response;
            console.log(this.testAttendeeArray);
        });
    }

    /**
     * Data for Report Table
     */
    reportData: Array<Object> = [
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 01 2017', score: 125, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 02 2017', score: 135, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 03 2017', score: 126, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 01 2018', score: 127, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 04 2017', score: 128, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 05 2017', score: 129, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 06 2017', score: 145, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 07 2017', score: 155, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 08 2017', score: 165, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 09 2017', score: 175, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 10 2017', score: 185, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 11 2017', score: 195, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 12 2017', score: 125, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 13 2017', score: 125, percentage: '62 %' },
        { name: 'John Doe', email: 'joh.doe@gmail.com', date: 'Sun Jan 14 2017', score: 125, percentage: '62 %' }
    ];

    viewIndividualReport(testAttendee: TestAttendee) {
        this.router.navigate(['/reports/' + testAttendee.id + '/individual-report']);
    }
}

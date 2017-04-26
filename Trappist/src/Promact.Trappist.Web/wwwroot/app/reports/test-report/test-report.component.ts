import { Component, OnInit } from '@angular/core';
import { ReportService } from '../report.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TestAttendee } from '../testAttendee'; 

@Component({
    moduleId: module.id,
    selector: 'test-report',
    templateUrl: 'test-report.html'
})

export class TestReportComponent implements OnInit {
    showSearchInput: boolean;
    testAttendeeArray: TestAttendee[];
    testId: number;
    constructor(private reportService: ReportService, private route: ActivatedRoute) {
        this.testAttendeeArray = new Array<TestAttendee>();
    }
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getAllTestAttendees();
    }
    getAllTestAttendees() {
        this.reportService.getAllTestAttendees(this.testId).subscribe((attendeeList) => {
            this.testAttendeeArray = attendeeList
        });
    }
}


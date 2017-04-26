import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestReportComponent } from '../reports/test-report/test-report.component';

@Injectable()
export class ReportService {
    private reportsApiUrl = 'api/report';
    constructor(private httpService: HttpService) {
    }
    getAllTestAttendees(id: number) {
        return this.httpService.get(this.reportsApiUrl+'/multiple'+'/'+id);
    }
} 
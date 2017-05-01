import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestReportComponent } from '../reports/test-report/test-report.component';

@Injectable()
export class ReportService {
    private reportsApiUrl = 'api/report';
    constructor(private httpService: HttpService) {
    }
    getAllTestAttendees(testId: number) {
        return this.httpService.get(this.reportsApiUrl+'/multiple'+'/'+testId);
    }
    setStarredCandidate(attendeeId: number) {
        return this.httpService.post(this.reportsApiUrl+'/star'+'/'+attendeeId,attendeeId);
    }
    setAllCandidatesStarred(testId: number, status: boolean, idList: number[]) {
        return this.httpService.put(this.reportsApiUrl + '/star/all/' + testId + '/' + status, idList);
    }
} 
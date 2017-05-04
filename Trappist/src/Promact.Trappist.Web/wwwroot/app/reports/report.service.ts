import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestReportComponent } from '../reports/test-report/test-report.component';

@Injectable()
export class ReportService {
    private reportsApiUrl = 'api/report';

    constructor(private httpService: HttpService) {
    }

    getTestName(testId: number) {
        return this.httpService.get(this.reportsApiUrl+'/testName' + '/' + testId);
    }

    getAllTestAttendees(testId: number) {
        return this.httpService.get(this.reportsApiUrl+'/'+testId);
    }

    setStarredCandidate(attendeeId: number) {
        return this.httpService.post(this.reportsApiUrl+'/star'+'/'+attendeeId,attendeeId);
    }

    setAllCandidatesStarred(status: boolean, searchString: string, selectedTestStatus: number) {
        return this.httpService.put(this.reportsApiUrl + '/star/all/' +selectedTestStatus+'/' +searchString,status);
    }
} 
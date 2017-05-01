import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()

export class ReportService {
    private reportApi = 'api/reports';
    constructor(private httpService: HttpService) { }

    getAllTestAttendees(id: number) {
        return this.httpService.get(this.reportApi + '/' + id);
    }

    getTestAttendeeById(id: number) {
        return this.httpService.get(this.reportApi + '/' + id + '/testAttendee');
    }
    getTestQuestions(testId: number) {
        return this.httpService.get(this.reportApi + '/' + testId + '/testQuestions');
    }
}
import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()

export class ReportService {
    private reportApi = 'api/reports';

    constructor(private httpService: HttpService) { }

    /**
     * Gets all the test attendees present in a test
     * @param id: Id of the test whose details are to be fetched
     */
    getAllTestAttendees(id: number) {
        return this.httpService.get(this.reportApi + '/' + id);
    }

    /**
     * Gets test attendee details from the server
     * @param id: Id of the test attendee
     */
    getTestAttendeeById(id: number) {
        return this.httpService.get(this.reportApi + '/' + id + '/testAttendee');
    }

    /**
     * Gets all the questions present in a test from the server
     * @param testId: Id of the test qhos questions are to be fetched
     */
    getTestQuestions(testId: number) {
        return this.httpService.get(this.reportApi + '/' + testId + '/testQuestions');
    }

    /**
     * Gets all the answers given by the test attendee
     * @param id: Id of the test attendee
     */
    getTestAttendeeAnswers(id: number) {
        return this.httpService.get(this.reportApi + '/' + id + '/testAnswers');
    }
}
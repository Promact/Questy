import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestReportComponent } from '../reports/test-report/test-report.component';
import { TestAttendee } from './testAttendee';

@Injectable()
export class ReportService {
    private reportsApiUrl = '/api/report';

    constructor(private httpService: HttpService) {
    }

    getTestName(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/testName' + '/' + testId);
    }

    getAllTestAttendees(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testId);
    }

    setStarredCandidate(attendeeId: number) {
        return this.httpService.post(this.reportsApiUrl + '/star' + '/' + attendeeId, attendeeId);
    }

    setAllCandidatesStarred(status: boolean, searchString: string, selectedTestStatus: number) {
        return this.httpService.put(this.reportsApiUrl + '/star/all/' + selectedTestStatus + '?searchString=' + searchString, status);
    }

    /**
     * Gets test attendee details from the server
     * @param id: Id of the test attendee
     */
    getTestAttendeeById(id: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + id + '/testAttendee');
    }

    /**
     * Gets all the questions present in a test from the server
     * @param testId: Id of the test qhos questions are to be fetched
     */
    getTestQuestions(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testId + '/testQuestions');
    }

    /**
     * Gets all the answers given by the test attendee
     * @param id: Id of the test attendee
     */
    getTestAttendeeAnswers(id: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + id + '/testAnswers');
    }

    /**
     * Gets the percentile of the selected test attendee
     * @param testAttendeeId: Id of the test attendee
     */
    getStudentPercentile(testAttendeeId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testAttendeeId + '/percentile');
    }

    getAllAttendeeMarksDetails(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testId + '/allAttendeeMarksDeatils');
    }

    /**
     * Gets the details of code snippet question test cases
     * @param attendeeId: Id of the test attendee
     * @param questionId: Id of the code snippet question of a particular test
     */
    getCodeSnippetQuestionTestCasesDetails(attendeeId: number, questionId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + attendeeId + '/' + questionId + '/codeSnippetTestCasesDetails');
    }

    /**
     * Gets the total marks scored by the test attendee in code snippet question attempted
     * @param attendeeId: Id of the test attendee
     * @param questionId: Id of the code snippet question of a particular test
     */
    getCodeSnippetQuestionMarks(attendeeId: number, questionId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + attendeeId + '/' + questionId + '/scoreOfCodeSnippetQuestion');
    }

    /**
     * Gets the details of test code solution for test cases of the code snippet question attempted by the test attendee
     * @param attendeeId: Id of the test attendee
     * @param questionId: Id of the code snippet question of a particular test
     */
    getTestCodeSolutionDetails(attendeeId: number, questionId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + attendeeId + '/' + questionId + '/testCodeSolutionDetails');
    }

    createSessionForAttendee(attendee: any, testLink: string, isTestEnd: boolean) {
        return this.httpService.post(this.reportsApiUrl + '/createSession/' + testLink + '/' + isTestEnd, attendee);
    }

    updateCandidateInfo(attendeeId: number, isTestResume: boolean) {
        return this.httpService.get(this.reportsApiUrl + '/' + attendeeId + '/' + isTestResume + '/sendRequest');
    }
    getInfoResumeTest(attendeeId: number) {
        return this.httpService.get(this.reportsApiUrl + '/getWindowClose/' + attendeeId);
    }
} 
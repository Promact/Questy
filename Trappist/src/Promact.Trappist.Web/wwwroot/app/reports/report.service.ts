import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestReportComponent } from '../reports/test-report/test-report.component';
import { TestAttendee } from './testAttendee';

@Injectable()
export class ReportService {
    private reportsApiUrl = '/api/report';

    constructor(private httpService: HttpService) {
    }

    /**
     * Gets the test-name with test-link from the server
     * @param testId: Id of the test
     */
    getTestName(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/testName' + '/' + testId);
    }

    /**
     * Gets all attendees of a test from the server
     * @param testId: Id of the test
     */
    getAllTestAttendees(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testId);
    }

    /**
     * Set an attendee as a starred attendee and also make unstarred as per choice
     * @param attendeeId: Id of an attendee
     */
    setStarredCandidate(attendeeId: number) {
        return this.httpService.post(this.reportsApiUrl + '/star' + '/' + attendeeId, attendeeId);
    }

    /**
     * Sets all attendee of a test as Starred attendees and also make unstarred as per choice
     * @param status: status of the star field 
     * @param searchString: Text that needs to be searched
     * @param selectedTestStatus: Filter status
     */
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
     * @param testId: Id of the test taken by the test attendee
     */
    getStudentPercentile(testAttendeeId: number, testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testAttendeeId + '/' + testId + '/percentile');
    }

    /**
     * Gets all the attendee details needed for generating excel sheet from server 
     * @param testId: testId Id of the test
     */
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

    /**
     * Gets the total number of questions attempted by a test attendee
     * @param attendeeId: Contains the id of the test attendee from the route
     */
    getTotalNumberOfAttemptedQuestions(attendeeId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + attendeeId + '/attemptedQuestions');
    }

    generateReport(attendeeIdList: number[]) {
        return this.httpService.post(this.reportsApiUrl + '/generateReport', attendeeIdList);
    }

    getAttendeeIdList(testId: number) {
        return this.httpService.get(this.reportsApiUrl + '/' + testId + '/getAttendeeIdList');
    }
} 
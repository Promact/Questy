import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestAnswer } from './test_answer.model';
import { TestStatus } from './teststatus.enum';

@Injectable()
export class ConductService {
    private testConductUrl: string = '/api/conduct/';
    private testApiUrl = '/api/tests';
    

    constructor(private httpService: HttpService) {
    }

    /**
     * This method used for register test attendee.
     * @param magicString-It will contain test link
     * @param testAttendee-This model object contain test attendee credential which are first name, last name, email, roll number, contact number.
     */
    registerTestAttendee(magicString: any, testAttendee: any) {
        return this.httpService.post(this.testConductUrl + magicString + '/register', testAttendee);
    }

    /**
     * Gets all the instruction details before starting of a particular test
     * @param testLink is used to fetch all instructions related to a particular test
     */
    getTestInstructionsByLink(testLink: string) {
        return this.httpService.get(this.testConductUrl + testLink + '/instructions');
    }

    /**
     *Get list of Questions
     */
    getQuestions(id: number) {
        return this.httpService.get(this.testConductUrl + 'testquestion/' + id);
    }

    /**
     * Gets the details of a particular test withs all categories it contains
     * @param id is passed to identify that particular "Test"
     */
    getTestByLink(link: string, testTypePreview: boolean) {
        return this.httpService.get(this.testConductUrl + 'testbylink/' + link + '/' + testTypePreview);
    }

    /**
     * Gets Test Attendee by Test Id
     * @param testId: Id of Test
     */
    getTestAttendeeByTestId(testId: number, isTestTypePreview: boolean) {
        return this.httpService.get(this.testConductUrl + 'attendee/' + testId + '/' + isTestTypePreview);
    }

    /**
     * Call API to add answers to the Database
     * @param attendeeId: Id of Attendee
     */
    addAnswer(attendeeId: number, testAnswer: TestAnswer) {
        return this.httpService.put(this.testConductUrl + 'answer/' + attendeeId, testAnswer);
    }

    /**
     * Gets the Attendee answers
     * @param attendeeId : Id of Attendee
     */
    getAnswer(attendeeId: number) {
        return this.httpService.get(this.testConductUrl + 'answer/' + attendeeId);
    }

    /**
     * Sets the time elapsed from the start of Test
     * @param attendeeId : Id of Attendee
     */
    setElapsedTime(attendeeId: number) {
        return this.httpService.put(this.testConductUrl + 'elapsetime/' + attendeeId, null);
    }

    /**
     * Gets the time elapsed from the start of Test
     * @param attendeeId : Id of Attendee
     */
    getElapsedTime(attendeeId: number) {
        return this.httpService.get(this.testConductUrl + 'elapsetime/' + attendeeId);
    }

    /**
     * Sets the TestStatus of the Attendee 
     * @param attendeeId: Id of Attendee
     * @param testStatus: TestStatus object
     */
    setTestStatus(attendeeId: number, testStatus: TestStatus) {
        return this.httpService.put(this.testConductUrl + 'teststatus/' + attendeeId, testStatus);
    }

    /**
     * Sets the TestStatus of the Attendee 
     * @param attendeeId: Id of Attendee
     * @param testStatus: TestStatus object
     */
    getTestStatus(attendeeId: number) {
        return this.httpService.get(this.testConductUrl + 'teststatus/' + attendeeId);
    }

    /**
     * Sets the values of certain fields of Test Logs Model
     * @param attendeeId is obtained from the route
     * @param body is the object of test logs model
     */
    addTestLogs(attendeeId: number, body: any) {
        return this.httpService.put(this.testConductUrl + 'testlogs/' + attendeeId, body);
    }

    /**
     * Gets the total number of questions of a Test 
     * @param testLink is obtained from the route
     */
    getTestSummary(testLink: string) {
        return this.httpService.get(this.testConductUrl + testLink + '/test-summary');
    }

    execute(attendeeId: number, testAnswer: TestAnswer) {
        return this.httpService.post(this.testConductUrl + 'code/' + attendeeId, testAnswer);
    }
}
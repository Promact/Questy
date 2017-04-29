import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { TestAnswer } from './test_answer.model';

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
        return this.httpService.get(this.testConductUrl + '/testquestion/' + id);
    }

    /**
     * Gets the details of a particular test withs all categories it contains
     * @param id is passed to identify that particular "Test"
     */
    getTestByLink(link: string) {
        return this.httpService.get(this.testConductUrl + '/testbylink/' + link);
    }

    /**
     * Gets Test Attendee by Test Id
     * @param testId: Id of Test
     */
    getTestAttendeeByTestId(testId: number) {
        return this.httpService.get(this.testConductUrl + 'attendee/' + testId);
    }

    /**
     * Call API to add answers to the Database
     * @param attendeeId: Id of Attendee
     */
    addAnswer(attendeeId: number, testAnswers: TestAnswer[]) {
        return this.httpService.put(this.testConductUrl + 'answer/' + attendeeId, testAnswers);
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
}
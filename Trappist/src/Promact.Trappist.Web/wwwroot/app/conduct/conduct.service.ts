import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()
export class ConductService {
    private registerTestAttendeeApiUrl: string = '/api/testconduct/';

    constructor(private httpService: HttpService) {
    }

    /**
     * This method used for register test attendee.
     * @param magicString-It will contain test link
     * @param testAttendee-This model object contain test attendee credential which are first name, last name, email, roll number, contact number.
     */
    registerTestAttendee(magicString: any, testAttendee: any) {
        return this.httpService.post(this.registerTestAttendeeApiUrl + magicString + '/register', testAttendee);
    }

    /**
     * Gets all the instruction details before starting of a particular test
     * @param testLink is used to fetch all instructions related to a particular test
     */
    getTestInstructionsByLink(testLink: string) {
        return this.httpService.get(this.registerTestAttendeeApiUrl + testLink + '/instructions');
    }
}
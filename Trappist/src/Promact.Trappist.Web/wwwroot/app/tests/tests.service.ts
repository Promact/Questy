import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Test } from './tests.model';
@Injectable()

export class TestService {
    private testApiUrl = 'api/tests';
    private testNameApiUrl = 'api/tests/isUnique';
    constructor(private httpService: HttpService) {
    }

    /**
     * get list of tests
     */
    getTests() {
        return this.httpService.get(this.testApiUrl);
    }

    /**
     * add new test
     * @param url
     * @param test
     */
    addTests(test: any) {
        return this.httpService.post(this.testApiUrl, test);
    }

    /**
     * get response whether test name is unique or not
     * @param testName is name of the test
     */
    IsTestNameUnique(testName: string, id: number) {
        return this.httpService.get(this.testNameApiUrl + '/' + testName + '/' + id);
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id is used to get the Settings of a Test by its Id
     */
    getTestById(id: number) {
        return this.httpService.get(this.testApiUrl + '/' + id + '/' + 'settings');
    }

    /**
     * Updates the changes made to the Settings of a Test
     * @param id is used to access the Settings of that Test
     * @param body is used as an object for the Model Test
     */
    updateTestById(id: number, body: Test) {
        return this.httpService.put(this.testApiUrl + '/' + id + '/' + 'settings', body);
    }

    /**
     * Updates the edited Test Name
     * @param id is used to access the Name of that Test
     * @param body is used as an object for the Model Test
     */
    updateTestName(id: number, body: Test) {
        return this.httpService.put(this.testApiUrl + '/' + id, body);
    }

    /**
     * Delete the selected test
     * @param testId: type number and has the id of the test to be deleted
     */
    deleteTest(testId: number) {
        return this.httpService.delete(this.testApiUrl + '/' + testId);
    }

    /**
     * Checks whether any test attendee exists 
     * @param testId: type number and has the id of the test to be deleted
     */
    isTestAttendeeExist(testId: number) {
        return this.httpService.get(this.testApiUrl + '/' + testId +'/testAttendee');
    }
}
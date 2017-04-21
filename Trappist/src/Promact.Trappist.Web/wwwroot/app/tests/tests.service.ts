import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Test } from './tests.model';
import { TestDetails } from './test';
import { QuestionBase } from '../questions/question';
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


    /**
     * Gets the questions of a particular category in a "Test"
     * @param testId is passed to identify that particular "Test"
     * @param categoryId is passed to identify that particular "category"
     */
    getQuestions(testId: number, categoryId: number) {
        return this.httpService.get(this.testApiUrl + '/questions/' + testId + '/' + categoryId);
    }

    /**
     * Adds the selected questions to the "Test"
     * @param selectedQuestions is a list of questions user wants to add to the test
     * @param testId is passed to identify that particular "Test"
     */
    addTestQuestions(selectedQuestions: QuestionBase[], testId: number) {
        return this.httpService.post(this.testApiUrl + '/questions/' + testId, selectedQuestions);
    }
    /**
     * Gets the details of a particular test withs all categories it contains
     * @param id is passed to identify that particular "Test"
     */
    getTestDetails(id: number) {
        return this.httpService.get(this.testApiUrl + '/' + id);
    }
}
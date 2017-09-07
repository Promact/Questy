import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { Test } from './tests.model';
import { QuestionBase } from '../questions/question';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from 'rxjs/Subject';

@Injectable()

export class TestService {
    private testApiUrl = 'api/tests';
    private testNameApiUrl = 'api/tests/isUnique';
    public isTestPreviewIsCalled = new Subject<any>();
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
     * Updates the changes made to the Settings of a Test
     * @param id is used to access the Settings of that Test
     * @param body is used as an object for the Model Test
     */
    updateTestById(id: number, body: Test) {
        return this.httpService.put(this.testApiUrl + '/' + id + '/' + 'settings', body);
    }

    /**
     * Updates the status of test ie. pause or resume
     * @param id
     * @param isPause
     */
    updateTestPauseResume(id: number, isPause: boolean) {
        return this.httpService.get(this.testApiUrl + '/isPausedResume/' + id + '/' + isPause);
    }
    /**
     * Deletes the ip address of a test
     * @param id
     */
    deleteTestipAddress(id: number) {
        return this.httpService.delete(this.testApiUrl + '/deleteIp/' + id);
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
        return this.httpService.get(this.testApiUrl + '/' + testId + '/testAttendee');
    }

    

    addTestCategories(testId: number, testCategory: any ) {
        return this.httpService.post(this.testApiUrl + '/' + 'addTestCategories/' +testId, testCategory);
    }

    /**
     * deletes the deselected category from TestCategory
     * @param testCategory
     */
    removeDeselectedCategory(testCategory: any) {
        return this.httpService.post(this.testApiUrl + '/' + 'deselectCategory', testCategory);
    }

    /**
     * deselects the category
     * @param categoryId
     * @param testId
     */
    deselectCategory(categoryId: number, testId: number) {
        return this.httpService.get(this.testApiUrl + '/' + 'deselectCategory' + '/' + categoryId + '/' + testId);
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
    getTestById(id: number) {
        return this.httpService.get(this.testApiUrl + '/' + id);
    }

    /**
     * Duplicates the slected test
     * @param testId: Id of the test that is to be duplicated
     * @param newTestId: Id of the duplicated Test
     */
    duplicateTest(testId: number, test:Test) {
        return this.httpService.post(this.testApiUrl + '/' + testId +'/duplicateTest', test);
    }

    /**
     * Sets the number of times the test has been updated
     * @param testId: Id of the test that is duplicated
     * @param testName: name of the test that is duplicated
     */
    setTestCopiedNumber(testId: number, testName: string) {
        return this.httpService.get(this.testApiUrl + '/' + testId + '/' + testName + '/setTestCopiedNumber');
    }
}
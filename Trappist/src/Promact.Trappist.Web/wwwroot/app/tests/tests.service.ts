﻿import { Injectable } from '@angular/core';
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
    getTestSettings(id: number) {
        return this.httpService.get(this.testApiUrl + '/' + id);
    }

    /**
     * Updates the changes made to the Settings of a Test
     * @param id is used to access the Settings of that Test
     * @param body is used as an object for the Model Test
     */
    updateTestSettings(id: number, body: Test) {
        return this.httpService.put(this.testApiUrl + '/' + id, body);
    }
}
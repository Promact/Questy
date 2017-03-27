import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()

export class TestService {

    private testsApiUrl = 'api/tests';

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
    addTests(url: string, test: any) {
        return this.httpService.post(url, test);
    }
    /**
     * get response whether test name is unique or not
     * @param testName is name of the test
     */
    getTest(testName: string) {
        return this.httpService.get(this.testApiUrl + '/' + testName);
    }
}

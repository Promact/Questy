import { Injectable } from '@angular/core';
import { Test } from "../../../tests/tests.model";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class MockTestService {
    tests: Array<Test> = new Array<Test>();
    constructor() {
        let mockTest = new Test();
        this.tests.push(mockTest);
    }
    getTests() {
        return new BehaviorSubject(this.tests).asObservable();
    }
}

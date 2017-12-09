import { Test } from '../tests/tests.model';
import { TestAttendee } from './test_attendee.model';
import { TestQuestions } from './test_conduct.model';

export class TestBundleModel {
    test: Test;
    testAttendee: TestAttendee;
    testQuestions: TestQuestions[];
}
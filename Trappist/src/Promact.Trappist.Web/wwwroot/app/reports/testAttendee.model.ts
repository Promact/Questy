import { Report } from './report.model';
import { Test } from '../tests/tests.model';
import { TestLogs } from './testlog.model';

export class TestAttendee {
    firstName: string;
    lastName: string;
    email: string;
    rollNumber: string;
    TestId: number;
    contactNumber: string;
    createdDateTime: string;
    id: number;
    test: Test;
    report: Report;
    testLogs: TestLogs;
}
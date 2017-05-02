import { Report } from '../reports/report';
import { TestStatus } from '../reports/enum-test-state';
export class TestAttendee {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    createdDateTime: Date;
    starredCandidate: boolean;
    testStatus: TestStatus;
    report: Report;

    constructor() {
        this.report = new Report();
    }
}
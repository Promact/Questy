import { Report } from '../reports/report';

export class TestAttendee {
    id: number;
    rollNumber: string;
    firstName: string;
    lastName: string;
    email: string;
    contactNumber: string;
    createdDateTime: Date;
    starredCandidate: boolean;
    checkedCandidate: boolean;
    report: Report;
    backToTest: boolean;
    generatingReport: boolean;
    reporNotFoundYet: boolean;

    constructor() {
        this.report = new Report();
        this.generatingReport = false;
    }
}
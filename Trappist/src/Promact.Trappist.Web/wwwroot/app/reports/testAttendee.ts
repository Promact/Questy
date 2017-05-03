import {Report} from '../reports/report';

export class TestAttendee {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    createdDateTime: Date;
    starredCandidate: boolean;
    report: Report;

    constructor() {
        this.report = new Report();
    }
}
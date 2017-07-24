import { TestStatus } from './enum-teststate';

export class Report {
    totalMarksScored: number;
    percentage: number;
    percentile: number;
    testStatus: TestStatus;
    timeTakenByAttendee: number;
}
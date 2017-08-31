import { TestStatus } from './enum-test-state';

export class Report {
    totalMarksScored: number;
    percentage: number;
    percentile: number;
    testStatus: TestStatus;
    timeTakenByAttendee: number;
    isTestPausedUnWillingly: boolean;
    isAllowResume: boolean;

    constructor() {
        this.totalMarksScored = null;
        this.percentage = null;
        this.percentile = null;
        this.testStatus = TestStatus.allCandidates;
        this.timeTakenByAttendee = 0;
        this.isTestPausedUnWillingly = null;
        this.isAllowResume = null;
    }
}
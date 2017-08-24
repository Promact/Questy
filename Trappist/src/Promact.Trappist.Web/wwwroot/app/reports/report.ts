import { TestStatus } from '../reports/enum-test-state';

export class Report {
    totalMarksScored: number;;
    percentage: number;
    percentile: number;
    timeTakenByAttendee: number;
    testStatus: TestStatus;
    isTestPausedUnWillingly: boolean;
    isAllowResume: boolean;
    testAttendeeId: number;
}


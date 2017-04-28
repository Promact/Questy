import { TestState } from '../reports/enum-test-state';
export class TestAttendee {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    createdDateTime: Date;
    starredCandidate: boolean;
    testState: TestState;
}
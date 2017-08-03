import { AllowTestResume } from '../tests/enum-allowtestresume';

export class TestSummary{
    public timeLeft: number;
    public totalNumberOfQuestions: number;
    public attemptedQuestions: number;
    public unAttemptedQuestions: number;
    public reviewedQuestions: number;
    public resumeType: AllowTestResume;
}
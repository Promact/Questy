import { AllowTestResume } from '../tests/enum-allowtestresume';

export class TestSummary{
    public timeLeft: number;
    public totalQuestions: number;
    public attemptedQuestions: number;
    public unattemptedQuestions: number;
    public reviewedQuestions: number;
    public testResumeType: AllowTestResume;
}
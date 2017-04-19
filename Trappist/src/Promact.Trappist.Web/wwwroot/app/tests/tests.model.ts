import { QuestionOrder } from './enum-questionorder';
import { OptionOrder } from './enum-optionorder';

export class Test {
    public id: number;
    public testName: string;
    public link: string;
    public startDate: Date;
    public endDate: Date;
    public duration: number;
    public warningTime: number;
    public fromIpAddress: string;
    public toIpAddress: string;
    public warningMessage: string;
    public correctMarks: string;
    public incorrectMarks: string;
    public browserTolerance: number;
    public questionOrder: QuestionOrder;
    public optionOrder: OptionOrder;
}


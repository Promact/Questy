import { Component } from '@angular/core';
import { Test } from '../tests.model';

@Component({
    moduleId: module.id,
    selector: 'test-launch-dialog',
    templateUrl: 'test-launch-dialog.html'
})
export class TestLaunchDialogComponent {
    copiedContent: boolean;
    testSettingObject: Test;

    constructor() {
        this.testSettingObject = new Test();
        this.copiedContent = true;
    }
}


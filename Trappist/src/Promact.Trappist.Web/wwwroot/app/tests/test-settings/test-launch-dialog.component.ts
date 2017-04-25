import { Component, OnInit } from '@angular/core';
import { Test } from '../tests.model';

@Component({
    moduleId: module.id,
    selector: 'test-launch-dialog',
    templateUrl: 'test-launch-dialog.html'
})
export class TestLaunchDialogComponent implements OnInit {
    copiedContent: boolean;
    testSettingObject: Test;
    testLink: string;

    constructor() {
        this.testSettingObject = new Test();
        this.copiedContent = true;
    }

    ngOnInit() {
        let magicString = this.testSettingObject.link;
        let domain = window.location.origin;
        this.testLink = domain + '/conduct/' + magicString;
    }
}


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
    tooltipMessage: string;

    constructor() {
        this.testSettingObject = new Test();
        this.copiedContent = true;
        this.tooltipMessage = 'Copy to Clipboard';
    }

    ngOnInit() {
        let magicString = this.testSettingObject.link;
        let domain = window.location.origin;
        this.testLink = domain + '/conduct/' + magicString;
    }

    /**
     * Displays the tooltip message
     * @param $event is of type Event and is used to call stopPropagation()
     */
    showTooltipMessage($event: Event, testLink: any) {
        $event.stopPropagation();
        setTimeout(() => {
            testLink.select();
        }, 0);
        this.tooltipMessage = 'Copied';
    }

    /**
     * Changes the tooltip message
     */
    changeTooltipMessage() {
        this.tooltipMessage = 'Copy to Clipboard';
    }

}


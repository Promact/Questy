import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PlatformLocation } from '@angular/common';


@Component({
    moduleId: module.id,
    selector: 'test-end',
    templateUrl: 'test-end.html',
})
export class TestEndComponent implements OnInit {

    testEndPathContent: string;
    displayDisqualifiedMessage: boolean;

    constructor(private route: ActivatedRoute, private platformLocation: PlatformLocation) {
        platformLocation.onPopState(() => {
            window.location.replace(window.location.origin + '/pageNotFound');
            if (window.history.length !== null) {
                for (let i = 0; i < window.history.length; i++)
                    window.history[i].state(null);
            }
        });
    }

    ngOnInit() {
        this.testEndPathContent = this.route.snapshot.params['blocked'];
        this.displayMessage();
    }

    displayMessage() {
        if (this.testEndPathContent === 'blocked')
            this.displayDisqualifiedMessage = true;
    }
}

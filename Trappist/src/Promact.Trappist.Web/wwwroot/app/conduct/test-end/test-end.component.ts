import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'test-end',
    templateUrl: 'test-end.html',
})
export class TestEndComponent implements OnInit {

    testEndPathContent: string;
    displayDisqualifiedMessage: boolean;

    constructor(private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.testEndPathContent = this.route.snapshot.params['blocked'];
        this.displayMessage();
        history.pushState(null, null, null);
        window.addEventListener('popstate', function (event) {
            history.pushState(null, null, null);
        });
    }

    displayMessage() {
        if (this.testEndPathContent === 'blocked')
            this.displayDisqualifiedMessage = true;
    }
}

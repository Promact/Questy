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
    blockUrl: string;

    constructor(private route: ActivatedRoute) {
    }

    ngOnInit() {
        let url = window.location.pathname;
        this.blockUrl = url.substring(url.lastIndexOf('/cnduct/') + 21);
        this.displayMessage();
        history.pushState(null, null, null);
        window.addEventListener('popstate', function (event) {
            history.pushState(null, null, null);
        });
    }

    displayMessage() {
        if (this.blockUrl === 'test-end-block')
            this.displayDisqualifiedMessage = true;
    }
}

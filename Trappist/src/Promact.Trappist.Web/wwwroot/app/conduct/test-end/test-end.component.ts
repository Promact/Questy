import { Component, OnInit } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'test-end',
    templateUrl: 'test-end.html',
})
export class TestEndComponent implements OnInit {

    constructor() {
    }
    ngOnInit() {
        history.pushState(null, null, null);
        window.addEventListener('popstate', function (event) {
            history.pushState(null, null, null);
        });
    }
}

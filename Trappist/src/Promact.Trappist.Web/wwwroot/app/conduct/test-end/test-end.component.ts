import { Component } from '@angular/core';
import { Router } from '@angular/router';
import * as screenfull from 'screenfull';

@Component({
    moduleId: module.id,
    selector: 'test-end',
    templateUrl: 'test-end.html',
})
export class TestEndComponent {

    constructor(public router:Router) {

    }

    public closeTest() {
        if (screenfull.enabled) {
            screenfull.toggle();
        }
        this.router.navigate(['test-summary']);
    }
}

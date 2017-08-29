import { Component, Input, Output } from '@angular/core';
import { Http } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ConductService } from './conduct/conduct.service';





@Component({
    moduleId: module.id,
    selector: 'app',
    templateUrl: 'app.html',
})
export class AppComponent {

    name = 'Angular';
    isPageTestPreview: boolean;
    disableHeader: boolean;

    constructor(private http: Http, private conductService: ConductService) {
        this.isPageTestPreview = true;
        let url = window.location.pathname;
        this.isPageTestPreview = url.substring(url.indexOf('/tests/') + 18) === 'preview';
        this.conductService.disableHeader.subscribe(disableHeader => {
            this.disableHeader = disableHeader;
        });
    }

    /**
     * user is logged out and redirected to login page
     */
    logOff() {
        let logoutForm = <HTMLFormElement>document.getElementById('logoutForm');
        logoutForm.submit();
    }
}
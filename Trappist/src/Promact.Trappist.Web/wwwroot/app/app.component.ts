import { Component, Input, Output } from '@angular/core';
import { Http } from '@angular/http';
import { TestService } from './tests/tests.service';

@Component({
    selector: 'app',
    templateUrl: './app.html'
})
export class AppComponent {

    name = 'Angular';
    isPageTestPreview: boolean;
    disableHeader: boolean;

    constructor(private http: Http, private testService:TestService) {
        this.testService.isTestPreviewIsCalled.subscribe(value => {
            this.isPageTestPreview = value;
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
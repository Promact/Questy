import { Component, Input, Output } from '@angular/core';
import { Http } from '@angular/http';



@Component({
    moduleId: module.id,
    selector: 'app',
    templateUrl: 'app.html',
})
export class AppComponent {

    name = 'Angular';
    isPageTestPreview: boolean;

    constructor(private http: Http) {
        this.isPageTestPreview = true;
        let url = window.location.pathname;
        this.isPageTestPreview = url.substring(url.indexOf('/tests/') + 18) === 'preview' ? true : false;
    }

   
    /**
     * user is logged out and redirected to login page
     */
    logOff() {
        let logoutForm = <HTMLFormElement>document.getElementById('logoutForm');
        logoutForm.submit();
    }
}
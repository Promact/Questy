import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    moduleId: module.id.toString(),
    selector: 'app',
    templateUrl: 'app.html',
})
export class AppComponent {

    name = 'Angular';

    constructor(private http: Http) {

    }

    /**
     * user is logged out and redirected to login page
     */
    logOff() {
        let logoutForm = <HTMLFormElement>document.getElementById('logoutForm');
        logoutForm.submit();
    }
}
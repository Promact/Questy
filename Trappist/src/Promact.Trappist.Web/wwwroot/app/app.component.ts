import { Component } from '@angular/core';
import { Http } from "@angular/http";

@Component({
    selector: 'app',
    templateUrl: 'app/app.html',
})
export class AppComponent {

    name = 'Angular';

    constructor(private http: Http) {

    }
}
import { Component } from '@angular/core';
import { Http } from "@angular/http";

@Component({
    moduleId: module.id,
    selector: 'app',
    templateUrl: 'app.html',
})
export class AppComponent {

    name = 'Angular';

    constructor(private http: Http) {

    }
}
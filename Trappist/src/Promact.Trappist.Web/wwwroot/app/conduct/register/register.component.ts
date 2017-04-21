import { Component } from '@angular/core';
import { TestAttendees } from './register.model';
import { ConductService } from '../conduct.service';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'register',
    templateUrl: 'register.html',
})
export class RegisterComponent {
    testAttendees: TestAttendees;
    loader: boolean;
    isErrorMessage: boolean;

    constructor(private conductService: ConductService, private router: Router) {
        this.testAttendees = new TestAttendees();
    }

    /**
     * This method used for register test attendee.
     */
    registerTestAttendee() {
        this.loader = true;
        let registrationUrl = window.location.pathname;
        let magicString = registrationUrl.substring(registrationUrl.indexOf('/conduct/') + 9, registrationUrl.indexOf('/register'));
        this.conductService.registerTestAttendee(magicString, this.testAttendees).subscribe(response => {
            this.isErrorMessage = false;
            this.loader = false;
            this.router.navigate(['instructions']);
        }, err => {
            this.isErrorMessage = true;
            this.loader = false;
        });
    }
}
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
    testAttendees: TestAttendees = new TestAttendees();
    isErrorMessage: boolean;
    magicString: string;
    registrationUrl: string;
    loader: boolean;

    constructor(private conductService: ConductService, private router: Router) {
    }

    /**
     * This method used for register test attendee.
     */
    registerTestAttendee() {
        this.loader = true;
        this.registrationUrl = window.location.pathname;
        this.magicString = this.registrationUrl.substring(this.registrationUrl.indexOf('/conduct/') + 9, this.registrationUrl.indexOf('/register'));
        this.conductService.registerTestAttendee(this.magicString, this.testAttendees).subscribe(response => {
            if (response === true) {
                this.isErrorMessage = false;
                this.loader = false;
                this.router.navigate(['instructions']);
            }
            else
                this.isErrorMessage = true;
            this.loader = false;
        }, err => {
            this.isErrorMessage = true;
            this.loader = false;
        });
    }
}
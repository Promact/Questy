import { Component } from '@angular/core';
import { TestAttendees } from './register.model';
import { ConductService } from '../conduct.service';
import { Router } from '@angular/router';
import { ConnectionService } from '../../core/connection.service';

@Component({
    moduleId: module.id,
    selector: 'register',
    templateUrl: 'register.html',
})
export class RegisterComponent {
    testAttendees: TestAttendees;
    loader: boolean;
    isErrorMessage: boolean;

    constructor(private conductService: ConductService, private router: Router, private connectionService: ConnectionService) {
        this.testAttendees = new TestAttendees();
        this.loader = true;
        this.conductService.getSessionPath().subscribe(path => {
            if (path)
                this.router.navigate([path.path], { replaceUrl: true });
            this.loader = false;
        }, err => {
            this.loader = false;
        });

    }

    /**
     * This method used for register test attendee.
     */
    registerTestAttendee() {
        let registrationUrl = window.location.pathname;
        let magicString = registrationUrl.substring(registrationUrl.indexOf('/conduct/') + 9, registrationUrl.indexOf('/register'));
        this.conductService.registerTestAttendee(magicString, this.testAttendees).subscribe(response => {
            if (response) {
                this.connectionService.sendReport(response);
                this.isErrorMessage = false;
                this.loader = false;
                this.router.navigate(['instructions'], { replaceUrl: true });
            }

        }, err => {
            if (err.status === 404) {
                this.isErrorMessage = true;
                this.loader = false;
            }
        });
    }
}
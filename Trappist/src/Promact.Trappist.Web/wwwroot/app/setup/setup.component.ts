import { Component } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'setup',
    templateUrl: 'setup.html',
})
export class SetupComponent {

    constructor() {
    }

    navigateToLogin() {
        window.location.href = '/login';
    }
}
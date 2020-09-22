import { Component } from '@angular/core';
import { TestService } from './tests/tests.service';





@Component({
    moduleId: module.id,
    selector: 'app',
    templateUrl: 'app.html',
})
export class AppComponent {

    name = 'Angular';
    isPageTestPreview: boolean;
    disableHeader: boolean;

    constructor(private testService: TestService) {
        this.testService.isTestPreviewIsCalled.subscribe(value => {
            this.isPageTestPreview = value;
        });
    }

    /**
     * user is logged out and redirected to login page
     */
    logOff() {
        const logoutForm = <HTMLFormElement>document.getElementById('logoutForm');
        logoutForm.submit();
    }
}
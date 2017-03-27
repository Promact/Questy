import { Component } from '@angular/core';
import { ConnectionString } from './setup.model';
import { SetupService } from './setup.service';
import { EmailSettings } from './setup.model';
import { BasicSetup } from './setup.model';
import { RegistrationFields } from './setup.model';
@Component({
    moduleId: module.id,
    selector: 'setup',
    templateUrl: 'setup.html',
})

export class SetupComponent {
    private basicSetup: BasicSetup = new BasicSetup();
    private emailSettings: EmailSettings = new EmailSettings();
    private connectionString: ConnectionString = new ConnectionString();
    private registrationFields: RegistrationFields = new RegistrationFields();
    confirmPasswordValid: boolean;
    errorMessage: boolean;
    loader: boolean = false;
    
    constructor(private setupService: SetupService) {
        this.emailSettings.connectionSecurityOption = 'None';
    }

    /**
     * This method used for validating connection string.
     * @param setup
     * @param connectionStringName
     */
    validateConnectionString(setup: any) {
        this.loader = true;
        this.setupService.validateConnectionString(this.connectionString).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.next();
                this.loader = false;
            }
            else {
                this.errorMessage = true;
                this.loader = false;
            }
        });
    }

    /**
     * This method used for verifying email Settings
     * @param setup
     */
    validateEmailSettings(setup: any) {
        this.loader = true;        
        this.setupService.validateEmailSettings(this.emailSettings).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.next();
                this.loader = false;
            }
            else {
                this.errorMessage = true;
                this.loader = false;
            }
        });
    }

    /**
     * This method used for validating Password and Confirm Password matched or not.
     */
    isValidPassword() {
        if (this.registrationFields.confirmPassword === this.registrationFields.password)
            this.confirmPasswordValid = true;
        else
            this.confirmPasswordValid = false;
    }

    /**
     * This method used for Creating user
     * @param setup
     */
    createUser(setup: any) {
        this.loader = true;
        this.basicSetup.emailSettings = this.emailSettings;
        this.basicSetup.connectionString = this.connectionString;
        this.basicSetup.registrationFields = this.registrationFields;
        this.setupService.createUser(this.basicSetup).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.complete();
                this.navigateToLogin();
                this.loader = false;
            }
            else {
                this.errorMessage = true;
                this.loader = false;
            }
        });
    }

    navigateToLogin() {
        window.location.href = '/login';
    }

    previousStep1(setup: any) {
        setup.previous();
    }

    previousStep2(setup: any) {
        setup.previous();
    }
}
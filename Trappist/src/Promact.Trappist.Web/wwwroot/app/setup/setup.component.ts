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
    confirmPasswordValid: boolean;
    connectionSecurityOption: string = 'None';
    connectionStringName: string;
    server: string;
    port: number;
    userName: string;
    password: string;
    nameOfUser: string;
    email: string;
    passwordOfUser: string;
    confirmPassword: string;
    errorMessage: boolean;

    constructor(private setupService: SetupService) {
    }

    /**
     * This method used for validating connection string.
     * @param setup
     * @param connectionStringName
     */
    validateConnectionString(setup: any) {
        this.connectionString.value = this.connectionStringName;
        this.setupService.validateConnectionString('api/setup/connectionstring', this.connectionString).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.next();
            }
            else
                this.errorMessage = true;
        });
    }

    /**
     *This method used for verifying email Settings 
     * @param setup
     * @param server
     * @param port
     * @param username
     * @param password
     * @param connectionSecurityOption
     */
    validateEmailSettings(setup: any) {
        this.emailSettings.server = this.server;
        this.emailSettings.port = this.port;
        this.emailSettings.userName = this.userName;
        this.emailSettings.password = this.password;
        this.emailSettings.connectionSecurityOption = this.connectionSecurityOption;
        this.setupService.validateEmailSettings('api/setup/mailsettings', this.emailSettings).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.next();
            }
            else
                this.errorMessage = true;
        });
    }

    /**
     * This method used for validating Password and Confirm Password matched or not.
     * @param password
     * @param confirmPassword
     */
    isValidPassword() {
        if (this.confirmPassword === this.password)
            this.confirmPasswordValid = true;
        else
            this.confirmPasswordValid = false;
    }

    /**
     * This method used for Creating user
     * @param setup
     * @param name
     * @param email
     * @param password
     * @param confirmPassword
     */
    createUser(setup: any) {
        this.basicSetup.emailSettings = this.emailSettings;
        this.basicSetup.connectionString = this.connectionString;
        this.basicSetup.registrationFields = new RegistrationFields();
        this.basicSetup.registrationFields.name = this.nameOfUser;
        this.basicSetup.registrationFields.email = this.email;
        this.basicSetup.registrationFields.password = this.password;
        this.basicSetup.registrationFields.confirmPassword = this.confirmPassword;
        this.setupService.createUser('api/setup/validateuser', this.basicSetup).subscribe(response => {
            if (response === true) {
                this.errorMessage = false;
                setup.complete();
                this.navigateToLogin();
            }
            else
                this.errorMessage = true;
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
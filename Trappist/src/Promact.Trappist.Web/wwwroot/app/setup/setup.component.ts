﻿import { Component } from '@angular/core';
import { ConnectionString } from './setup.model';
import { SetupService } from './setup.service';
import { EmailSettings } from './setup.model';
import { BasicSetup } from './setup.model';
import { RegistrationFields } from './setup.model';
import { ServiceResponse } from './setup.model';
@Component({
    moduleId: module.id,
    selector: 'setup',
    templateUrl: 'setup.html',
})

export class SetupComponent {
    basicSetup: BasicSetup = new BasicSetup();
    emailSettings: EmailSettings = new EmailSettings();
    connectionString: ConnectionString = new ConnectionString();
    registrationFields: RegistrationFields = new RegistrationFields();
    confirmPasswordValid: boolean;
    stepOneErrorMessage: boolean = false;
    stepTwoErrorMessage: boolean = false;
    stepThreeErrorMessage: boolean = false;
    loader: boolean;
    exceptionMessage: string;

    constructor(private setupService: SetupService) {
        this.emailSettings.connectionSecurityOption = 'None';
    }

    /**
     * This method used for validating connection string.
     * @param setup
     */
    validateConnectionString(setup: any) {
        this.loader = true;
        this.setupService.validateConnectionString(this.connectionString).subscribe((response) => {
            if (response === true)
                setup.next();
            else
                this.stepOneErrorMessage = true;
            this.loader = false;
        }, err => {
            this.stepOneErrorMessage = true;
            this.loader = false;
        });
    }

    /**
     * This method used for verifying email Settings
     * @param setup
     */
    validateEmailSettings(setup: any) {
        this.loader = true;
        this.setupService.validateEmailSettings(this.emailSettings).subscribe(response => {
            if (response === true)
                setup.next();
            else
                this.stepTwoErrorMessage = true;
            this.loader = false;
        }, err => {
            this.stepTwoErrorMessage = true;
            this.loader = false;
        });
    }

    /**
     * This method used to redirect user to admin registration page if admin want to
       proceed further in case of email setting are invalid due to the server or any issue.
     * @param setup
     */
    proccedFurtherWithoutEmailSettings(setup: any) {
        setup.next();
    }

    /**
     * This method used for validating Password and Confirm Password matched or not.
     */
    isValidPassword() {
        this.confirmPasswordValid = this.registrationFields.confirmPassword === this.registrationFields.password;
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
        this.setupService.createUser(this.basicSetup).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.isSuccess === true) {
                setup.complete();
                this.navigateToLogin();
            }
            else {
                this.stepThreeErrorMessage = true;
                this.exceptionMessage = serviceResponse.exceptionMessage;
            }
            this.loader = false;
        }, err => {
            this.stepThreeErrorMessage = true;
            this.loader = false;
        });
    }

    navigateToLogin() {
        window.location.href = '/login';
    }

    previousStep1(setup: any) {
        this.stepOneErrorMessage = false;
        this.stepTwoErrorMessage = false;
        setup.previous();
    }

    previousStep2(setup: any) {
        this.stepTwoErrorMessage = false;
        this.stepThreeErrorMessage = false;
        setup.previous();
    }
}
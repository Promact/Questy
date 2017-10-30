import { ComponentFixture, TestBed, async, inject } from '@angular/core/testing';
import { SetupComponent } from './setup.component';
import { SetupService } from './setup.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { CoreModule } from '../core/core.module';
import { FormWizardModule, WizardComponent, WizardStepComponent } from 'angular2-wizard-fix';
import { Observable } from 'rxjs/Rx';
import { ConnectionString, ServiceResponse } from './setup.model';

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

describe('Testing of setup component:-', () => {
    let fixture: ComponentFixture<SetupComponent>;
    let setupComponent: SetupComponent;
    let connectionString = new ConnectionString;
    let setup: any = {};
    let response: ServiceResponse;

    response = new ServiceResponse();
    response.isSuccess = true;
    response.exceptionMessage = '';

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                SetupComponent,
            ],
            providers: [
                SetupService,
                { provide: Router, useClass: RouterStub },
                { provide: ActivatedRoute, useclass: ActivatedRoute },
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule, FormWizardModule]
        }).compileComponents();

    }));
    beforeEach(() => {
        fixture = TestBed.createComponent(SetupComponent);
        setupComponent = fixture.componentInstance;
        setup.next = function () { };
        setup.complete = function () { };

    });

    it('should return true if connection string is valid', () => {
        spyOn(SetupService.prototype, 'validateConnectionString').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(setup, 'next');
        setupComponent.validateConnectionString(setup);
        expect(setup.next).toHaveBeenCalled();
    });

    it('should show error messsage if connection string is invalid', () => {
        spyOn(SetupService.prototype, 'validateConnectionString').and.callFake(() => {
            return Observable.of(false);
        });
        setupComponent.validateConnectionString(setup);
        expect(setupComponent.stepOneErrorMessage).toBeTruthy();
    });

    it('should show error message if internal server error is thrown', () => {
        spyOn(SetupService.prototype, 'validateConnectionString').and.callFake(() => {
            return Observable.throw(Error);
        });
        setupComponent.validateConnectionString(setup);
        expect(setupComponent.stepOneErrorMessage).toBeTruthy();
    });

    it('should return true if email setting is valid', () => {
        spyOn(SetupService.prototype, 'validateEmailSettings').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(setup, 'next');
        setupComponent.validateEmailSettings(setup);
        expect(setup.next).toHaveBeenCalled();
    });

    it('should show error message if email setting is invalid', () => {
        spyOn(SetupService.prototype, 'validateEmailSettings').and.callFake(() => {
            return Observable.of(false);
        });
        setupComponent.validateEmailSettings(setup);
        expect(setupComponent.stepTwoErrorMessage).toBeTruthy();
    });

    it('should show error message if internal server error is thrown', () => {
        spyOn(SetupService.prototype, 'validateEmailSettings').and.callFake(() => {
            return Observable.throw(Error);
        });
        setupComponent.validateEmailSettings(setup);
        expect(setupComponent.stepTwoErrorMessage).toBeTruthy();
    });

    it('should create user succesfully and navigate to login', () => {
        spyOn(SetupService.prototype, 'createUser').and.callFake(() => {
            return Observable.of(response);
        });
        spyOn(setup, 'complete');
        spyOn(setupComponent, 'navigateToLogin');
        setupComponent.createUser(setup);
        expect(setup.complete).toHaveBeenCalled();
    });

    it('should show error message if user creation fails', () => {
        response.isSuccess = false;
        spyOn(SetupService.prototype, 'createUser').and.callFake(() => {
            return Observable.of(response);
        });
        setupComponent.createUser(setup);
        expect(setupComponent.stepThreeErrorMessage).toBeTruthy();
    });

    it('should show error message if internal server error is thrown ', () => {
        spyOn(SetupService.prototype, 'createUser').and.callFake(() => {
            return Observable.throw(Error);
        });
        setupComponent.createUser(setup);
        expect(setupComponent.stepThreeErrorMessage).toBeTruthy();
    });

    it('should return true if new password matched confirmed password ', () => {
        setupComponent.registrationFields.password = 'Abc@12345';
        setupComponent.registrationFields.confirmPassword = 'Abc@12345';
        setupComponent.isValidPassword();
        expect(setupComponent.confirmPasswordValid).toBeTruthy();
    });
});

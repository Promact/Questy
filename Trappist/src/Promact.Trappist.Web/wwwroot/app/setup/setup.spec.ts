//import { ComponentFixture, TestBed, async, inject } from '@angular/core/testing';
//import { SetupComponent } from './setup.component';
//import { SetupService } from './setup.service';
//import { ActivatedRoute, Router, RouterModule } from '@angular/router';
//import { BrowserModule, By } from '@angular/platform-browser';
//import { FormsModule } from '@angular/forms';
//import { MaterialModule } from '@angular/material';
//import { CoreModule } from '../core/core.module';
//import { FormWizardModule, WizardComponent, WizardStepComponent } from 'angular2-wizard-fix';
//import { Observable } from 'rxjs/Rx';
//import { ConnectionString } from './setup.model';
//import { Component } from "@angular/core";

//class RouterStub {
//    navigateByUrl(url: string) { return url; }
//}

//@Component({
//    selector: 'setup',
//    template:'setup.html',
//})

//class TestWrapperComponent {
//}

//describe('testing of setup:-', () => {
//    let fixture: ComponentFixture<SetupComponent>;
//    let setupComponent: SetupComponent;
//    let connectionString = new ConnectionString();


//    //connectionString.value ='server=(localdb)\mssqllocaldb;Database=Trappist;integrated security=true'

//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            declarations: [
//                SetupComponent
//            ],

//            providers: [
//                SetupService,
//                { provide: Router, useClass: RouterStub },
//                { provide: ActivatedRoute, useclass: ActivatedRoute },
//            ],

//            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule, FormWizardModule]
//        }).compileComponents();

//    }));
//    beforeEach(() => {
//        fixture = TestBed.createComponent(SetupComponent);
//        setupComponent = fixture.componentInstance;
       
//    });

//    it('should return true if connection string is valid', () => {
//        spyOn(SetupService.prototype, 'validateConnectionString').and.callFake(() => {
//            return Observable.of(true);
//        })
//        setupComponent.validateConnectionString(connectionString);
//        expect(setupComponent.stepOneErrorMessage).toBeFalsy();
//    })

//    it('should throw error messsage if connection string is invalid', () => {
//        spyOn(SetupService.prototype, 'validateConnectionString').and.callFake(() => {
//            return Observable.of(true);
//        })
//        setupComponent.validateConnectionString(connectionString);
//        expect(setupComponent.stepOneErrorMessage).toBeTruthy();
//    })

//    it('should return true if new password matched confirmed password ', () => {
//        setupComponent.registrationFields.password = 'Abc@12345';
//        setupComponent.registrationFields.confirmPassword = 'Abc@12345';
//        setupComponent.isValidPassword();
//        expect(setupComponent.confirmPasswordValid).toBeTruthy();
//    });
//});

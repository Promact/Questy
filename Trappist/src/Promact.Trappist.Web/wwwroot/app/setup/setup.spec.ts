import { ComponentFixture, TestBed, async } from '@angular/core/testing';
import { SetupComponent } from './setup.component';
import { SetupService } from './setup.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BrowserModule, By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { CoreModule } from '../core/core.module';
import { FormWizardModule } from 'angular2-wizard-fix';
import { Observable } from 'rxjs/Rx';

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

describe('testing of setup:-', () => {
    let fixture: ComponentFixture<SetupComponent>;
    let setupComponent: SetupComponent;
    let connectionString: any;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                SetupComponent
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
    });

    it('should return true if new password matched confirmed password ', () => {
        setupComponent.registrationFields.password = 'Abc@12345';
        setupComponent.registrationFields.confirmPassword = 'Abc@12345';
        setupComponent.isValidPassword();
        expect(setupComponent.confirmPasswordValid).toBeTruthy();
    });
});

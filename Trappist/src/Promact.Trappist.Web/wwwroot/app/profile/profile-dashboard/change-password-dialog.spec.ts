import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { TestBed, async, fakeAsync, ComponentFixture, tick } from '@angular/core/testing';

import { MaterialModule, MdDialogModule, MdDialogRef } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { HttpService } from '../../core/http.service';
import { CoreModule } from '../../core/core.module';
import { DebugElement } from '@angular/core/core';
import { ChangePasswordDialogComponent } from './change-password-dialog.component';
import { ProfileService } from '../profile.service';
import { ChangePasswordModel } from '../password.model';
import { Observable } from 'rxjs/Rx';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule } from '@angular/http';

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

class MockDialog {
    open() {
        return true;
    }

    close() {
        return true;
    }
}

class MockError {
    json(): Observable<any> {
        return Observable.of({ 'error': ['old password is wrong'] });
    }
}

describe('Testing of change-password component:-', () => {
    let fixture: ComponentFixture<ChangePasswordDialogComponent>;
    let changePasswordComponent: ChangePasswordDialogComponent;

    let changedPassword = new ChangePasswordModel();
    changedPassword.oldPassword = 'Tyu@12345';
    changedPassword.newPassword = 'Abc@12345';
    changedPassword.confirmPassword = 'Abc@12345';

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                ChangePasswordDialogComponent],

            providers: [
                ProfileService,
                { provide: Router, useClass: RouterStub },
                { provide: ActivatedRoute, useclass: ActivatedRoute },
                { provide: MdDialogRef, useClass: MockDialog },
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, HttpModule, RouterModule, CoreModule, MdDialogModule, BrowserAnimationsModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ChangePasswordDialogComponent);
        changePasswordComponent = fixture.componentInstance;
    });

    it('should check whether new and confirm pasword same or not and update it', () => {
        spyOn(ProfileService.prototype, 'updateUserPassword').and.callFake(() => {
            return Observable.of(true);
        });
        spyOn(changePasswordComponent.snackBar, 'open').and.callThrough();
        spyOn(changePasswordComponent.dialog, 'close').and.callThrough();

        changePasswordComponent.changePassword(changedPassword);
        expect(changePasswordComponent.isPasswordSame).toBeTruthy();
        expect(changePasswordComponent.dialog.close).toHaveBeenCalled();
        expect(changePasswordComponent.snackBar.open).toHaveBeenCalled();
    });

    it('should throw error message if password update fails', () => {
        spyOn(ProfileService.prototype, 'updateUserPassword').and.callFake(() => {
            return Observable.throw(new MockError());
        });
        changePasswordComponent.changePassword(changedPassword);
        expect(changePasswordComponent.errorCorrection).toBeTruthy();
    });

    it('should show error message if new and confirm pasword are not same', () => {
        spyOn(ProfileService.prototype, 'updateUserPassword').and.callFake(() => {
            return Observable.of(false);
        });
        changedPassword.confirmPassword = 'Abv@123456';
        changePasswordComponent.changePassword(changedPassword);
        expect(changePasswordComponent.isPasswordSame).toBeFalsy();
    });

    it('should check condition for showing error message', () => {
        changePasswordComponent.changeCurrentPassword();
        expect(changePasswordComponent.isPasswordSame).toBeTruthy();
    });
});
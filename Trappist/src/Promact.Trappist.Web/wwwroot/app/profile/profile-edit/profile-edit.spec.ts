import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { TestBed, async, fakeAsync, ComponentFixture, tick } from '@angular/core/testing';
import { MaterialModule, MdSnackBar } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { HttpService } from '../../core/http.service';
import { CoreModule } from '../../core/core.module';
import { DebugElement } from '@angular/core/core';
import { ProfileEditComponent } from './profile-edit.component';
import { ProfileService } from '../profile.service';
import { ApplicationUser } from '../profile.model';
import { Observable } from 'rxjs/Rx';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate() { return true;}
}

describe('Testing of profile-edit component:-', () => {
    let fixture: ComponentFixture<ProfileEditComponent>;
    let profileEditComponent: ProfileEditComponent;

    let applicationUserDetails = new ApplicationUser();
    applicationUserDetails.name = 'Suparna';
    applicationUserDetails.email = 'suparna@promactinfo.com';
    applicationUserDetails.organizationName = null;
    applicationUserDetails.phoneNumber = null;


    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                ProfileEditComponent],

            providers: [
                ProfileService,
                { provide: ActivatedRoute, useclass: ActivatedRoute },
                { provide: Router, useClass: RouterStub }
            ],

            imports: [BrowserModule, RouterModule.forRoot([]), FormsModule, MaterialModule, RouterModule, CoreModule, BrowserAnimationsModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ProfileEditComponent);
        profileEditComponent = fixture.componentInstance;
    });


    it('should return the user details', () => {
        spyOn(ProfileService.prototype, 'getUserDetails').and.callFake(() => {
            return Observable.of(applicationUserDetails);
        });
        profileEditComponent.getUserDetails();
        expect(profileEditComponent.editUser.name).toBe('Suparna');
    });

    it('should update the user details', () => {
        applicationUserDetails.phoneNumber = '7896541230';
        applicationUserDetails.organizationName = 'Promact';
        spyOn(ProfileService.prototype, 'updateUserDetails').and.callFake(() => {
            return Observable.of(applicationUserDetails);
        });
        spyOn(MdSnackBar.prototype, 'open').and.callThrough();
        profileEditComponent.updateUserDetails();
        expect(MdSnackBar.prototype.open).toHaveBeenCalled();
    });

});
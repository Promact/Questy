import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { TestBed, async, fakeAsync, ComponentFixture, inject } from '@angular/core/testing';
import { MaterialModule, MdDialogModule, MdDialogRef } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { HttpService } from '../../core/http.service';
import { CoreModule } from '../../core/core.module';
import { DebugElement } from '@angular/core/core';
import { ProfileService } from "../profile.service";
import { Observable } from "rxjs/Rx";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ProfileDashboardComponent } from "./profile-dashboard.component";
import { ApplicationUser } from "../profile.model";
import { APP_BASE_HREF } from "@angular/common";
import { ProfileEditComponent } from "../profile-edit/profile-edit.component";
import { ChangePasswordDialogComponent } from "./change-password-dialog.component";
import { BrowserDynamicTestingModule } from "@angular/platform-browser-dynamic/testing";
import { HttpModule } from "@angular/http";

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

class MockActivatedRoute {

}

describe('testing of profile-dashboard component:-', () => {
    let fixture: ComponentFixture<ProfileDashboardComponent>;
    let profileDashboardComponent: ProfileDashboardComponent;

    let applicationUserDetails = new ApplicationUser();
    applicationUserDetails.name = 'Suparna';
    applicationUserDetails.email = 'suparna@promactinfo.com';
    applicationUserDetails.organizationName = 'Promact';
    applicationUserDetails.phoneNumber = '7896541230';

    beforeEach(async(() => {
        TestBed.overrideModule(BrowserDynamicTestingModule, {
            set: {
                entryComponents: [ChangePasswordDialogComponent]
            }
        });
        TestBed.configureTestingModule({
            declarations: [
                ProfileDashboardComponent,
                ChangePasswordDialogComponent
            ],

            providers: [
                ProfileService,
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: Router, useClass: RouterStub },
                {
                    provide: ActivatedRoute, useClass: MockActivatedRoute
                },
                { provide: MdDialogRef, useClass: MockDialog },
                HttpService
            ],

            imports: [BrowserModule, FormsModule, RouterModule.forRoot([]),MaterialModule, HttpModule, RouterModule, CoreModule, BrowserAnimationsModule, MdDialogModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ProfileDashboardComponent);
        profileDashboardComponent = fixture.componentInstance;
    });

    //it('should return the details of the application user', async(inject([ProfileService], (proService: ProfileService) => {
    //    spyOn(proService, 'getUserDetails').and.returnValue(Observable.of(applicationUserDetails));
    //    profileDashboardComponent.getUserDetails();
    //    fixture.detectChanges();

    //    fixture.whenStable().then(() => {
    //        expect(profileDashboardComponent.user.name).toBe('Suparna');
    //    });       
    //})));

    //it('should open the change password dialog component', () => {
    //    spyOn(profileDashboardComponent.dialog, 'open').and.callThrough();
    //    profileDashboardComponent.changePasswordDialog();
    //    expect(profileDashboardComponent.dialog.open).toHaveBeenCalled();

    //});
});
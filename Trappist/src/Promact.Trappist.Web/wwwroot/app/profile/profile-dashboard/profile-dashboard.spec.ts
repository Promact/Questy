import { TestBed, async, ComponentFixture, inject } from '@angular/core/testing';
import { ProfileService } from '../profile.service';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MaterialModule, MdDialogRef, MdDialogModule } from '@angular/material';
import { HttpModule } from '@angular/http';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { CoreModule } from '../../core/core.module';
import { ProfileDashboardComponent } from './profile-dashboard.component';
import { Observable } from 'rxjs/Rx';
import { ProfileComponent } from '../profile.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpService } from '../../core/http.service';
import { APP_BASE_HREF } from '@angular/common';
import { ApplicationUser } from '../profile.model';
import { ChangePasswordDialogComponent } from './change-password-dialog.component';

class MockDialog {
    open() {
        return true;
    }

    close() {
        return true;
    }
}

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

describe('Testing of profile-dashboard component:-', () => {
    let dashboardfixture: ComponentFixture<ProfileDashboardComponent>;
    let profileDashboardComponent: ProfileDashboardComponent;

    let applicationUserDetails = new ApplicationUser();
    applicationUserDetails.name = 'Suparna';
    applicationUserDetails.email = 'suparna@promactinfo.com';
    applicationUserDetails.organizationName = 'Promact';
    applicationUserDetails.phoneNumber = '7896541230';
    beforeEach(async(() => {
        TestBed.overrideModule(BrowserAnimationsModule, {
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
                HttpService,
                { provide: APP_BASE_HREF, useValue: '/' },
                { provide: MdDialogRef, useClass: MockDialog },
                { provide: ActivatedRoute }
            ],

            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule.forRoot([]), HttpModule, CoreModule, MdDialogModule, BrowserAnimationsModule]
        }).compileComponents();
    }));

    beforeEach(() => {
        dashboardfixture = TestBed.createComponent(ProfileDashboardComponent);
        profileDashboardComponent = dashboardfixture.componentInstance;
    });

    it('should return the details of the application user', () => {
        spyOn(ProfileService.prototype, 'getUserDetails').and.callFake(() => {
            return Observable.of(applicationUserDetails);
        });
        profileDashboardComponent.getUserDetails();
        expect(profileDashboardComponent.user.name).toBe('Suparna');
    });

    it('should open the change password dialog component', () => {
        spyOn(profileDashboardComponent.dialog, 'open').and.callThrough();
        profileDashboardComponent.changePasswordDialog();
        expect(profileDashboardComponent.dialog.open).toHaveBeenCalled();
    });
});
import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog } from '@angular/material';
import { ApplicationUser } from '../profile.model';
import { ProfileService } from '../profile.service';
import { ChangePasswordDialogComponent } from './change-password-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'profile-dashboard',
    templateUrl: 'profile-dashboard.html'
})
export class ProfileDashboardComponent implements OnInit {
    user: ApplicationUser = new ApplicationUser;
    loader: boolean;

    ngOnInit() {
        this.getUserDetails();
    }
    constructor(public profileService: ProfileService, public dialog: MdDialog) {
        this.loader = true;
    }

    /**
    * get details of the user
    */
    getUserDetails() {
        this.profileService.getUserDetails().subscribe((response) => {
            this.loader = false;
            this.user = response;
        });
    }

    // Open Change Password Dialog
    changePasswordDialog() {
        this.dialog.open(ChangePasswordDialogComponent);
    }
}

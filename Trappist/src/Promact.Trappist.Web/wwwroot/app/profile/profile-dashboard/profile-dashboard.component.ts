import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { ApplicationUser } from "../profile.model";
import { ProfileService } from "../profile.service";

@Component({
    moduleId: module.id,
    selector: "profile-dashboard",
    templateUrl: "profile-dashboard.html"
})
export class ProfileDashboardComponent implements OnInit {

  user: ApplicationUser = new ApplicationUser;

  ngOnInit() {
    this.getUserDetails();
  }

  constructor(public profileService:ProfileService, public dialog: MdDialog) { }

  /**
  * get details of the user
  */
  getUserDetails() {
    this.profileService.getUserDetails().subscribe((response) => {
      this.user = response;
    });
  }

    // Open Change Password Dialog
    changePasswordDialog() {
        this.dialog.open(ChangePasswordDialogComponent);
    }
   
}

@Component({
    moduleId: module.id,
    selector: 'change-password-dialog',
    templateUrl: "change-password-dialog.html"
})
export class ChangePasswordDialogComponent { }

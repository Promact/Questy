import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { Http } from '@angular/http';
import { ChangePasswordDialogComponent } from "./change-password-dialog.component";

@Component({
  moduleId: module.id,
  selector: "profile-dashboard",
  templateUrl: "profile-dashboard.html"
})
export class ProfileDashboardComponent implements OnInit {

  user: ApplicationUser = new ApplicationUser;



  ngOnInit() {
    this.GetUserDetails();
  }

  constructor(public http: Http, public dialog: MdDialog) { }

  // Get's current user's details from the server and displays it
  GetUserDetails() {
    this.http.get("api/Profile").subscribe((response) => {
      this.user = response.json();
    });
  }

    // Open Change Password Dialog
    changePasswordDialog() {
        this.dialog.open(ChangePasswordDialogComponent);
    }
   
}

class ApplicationUser {
  Name: string;
  OrganizationName: string;
  UserName: string;
  PhoneNumber: string;
}



@Component({
    moduleId: module.id,
    selector: 'change-password-dialog',
    templateUrl: "change-password-dialog.html"
})
export class ChangePasswordDialogComponent { }

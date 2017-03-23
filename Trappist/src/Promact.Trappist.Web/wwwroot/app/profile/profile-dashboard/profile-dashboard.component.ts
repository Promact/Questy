import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';
import { ChangePasswordDialogComponent } from "./change-password-dialog.component";

@Component({
  moduleId: module.id,
  selector: "profile-dashboard",
  templateUrl: "profile-dashboard.html"
})

export class ProfileDashboardComponent {

  // Open Change Password Dialog
  constructor(public dialog: MdDialog) { }

  changePasswordDialog() {
    this.dialog.open(ChangePasswordDialogComponent);
  }
}

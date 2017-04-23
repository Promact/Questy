import { Component } from "@angular/core";
import { ChangePasswordModel } from "../password.model";
import { MdDialogRef } from '@angular/material';
import { ProfileService } from "../profile.service";

@Component({
  moduleId: module.id,
  selector: 'change-password-dialog',
  templateUrl: "change-password-dialog.html"
})
export class ChangePasswordDialogComponent {
  constructor(public profileService: ProfileService, public dialog: MdDialogRef<any>) { }
  user: ChangePasswordModel = new ChangePasswordModel();

  /**
   * calls the server to update the database with new password
   */
  changePassword() {
    this.profileService.updateUserPassword(this.user).subscribe((response) => {
      console.log(response);
    });
    this.dialog.close();
  }
}
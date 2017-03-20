import { Component } from "@angular/core";
import { ChangePasswordModel } from "../password.model";
import { PasswordService } from "../change-password.service";
import { MdDialogRef } from '@angular/material';

@Component({
  moduleId: module.id,
  selector: 'change-password-dialog',
  templateUrl: "change-password-dialog.html"
})
export class ChangePasswordDialogComponent {
  constructor(private passwordService: PasswordService, public dialog: MdDialogRef<any>) { }
  user: ChangePasswordModel = new ChangePasswordModel();

  /**
   * calls the server to update the database with new password
   */
  changePassword() {
    this.passwordService.updateUserPassword(this.user).subscribe((response) => {
      console.log(response);
    });
    this.dialog.close();
  }
}

import { Component } from '@angular/core';
import { ChangePasswordModel } from '../password.model';
import { MdDialogRef } from '@angular/material';
import { ProfileService } from '../profile.service';
import { MdSnackBar } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'change-password-dialog',
    templateUrl: 'change-password-dialog.html'
})
export class ChangePasswordDialogComponent {
    constructor(public profileService: ProfileService, public dialog: MdDialogRef<any>, public snackBar: MdSnackBar) { }
    user: ChangePasswordModel = new ChangePasswordModel();
    isRequired: boolean = true;
    isMessageMismatch: boolean = true;
    requiredMessage: string;
    messageMismatch: string;

    /**
     * update the database with new password
     * @param userPassword of type ChangePasswordModel which has the new password of the user
     */
    changePassword(userPassword: ChangePasswordModel) {
        if (userPassword.oldPassword !== null && userPassword.oldPassword !== ' ' && userPassword.oldPassword !== undefined && userPassword.newPassword !== null && userPassword.newPassword !== ' ' && userPassword.newPassword !== undefined && userPassword.confirmPassword !== null && userPassword.confirmPassword !== ' ' && userPassword.confirmPassword !== undefined) {
            if (userPassword.newPassword === userPassword.confirmPassword && userPassword.confirmPassword === userPassword.newPassword) {
                this.isRequired = false;
                this.isMessageMismatch = false;
                this.profileService.updateUserPassword(userPassword).subscribe((response) => {
                    console.log(response);
                });
                this.dialog.close();
                let snackBarRef = this.snackBar.open('Your Password has been changed', 'Dismiss', {
                    duration: 3000,
                });
            }
            else {
                this.isMessageMismatch = true;
                this.messageMismatch = 'Your new password n current password do not match';
            }
        }
        else {
            this.isRequired = true;
            this.requiredMessage = 'Please enter the details';
        }
    }
}
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
    message: string;

    /**
     * update the database with new password
     * @param userPassword of type ChangePasswordModel which has the new password of the user
     */
    changePassword(userPassword: ChangePasswordModel) {
        if (userPassword.oldPassword == null && userPassword.newPassword == null && userPassword.confirmPassword == null) {
            this.message = 'Please enter the details';
        }
        else {
            if (userPassword.oldPassword !== null && userPassword.oldPassword !== ' ' && userPassword.oldPassword !== undefined) {
                if (userPassword.newPassword !== null && userPassword.newPassword !== ' ' && userPassword.newPassword !== undefined) {
                    if (userPassword.confirmPassword !== null && userPassword.confirmPassword !== ' ' && userPassword.confirmPassword !== undefined) {
                        if (userPassword.newPassword == userPassword.confirmPassword && userPassword.confirmPassword == userPassword.newPassword) {
                            this.isRequired = false;
                            this.profileService.updateUserPassword(userPassword).subscribe((response) => {
                                console.log(response);
                            });
                            this.dialog.close();
                            let snackBarRef = this.snackBar.open('Your Password has been changed', 'Dismiss', {
                                duration: 3000,
                            });
                        }
                        else {
                            this.isRequired = true;
                            this.message = 'Your new password n current password do not match';
                        }
                    }
                    else {
                        this.isRequired = true;
                        this.message = 'Please retype you new password';
                    }
                }
                else {
                    this.isRequired = true;
                    this.message = 'Please enter new password';
                }
            }
            else {
                this.isRequired = false;
                this.message = 'Current Password is required';
            }
        }

    }
}
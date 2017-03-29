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
    isPasswordSame: boolean;
    response: any;
    errorMesseage: any;
    errorCorrection: boolean = true;

    /**
     * update the database with new password
     * @param userPassword of type ChangePasswordModel which has the new password of the user
     */
    changePassword(userPassword: ChangePasswordModel) {
        if (userPassword.newPassword === userPassword.confirmPassword) {
            this.isPasswordSame = true;
            this.profileService.updateUserPassword(userPassword).subscribe((response) => {
                console.log(response);
                this.dialog.close();
                let snackBarRef = this.snackBar.open('Your Password has been changed', 'Dismiss', {
                    duration: 3000,
                });
            },
                err => {
                    this.errorCorrection = true;
                    this.response = (err.json());
                    this.errorMesseage = this.response['error'][0];
                });
        }
        else {
            this.isPasswordSame = false;
        }
    }
}
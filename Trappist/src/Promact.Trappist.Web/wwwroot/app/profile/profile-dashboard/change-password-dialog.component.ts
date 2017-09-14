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
    isPasswordSame: boolean = true;
    response: any;
    errorMesseage: any;
    errorCorrection: boolean = true;
    loader: boolean;

    /**
     * update the database with new password
     * @param userPassword: Object of type ChangePasswordModel which has the new password of the user
     */
    changePassword(userPassword: ChangePasswordModel) {
        this.loader = true;
        if (userPassword.newPassword === userPassword.confirmPassword) {
            this.isPasswordSame = true;
            this.profileService.updateUserPassword(userPassword).subscribe((response) => {
                this.loader = false;
                this.dialog.close();
                let snackBarRef = this.snackBar.open('Your password has been changed.', 'Dismiss', {
                    duration: 3000,
                });
            },
                err => {
                    this.loader = false;
                    this.errorCorrection = true;
                    this.response = (err.json());
                    this.errorMesseage = this.response['error'][0];
                });
        }
        else {
            this.loader = false;
            this.isPasswordSame = false;
        }
    }

    /**
     * Sets the conditions for checking and showing error message if changed password and confirm password are not same
     */
    changeCurrentPassword() {
        this.isPasswordSame = true;
        this.errorCorrection = false;
    }
}
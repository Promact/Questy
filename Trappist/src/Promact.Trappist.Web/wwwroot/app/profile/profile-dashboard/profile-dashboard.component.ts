import { Component, OnInit, ViewChild } from "@angular/core";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: "profile-dashboard",
    templateUrl: "profile-dashboard.html"
})

export class ProfileDashboardComponent{

    // Open Change Password Dialog
    constructor(public dialog: MdDialog) { }

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

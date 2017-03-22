import { Component, OnInit, ViewChild } from "@angular/core";
import { MdSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ApplicationUser } from "../profile.model";
import { ProfileService } from "../profile.service";

@Component({
    moduleId: module.id,
    selector: "profile-edit",
    templateUrl: "profile-edit.html"
})
export class ProfileEditComponent implements OnInit {
    editUser: ApplicationUser = new ApplicationUser();
    constructor(public profileService: ProfileService, private router: Router, public snackBar: MdSnackBar) { }

    ngOnInit() {
        this.editUserDetails();
    }

    /**
    * get details of the user and display them in the profile edit page so that the user can edit them
    */
    editUserDetails() {
        this.profileService.editUserDetails().subscribe((response) => {
            this.editUser = response;
        });
    }

    /**
    * update the  details of the user
    */
    updateUserDetails() {
        this.profileService.updateUserDetails(this.editUser).subscribe((response) => {
        });

        // Open Snackbar
        let snackBarRef = this.snackBar.open('Saved Changes Successfully', 'Dismiss', {
            duration: 3000,
        });
        snackBarRef.afterDismissed().subscribe(() => {
            this.router.navigate(['/profile']);
        });
    }
}


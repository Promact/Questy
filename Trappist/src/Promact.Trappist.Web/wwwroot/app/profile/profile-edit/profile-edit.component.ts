import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ApplicationUser } from '../profile.model';
import { ProfileService } from '../profile.service';

@Component({
    moduleId: module.id,
    selector: 'profile-edit',
    templateUrl: 'profile-edit.html'
})
export class ProfileEditComponent implements OnInit {
    editUser: ApplicationUser = new ApplicationUser();
    nameLength: boolean = false;
    loader: boolean;
    constructor(public profileService: ProfileService, private router: Router, public snackBar: MdSnackBar) { }

    ngOnInit() {
        this.getUserDetails();
    }

    /**
    * get details of the user and display them in the profile edit page so that the user can edit them
    */
    getUserDetails() {
        this.profileService.getUserDetails().subscribe((response) => {
            this.editUser = response;
        });
    }

    /**
    * update the  details of the user
    */
    updateUserDetails() {
        this.loader = true;
        this.profileService.updateUserDetails(this.editUser).subscribe((response) => {
            this.loader = false;
            // Open Snackbar
            this.snackBar.open('Saved changes successfully.', 'Dismiss', {
                duration: 3000,
            });
            this.router.navigate(['/profile']);
        });
    }
}


import { Component, OnInit, ViewChild } from "@angular/core";
import { Http } from "@angular/http";
import { ApplicationUser } from "../profile.model";
import { ProfileService } from "../profile.service";

@Component({
    moduleId: module.id,
    selector: "profile-edit",
    templateUrl: "profile-edit.html"
})

export class ProfileEditComponent implements OnInit{

  editUser: ApplicationUser = new ApplicationUser();
  constructor(public profileService:ProfileService ) { }

  ngOnInit() {
    this.editUserDetails();
  }

  /**
  * get details of the user so that the user can edit the details
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
  }

}


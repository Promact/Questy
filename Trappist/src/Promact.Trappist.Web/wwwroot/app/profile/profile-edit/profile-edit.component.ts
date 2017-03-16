import { Component, OnInit, ViewChild } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    moduleId: module.id,
    selector: "profile-edit",
    templateUrl: "profile-edit.html"
})

export class ProfileEditComponent implements OnInit{

  editUser: ApplicationUser = new ApplicationUser;
  constructor(public http: Http) { }

  ngOnInit() {
    this.EditUserDetails();
  }

  EditUserDetails() {
    this.http.get("api/Profile").subscribe((response) => {
      this.editUser = response.json();
      console.log(this.editUser);
    });
    }

  //Update User's detailsin the database
  UpdateUserDetails() {
    this.http.put("api/Profile/" + this.editUser.Name, this.editUser).subscribe((response) => {
      response.json();
    });
  }

}

class ApplicationUser {
  Name: string;
  OrganizationName: string;
  Email: string;
  PhoneNumber: string;
}

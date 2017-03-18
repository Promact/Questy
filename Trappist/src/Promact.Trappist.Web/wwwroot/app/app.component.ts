import { Component } from '@angular/core';
import { Http } from "@angular/http";
import { ProfileService } from "./profile/profile.service";
import { ApplicationUser } from "./profile/profile.model";
@Component({
  moduleId: module.id,
  selector: 'app',
  templateUrl: 'app.html',
})
export class AppComponent {

  name = 'Angular';
  user: ApplicationUser = new ApplicationUser();
  constructor(private profileService: ProfileService) {
  }
  logOut() {
    this.profileService.logOut().subscribe((response) => { });
  }
}
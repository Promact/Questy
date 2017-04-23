import { Component } from '@angular/core';
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

  /**
   * user is logged out and redirected to login page
   */
  logOff() {
    let logoutForm = <HTMLFormElement>document.getElementById("logoutForm");
    logoutForm.submit();
  }

}
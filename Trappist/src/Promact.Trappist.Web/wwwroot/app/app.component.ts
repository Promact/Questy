import { Component } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'app',
  templateUrl: 'app.html',
})
export class AppComponent {
  name = 'Angular';
  constructor() {
  }

  /**
   * user is logged out and redirected to login page
   */
  logOff() {
    let logoutForm = <HTMLFormElement>document.getElementById("logoutForm");
    logoutForm.submit();
  }

}
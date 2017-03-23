import { Component } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'setup',
  templateUrl: 'setup.html',
})
export class SetupComponent {

  constructor() {
  }

  navigateToLogin() {
    window.location.href = '/login';
  }

  //Wizard Step Events
  nextStep2(setup: any) {
    setup.next();
  }

  nextStep3(setup: any) {
    setup.next();
  }

  previousStep1(setup: any) {
    setup.previous();
  }

  previousStep2(setup: any) {
    setup.previous();
  }

  finish(setup: any) {
    setup.complete();
  }
}
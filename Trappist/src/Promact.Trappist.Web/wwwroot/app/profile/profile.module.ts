import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { profileRouting } from "./profile.routing";
import { ProfileComponent } from "./profile.component";
import { ProfileDashboardComponent } from "./profile-dashboard/profile-dashboard.component";
import { ChangePasswordDialogComponent } from "./profile-dashboard/change-password-dialog.component";
import { ProfileEditComponent } from "./profile-edit/profile-edit.component";

@NgModule({
  imports: [
    SharedModule,
    profileRouting
  ],
  declarations: [
    ProfileComponent,
    ProfileDashboardComponent,
    ProfileEditComponent,
    ChangePasswordDialogComponent
  ],
  entryComponents: [
    ChangePasswordDialogComponent
  ],
  providers: [

  ]
})
export class ProfileModule { }

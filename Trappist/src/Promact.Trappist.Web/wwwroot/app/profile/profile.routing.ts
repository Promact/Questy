import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from "./profile.component";
import { ProfileDashboardComponent } from './profile-dashboard/profile-dashboard.component';
import { ProfileEditComponent } from './profile-edit/profile-edit.component';

const profileRoutes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    children: [
      { path: '', component: ProfileDashboardComponent },
      { path: 'edit', component: ProfileEditComponent }
    ]
  }
];

export const profileRouting: ModuleWithProviders = RouterModule.forChild(profileRoutes);

import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SetupComponent } from './setup.component';
const appRoutes: Routes = [
    {
        path: 'setup',
        component: SetupComponent,
        pathMatch: 'full'
    }
];

export const setupRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes);

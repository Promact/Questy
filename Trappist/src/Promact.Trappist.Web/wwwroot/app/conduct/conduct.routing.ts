import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const conductRoutes: Routes = [
    {
        path: '',
        redirectTo: '/register',
        pathMatch: 'full'
    }
];

export const conductRouting: ModuleWithProviders = RouterModule.forRoot(conductRoutes);

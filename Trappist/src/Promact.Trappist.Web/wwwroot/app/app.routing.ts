import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const appRoutes: Routes = [
    {
        path: '',
        redirectTo: '/tests',
        pathMatch: 'full'
    },
    {
        path: '404',
        component: PageNotFoundComponent
    },
    {
        path: '**',
        redirectTo: '404'
    }
];

export const appRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes);

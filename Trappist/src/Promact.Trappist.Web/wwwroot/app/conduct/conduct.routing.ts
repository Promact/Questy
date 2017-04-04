import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { InstructionsComponent } from './instructions/instructions.component';

const conductRoutes: Routes = [
    {
        path: '',
        redirectTo: '/register',
        pathMatch: 'full'
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'instructions',
        component: InstructionsComponent
    }
];

export const conductRouting: ModuleWithProviders = RouterModule.forRoot(conductRoutes);

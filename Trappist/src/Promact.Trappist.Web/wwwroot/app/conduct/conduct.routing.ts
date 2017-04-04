import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { InstructionsComponent } from './instructions/instructions.component';
import { TestSummaryComponent } from './test-summary/test-summary.component';
import { TestEndComponent } from './test-end/test-end.component';

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
    },
    {
        path: 'test-summary',
        component: TestSummaryComponent
    },
    {
        path: 'test-end',
        component: TestEndComponent
    }
];

export const conductRouting: ModuleWithProviders = RouterModule.forRoot(conductRoutes);

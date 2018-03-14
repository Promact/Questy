import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { InstructionsComponent } from './instructions/instructions.component';
import { TestSummaryComponent } from './test-summary/test-summary.component';
import { TestEndComponent } from './test-end/test-end.component';
import { TestComponent } from './test/test.component';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';

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
    },
    {
        path: 'test',
        component: TestComponent
    },
    {
        path: 'test-end-block',
        component: TestEndComponent
    }, {
        path: '**',
        redirectTo:'/404'
    }, {
        path: '404',
        component:PageNotFoundComponent
    }
];

export const conductRouting: ModuleWithProviders = RouterModule.forRoot(conductRoutes);

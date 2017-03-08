import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent } from './tests-dashboard/tests-dashboard.component';

const testsRoutes: Routes = [
    {
        path: 'tests',
        component: TestsComponent,
        children: [
            { path: '', component: TestsDashboardComponent }
        ]
    }
];

export const testsRouting: ModuleWithProviders = RouterModule.forChild(testsRoutes);

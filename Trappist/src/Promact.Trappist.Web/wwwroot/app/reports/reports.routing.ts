import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportsComponent } from './reports.component';
import { TestReportComponent } from './test-report/test-report.component';
import { IndividualReportComponent } from './individual-report/individual-report.component';

const reportsRoutes: Routes = [
    {
        path: 'reports',
        component: ReportsComponent,
        children: [
            { path: 'test/:id', component: TestReportComponent },
            { path: 'individual-report', component: IndividualReportComponent }
        ]
    }
];

export const reportsRouting: ModuleWithProviders = RouterModule.forChild(reportsRoutes);

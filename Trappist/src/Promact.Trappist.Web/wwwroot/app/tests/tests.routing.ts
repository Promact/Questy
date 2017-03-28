import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TestsComponent } from './tests.component';
import { TestsDashboardComponent } from './tests-dashboard/tests-dashboard.component';
import { TestSettingsComponent } from './test-settings/test-settings.component';
import { TestSectionsComponent } from './test-sections/test-sections.component';
import { TestQuestionsComponent } from './test-questions/test-questions.component';

const testsRoutes: Routes = [
  {
    path: 'tests',
    component: TestsComponent,
    children: [
      { path: '', component: TestsDashboardComponent },
      { path: 'settings/:id', component: TestSettingsComponent },
      { path: 'sections', component: TestSectionsComponent },
      { path: 'questions', component: TestQuestionsComponent }
    ]
  }
];

export const testsRouting: ModuleWithProviders = RouterModule.forChild(testsRoutes);

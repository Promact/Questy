import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TestsComponent } from './tests.component';
import { TestsDashboardComponent } from './tests-dashboard/tests-dashboard.component';
import { TestSettingsComponent } from './test-settings/test-settings.component';
import { TestSectionsComponent } from './test-sections/test-sections.component';
import { TestQuestionsComponent } from './test-questions/test-questions.component';
import { TestViewComponent } from './test-view/test-view.component';

const testsRoutes: Routes = [
  {
    path: 'tests',
    component: TestsComponent,
    children: [
      { path: '', component: TestsDashboardComponent },
      { path: ':id/settings', component: TestSettingsComponent },
      { path: 'sections/:id', component: TestSectionsComponent },
      { path: 'questions', component: TestQuestionsComponent },
      { path: 'view', component: TestViewComponent }
    ]
  }
];

export const testsRouting: ModuleWithProviders = RouterModule.forChild(testsRoutes);

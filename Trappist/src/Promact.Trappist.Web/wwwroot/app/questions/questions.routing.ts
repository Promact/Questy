import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';

const questionsRoutes: Routes = [
    {
        path: 'questions',
        component: QuestionsComponent,
        children: [
            { path: '', component: QuestionsDashboardComponent }
        ]
    }
];

export const questionsRouting: ModuleWithProviders = RouterModule.forChild(questionsRoutes);
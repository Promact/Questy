import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';
import { QuestionsSingleAnswerComponent } from './questions-single-answer/questions-single-answer.component';
import { QuestionsMultipleAnswersComponent } from './questions-multiple-answers/questions-multiple-answers.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';

const questionsRoutes: Routes = [
    {
        path: 'questions',
        component: QuestionsComponent,
        children: [
            { path: '', component: QuestionsDashboardComponent },
            { path: 'single-answer', component: QuestionsSingleAnswerComponent },
            { path: 'multiple-answers', component: QuestionsMultipleAnswersComponent },
            { path: 'programming', component: QuestionsProgrammingComponent }
        ]
    }
];

export const questionsRouting: ModuleWithProviders = RouterModule.forChild(questionsRoutes);

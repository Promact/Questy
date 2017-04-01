import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionsComponent } from './questions.component';
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';
import { QuestionsSingleMultipleAnswerComponent } from './questions-single-multiple-answer/questions-single-multiple-answer.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';

const questionsRoutes: Routes = [
    {
        path: 'questions',
        component: QuestionsComponent,
        children: [
            { path: '', component: QuestionsDashboardComponent },
            { path: 'single-answer', component: QuestionsSingleMultipleAnswerComponent },
            { path: 'multiple-answers', component: QuestionsSingleMultipleAnswerComponent },
            { path: 'programming', component: QuestionsProgrammingComponent }
        ]
    }
];

export const questionsRouting: ModuleWithProviders = RouterModule.forChild(questionsRoutes);

import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionsComponent } from './questions.component';
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';
import { SingleMultipleAnswerQuestionComponent } from './questions-single-multiple-answer/questions-single-multiple-answer.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';

const questionsRoutes: Routes = [
    {
        path: 'questions',
        component: QuestionsComponent,
        children: [
            { path: '', component: QuestionsDashboardComponent },
            { path: 'single-answer', component: SingleMultipleAnswerQuestionComponent },
            { path: 'edit-single-answer/:id', component: SingleMultipleAnswerQuestionComponent },
            { path: 'multiple-answers', component: SingleMultipleAnswerQuestionComponent },
            { path: 'edit-multiple-answers/:id', component: SingleMultipleAnswerQuestionComponent },
            { path: 'programming', component: QuestionsProgrammingComponent }
        ]
    }
];

export const questionsRouting: ModuleWithProviders = RouterModule.forChild(questionsRoutes);

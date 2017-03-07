import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent } from "./questions-dashboard/questions-dashboard.component";
import { QuestionsService } from "./questions.service";

@NgModule({
    imports: [
        SharedModule,
        questionsRouting
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent
    ],
    providers: [
        QuestionsService
    ]
})
export class QuestionsModule { }
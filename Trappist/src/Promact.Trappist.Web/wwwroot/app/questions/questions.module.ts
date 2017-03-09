import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent, AddCategoryDialogComponent } from "./questions-dashboard/questions-dashboard.component";
import { QuestionsService } from "./questions.service";

@NgModule({
    imports: [
        SharedModule,
        questionsRouting
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent,
        AddCategoryDialogComponent
    ],
    entryComponents: [
        AddCategoryDialogComponent
    ],
    providers: [
        QuestionsService
    ]
})
export class QuestionsModule { }

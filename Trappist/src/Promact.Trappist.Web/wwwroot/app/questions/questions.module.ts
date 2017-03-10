import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent, AddCategoryDialogComponent } from "./questions-dashboard/questions-dashboard.component";
import { QuestionsSingleAnswerComponent } from "./questions-single-answer/questions-single-answer.component";
import { QuestionsMultipleAnswersComponent } from './questions-multiple-answers/questions-multiple-answers.component';
import { QuestionsService } from "./questions.service";

@NgModule({
    imports: [
        SharedModule,
        questionsRouting
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent,
        AddCategoryDialogComponent,
        QuestionsSingleAnswerComponent,
        QuestionsMultipleAnswersComponent
    ],
    entryComponents: [
        AddCategoryDialogComponent
    ],
    providers: [
        QuestionsService
    ]
})
export class QuestionsModule { }

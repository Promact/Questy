import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent, AddCategoryDialogComponent, DeleteCategoryDialogComponent, EditCategoryDialogComponent } from "./questions-dashboard/questions-dashboard.component";
import { QuestionsSingleAnswerComponent } from "./questions-single-answer/questions-single-answer.component";
import { QuestionsMultipleAnswersComponent } from './questions-multiple-answers/questions-multiple-answers.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';
import { QuestionsService } from "./questions.service";
import { CategoryService } from "./categories.service";

@NgModule({
    imports: [
        SharedModule,
        questionsRouting
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent,
        AddCategoryDialogComponent,
        EditCategoryDialogComponent,
        QuestionsSingleAnswerComponent,
        QuestionsMultipleAnswersComponent,
        QuestionsProgrammingComponent,
        DeleteCategoryDialogComponent
    ],
    entryComponents: [
        AddCategoryDialogComponent,
        EditCategoryDialogComponent,
        DeleteCategoryDialogComponent
    ],
    providers: [
        QuestionsService,
        CategoryService
    ]
})
export class QuestionsModule { }

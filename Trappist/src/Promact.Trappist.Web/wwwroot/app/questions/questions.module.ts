﻿import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent, AddCategoryDialogComponent, DeleteCategoryDialogComponent } from "./questions-dashboard/questions-dashboard.component";
import { RenameCategoryDialogComponent } from "./questions-dashboard/rename-category-dialog.component";
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
        RenameCategoryDialogComponent,
        QuestionsSingleAnswerComponent,
        QuestionsMultipleAnswersComponent,
        QuestionsProgrammingComponent,
        DeleteCategoryDialogComponent
    ],
    entryComponents: [
        AddCategoryDialogComponent,
        RenameCategoryDialogComponent,
        DeleteCategoryDialogComponent
    ],
    providers: [
        QuestionsService,
        CategoryService
    ]
})
export class QuestionsModule { }

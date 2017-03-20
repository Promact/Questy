import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { questionsRouting } from "./questions.routing";
import { QuestionsComponent } from "./questions.component";
import { QuestionsDashboardComponent } from "./questions-dashboard/questions-dashboard.component";
import { AddCategoryDialogComponent } from "./questions-dashboard/add-category-dialog.component";
import { DeleteCategoryDialogComponent } from "./questions-dashboard/delete-category-dialog.component";
import { DeleteQuestionDialogComponent } from "./questions-dashboard/delete-question-dialog.component";
import { QuestionsSingleAnswerComponent } from "./questions-single-answer/questions-single-answer.component";
import { QuestionsMultipleAnswersComponent } from './questions-multiple-answers/questions-multiple-answers.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';
import { QuestionsService } from "./questions.service";
import { CategoryService } from "./categories.service";
@NgModule({
    imports: [
        SharedModule,
        questionsRouting,
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent,
        AddCategoryDialogComponent,
        QuestionsSingleAnswerComponent,
        QuestionsMultipleAnswersComponent,
        QuestionsProgrammingComponent,
        DeleteCategoryDialogComponent,
        DeleteQuestionDialogComponent
    ],
    entryComponents: [
      AddCategoryDialogComponent,
      DeleteCategoryDialogComponent,
      DeleteQuestionDialogComponent
    ],
    providers: [
        QuestionsService,
		CategoryService
    ]
})
export class QuestionsModule { }
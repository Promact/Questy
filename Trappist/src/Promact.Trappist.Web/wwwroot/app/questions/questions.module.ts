import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { questionsRouting } from './questions.routing';
import { QuestionsComponent } from './questions.component';
import { UpdateCategoryDialogComponent } from './questions-dashboard/update-category-dialog.component';
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';
import { AddCategoryDialogComponent } from './questions-dashboard/add-category-dialog.component';
import { DeleteCategoryDialogComponent } from './questions-dashboard/delete-category-dialog.component';
import { DeleteQuestionDialogComponent } from './questions-dashboard/delete-question-dialog.component';
import { SingleMultipleAnswerQuestionComponent } from './questions-single-multiple-answer/questions-single-multiple-answer.component';
import { QuestionsProgrammingComponent } from './questions-programming/questions-programming.component';
import { QuestionsService } from './questions.service';
import { CategoryService } from './categories.service';
import { InfiniteScrollModule } from 'angular2-infinite-scroll';
import { TinymceModule } from 'angular2-tinymce';
import { AceEditorModule } from 'ng2-ace-editor';

@NgModule({
    imports: [
        SharedModule,
        questionsRouting,
        InfiniteScrollModule,
        TinymceModule.withConfig({}),
        AceEditorModule
    ],
    declarations: [
        QuestionsComponent,
        QuestionsDashboardComponent,
        AddCategoryDialogComponent,
        SingleMultipleAnswerQuestionComponent,
        UpdateCategoryDialogComponent,
        QuestionsProgrammingComponent,
        DeleteCategoryDialogComponent,
        DeleteQuestionDialogComponent
    ],
    entryComponents: [
        AddCategoryDialogComponent,
        DeleteCategoryDialogComponent,
        DeleteQuestionDialogComponent,
        UpdateCategoryDialogComponent
    ],
    providers: [
        QuestionsService,
        CategoryService
    ]
})
export class QuestionsModule { }
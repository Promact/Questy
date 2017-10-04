import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { testsRouting } from './tests.routing';

import { TestsComponent } from './tests.component';
import { TestsDashboardComponent } from './tests-dashboard/tests-dashboard.component';
import { TestCreateDialogComponent } from './tests-dashboard/test-create-dialog.component';
import { DeleteTestDialogComponent } from './tests-dashboard/delete-test-dialog.component';
import { TestSettingsComponent } from './test-settings/test-settings.component';
import { RandomQuestionSelectionDialogComponent } from './test-settings/test-launch-dialog.component';
import { TestSectionsComponent } from './test-sections/test-sections.component';
import { TestService } from './tests.service';
import { FilterPipe } from './tests-dashboard/test-dashboard.pipe';
import { TestQuestionsComponent } from './test-questions/test-questions.component';
import { TestViewComponent } from './test-view/test-view.component';
import { Test } from './tests.model';
import { TestPreviewComponent } from './test-preview/test-preview.compponent';
import { CreateTestHeaderComponent } from './shared/create-test-header/create-test-header.component';
import { CreateTestFooterComponent } from './shared/create-test-footer/create-test-footer.component';

import { DuplicateTestDialogComponent } from './tests-dashboard/duplicate-test-dialog.component';
import { DeselectCategoryComponent } from './test-sections/deselect-category.component';
import { IncompleteTestCreationDialogComponent } from './test-settings/incomplete-test-creation-dialog.component';

import { ConductService } from '../conduct/conduct.service';
import { ConnectionService } from '../core/connection.service';


@NgModule({
    imports: [
        SharedModule,
        testsRouting,
        BrowserModule,
       
    ],
    declarations: [
        TestsComponent,
        TestsDashboardComponent,
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        DuplicateTestDialogComponent,
        TestSettingsComponent,
        RandomQuestionSelectionDialogComponent,
        TestSectionsComponent,
        TestQuestionsComponent,
        TestViewComponent,
        CreateTestHeaderComponent,
        CreateTestFooterComponent,
        FilterPipe,
        DeselectCategoryComponent,
        IncompleteTestCreationDialogComponent,
        TestPreviewComponent

    ],
    entryComponents: [
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        RandomQuestionSelectionDialogComponent,
        DeselectCategoryComponent,
        IncompleteTestCreationDialogComponent,
        DuplicateTestDialogComponent
        
    ],
    providers: [
        TestService,
        Test,
        ConductService,
        ConnectionService
    ]
})
export class TestsModule { }

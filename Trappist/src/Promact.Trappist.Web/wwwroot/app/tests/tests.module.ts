import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { testsRouting } from './tests.routing';
import { TestsComponent } from './tests.component';
import { TestsDashboardComponent } from './tests-dashboard/tests-dashboard.component';
import { TestCreateDialogComponent } from './tests-dashboard/test-create-dialog.component';
import { DeleteTestDialogComponent } from './tests-dashboard/delete-test-dialog.component';
import { TestSettingsComponent } from './test-settings/test-settings.component';
import { TestLaunchDialogComponent } from './test-settings/test-launch-dialog.component';
import { TestSectionsComponent } from './test-sections/test-sections.component';
import { TestService } from './tests.service';
import { FilterPipe } from './tests-dashboard/test-dashboard.pipe';
import { TestQuestionsComponent } from './test-questions/test-questions.component';
import { TestViewComponent } from './test-view/test-view.component';
import { ClipboardModule } from 'ngx-clipboard';
import { Test } from './tests.model';
import { CreateTestHeaderComponent } from './shared/create-test-header/create-test-header.component';
import { CreateTestFooterComponent } from './shared/create-test-footer/create-test-footer.component';
import { DeselectCategoryComponent } from './test-sections/deselect-category.component';
@NgModule({
    imports: [
        SharedModule,
        testsRouting,
        ClipboardModule       
    ],
    declarations: [
        TestsComponent,
        TestsDashboardComponent,
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        TestSettingsComponent,
        TestLaunchDialogComponent,
        TestSectionsComponent,
        TestQuestionsComponent,
        TestViewComponent,
        CreateTestHeaderComponent,
        CreateTestFooterComponent,
        FilterPipe,
        DeselectCategoryComponent

    ],
    entryComponents: [
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        TestLaunchDialogComponent,
        DeselectCategoryComponent
    ],
    providers: [
        TestService,
        Test
    ]
})
export class TestsModule { }

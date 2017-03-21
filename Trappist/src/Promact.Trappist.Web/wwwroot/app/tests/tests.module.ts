import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { testsRouting } from "./tests.routing";
import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent } from "./tests-dashboard/tests-dashboard.component";
import { TestCreateDialogComponent } from "./tests-dashboard/test-create-dialog.component";
import { DeleteTestDialogComponent } from "./tests-dashboard/delete-test-dialog.component";
import { TestSettingsComponent } from "./test-settings/test-settings.component";
import { TestLaunchDialogComponent } from "./test-settings/test-launch-dialog.component";
import { TestSectionsComponent } from './test-sections/test-sections.component';
import { TestService } from "./tests.service";
import { FilterPipe } from "./tests-dashboard/test-dashboard.pipe";
import { TestSettingService } from "./testsetting.service";
import { ClipboardModule } from 'ngx-clipboard';

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
        FilterPipe

    ],
    entryComponents: [
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        TestLaunchDialogComponent
    ],
    providers: [
        TestService,
        TestSettingService
    ]
})
export class TestsModule { }

import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { testsRouting } from "./tests.routing";
import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent, TestCreateDialogComponent } from "./tests-dashboard/tests-dashboard.component";
import { TestSettingsComponent, TestLaunchDialogComponent } from "./test-settings/test-settings.component";
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
        TestSettingsComponent,
        TestLaunchDialogComponent,
        FilterPipe

    ],
    entryComponents: [
        TestCreateDialogComponent,
        TestLaunchDialogComponent
    ],
    providers: [
        TestService,
        TestSettingService
    ]
})
export class TestsModule { }
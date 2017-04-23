import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { testsRouting } from "./tests.routing";
import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent } from "./tests-dashboard/tests-dashboard.component";
import { TestCreateDialogComponent } from "./tests-dashboard/test-create-dialog.component";
import { DeleteTestDialogComponent } from "./tests-dashboard/delete-test-dialog.component";
import { TestSettingsComponent } from "./test-settings/test-settings.component";
import { TestLaunchDialogComponent } from "./test-settings/test-launch-dialog.component";
import { TestService } from "./tests.service";
import { FilterPipe } from "./tests-dashboard/test-dashboard.pipe";

@NgModule({
    imports: [
        SharedModule,
        testsRouting
    ],
    declarations: [
        TestsComponent,
        TestsDashboardComponent,
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        TestSettingsComponent,
        TestLaunchDialogComponent,
        FilterPipe

    ],
    entryComponents: [
        TestCreateDialogComponent,
        DeleteTestDialogComponent,
        TestLaunchDialogComponent
    ],
    providers: [
        TestService
    ]
})
export class TestsModule { }

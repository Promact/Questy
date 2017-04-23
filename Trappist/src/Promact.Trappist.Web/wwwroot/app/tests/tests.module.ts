import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { testsRouting } from "./tests.routing";
import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent, TestCreateDialogComponent } from "./tests-dashboard/tests-dashboard.component";
import { TestSettingsComponent, TestLaunchDialogComponent } from "./test-settings/test-settings.component";
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
        FilterPipe
        
    ]
})
export class TestsModule { }

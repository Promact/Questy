﻿import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { testsRouting } from "./tests.routing";
import { TestsComponent } from "./tests.component";
import { TestsDashboardComponent, TestCreateDialogComponent } from "./tests-dashboard/tests-dashboard.component";

@NgModule({
    imports: [
        SharedModule,
        testsRouting
    ],
    declarations: [
        TestsComponent,
        TestsDashboardComponent,
        TestCreateDialogComponent
    ],
    entryComponents: [
        TestCreateDialogComponent
    ],
    providers: [
        
    ]
})
export class TestsModule { }

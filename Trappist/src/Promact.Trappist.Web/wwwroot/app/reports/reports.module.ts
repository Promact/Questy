import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { reportsRouting } from './reports.routing';
import { ReportsComponent } from './reports.component';
import { TestReportComponent } from './test-report/test-report.component';
import { IndividualReportComponent } from './individual-report/individual-report.component';
import { ReportService } from './report.service';
import { ConductService } from '../conduct/conduct.service';
import { ConnectionService } from '../core/connection.service';

@NgModule({
    imports: [
        SharedModule,
        reportsRouting
    ],
    declarations: [
        ReportsComponent,
        TestReportComponent,
        IndividualReportComponent
    ],
    providers: [
        ReportService,
        ConductService,
        ConnectionService
    ]
})
export class ReportsModule { }

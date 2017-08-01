import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { conductRouting } from './conduct.routing';
import { ConductComponent } from './conduct.component';
import { RegisterComponent } from './register/register.component';
import { InstructionsComponent } from './instructions/instructions.component';
import { TestSummaryComponent } from './test-summary/test-summary.component';
import { TestEndComponent } from './test-end/test-end.component';
import { TestConductHeaderComponent } from './shared/test-conduct-header/test-conduct-header.component';
import { TestConductFooterComponent } from './shared/test-conduct-footer/test-conduct-footer.component';
import { ConductService } from './conduct.service';
import { TestComponent } from './test/test.component';
import { AceEditorModule } from 'ng2-ace-editor';


@NgModule({
    bootstrap: [ConductComponent],
    imports: [
        BrowserModule,
        conductRouting,
        SharedModule,
        CoreModule,
        AceEditorModule

    ],
    providers: [
        ConductService
    ],
    declarations: [
        ConductComponent,
        RegisterComponent,
        InstructionsComponent,
        TestSummaryComponent,
        TestEndComponent,
        TestConductHeaderComponent,
        TestConductFooterComponent,
        TestComponent,
    ]
})
export class ConductModule { }

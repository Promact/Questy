import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import 'rxjs/Rx';
import { FormWizardModule } from 'angular2-wizard';
import { SetupComponent } from './setup.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { SetupService } from './setup.service';
@NgModule({
    bootstrap: [SetupComponent],
    imports: [
        BrowserModule,
        SharedModule,
        CoreModule,
        FormWizardModule    
    ],
    providers: [
        SetupService
    ],
    declarations: [
        SetupComponent
    ]
})
export class SetupModule { }

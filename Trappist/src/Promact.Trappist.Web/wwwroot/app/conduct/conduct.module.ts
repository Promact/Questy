import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import 'rxjs/Rx';
import { ConductComponent } from './conduct.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';

@NgModule({
    bootstrap: [ConductComponent],
    imports: [
        BrowserModule,
        SharedModule,
        CoreModule
    ],
    providers: [

    ],
    declarations: [
        ConductComponent
    ]
})
export class ConductModule { }

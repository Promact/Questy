﻿import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import 'rxjs/Rx';

import { appRouting } from './app.routing';
import { AppComponent } from './app.component';

import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { QuestionsModule } from './questions/questions.module';
import { TestsModule } from './tests/tests.module';
import { ProfileModule } from './profile/profile.module';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        appRouting,
        SharedModule,
        CoreModule,
        QuestionsModule,
        TestsModule,
        ProfileModule
    ],
    providers: [

    ],

    declarations: [
        AppComponent
    ]
})
export class AppModule { }

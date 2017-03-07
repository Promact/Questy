import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from "@angular/router";

import 'rxjs/Rx';

import { appRouting } from "./app.routing";
import { AppComponent } from './app.component';

import { SharedModule } from "./shared/shared.module";
import { QuestionsModule } from "./questions/questions.module";

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        appRouting,
        SharedModule,
        QuestionsModule
    ],
    providers: [

    ],

    declarations: [
        AppComponent
    ]
})
export class AppModule { }
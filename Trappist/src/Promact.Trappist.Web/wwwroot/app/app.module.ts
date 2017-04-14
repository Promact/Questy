import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import 'rxjs/Rx';

import { appRouting } from './app.routing';
import { AppComponent } from './app.component';
import { CKEditorModule } from 'ng2-ckeditor';

import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { QuestionsModule } from './questions/questions.module';
import { TestsModule } from './tests/tests.module';
import { ProfileModule } from './profile/profile.module';
import { ReportsModule } from './reports/reports.module';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        appRouting,
        SharedModule,
        CoreModule,
        QuestionsModule,
        TestsModule,
        ProfileModule,
        FormsModule,
        CKEditorModule,
        ReportsModule
    ],
    providers: [

    ],

    declarations: [
        AppComponent
    ]
})
export class AppModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
//import { Http, ConnectionBackend, XHRBackend } from '@angular/http';

import { appRouting } from './app.routing';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { QuestionsModule } from './questions/questions.module';
import { TestsModule } from './tests/tests.module';
import { ProfileModule } from './profile/profile.module';
import { ReportsModule } from './reports/reports.module';
//import { HttpWrapper } from './core/http.wrapper';


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
        ReportsModule
       
    ],
    providers: [
        //{
        //    provide: Http,
        //    useClass: HttpWrapper
        //},
        //{
        //    provide: ConnectionBackend,
        //    useClass: XHRBackend
        //}
    ],

    declarations: [
        AppComponent
    ]
})
export class AppModule { }

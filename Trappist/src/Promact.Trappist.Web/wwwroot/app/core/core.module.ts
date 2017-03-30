import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { HttpService } from './http.service';

@NgModule({
    imports: [
        HttpModule
    ],
    declarations: [],
    exports: [],
    providers: [
        HttpService
    ]
})
export class CoreModule {

}
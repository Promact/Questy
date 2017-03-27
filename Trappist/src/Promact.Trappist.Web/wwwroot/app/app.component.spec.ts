declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from '@angular/core';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AppModule } from './app.module';
import { Http } from '@angular/http';

class MockHttp { }
describe('User Add Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [AppModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Http, useClass: MockHttp },
             ]
        }).compileComponents();
    }));

    it('Load app Component', () => {
        let fixture = TestBed.createComponent(AppComponent);
        let comp = fixture.componentInstance;
    });
});


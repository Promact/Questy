
import {throwError as observableThrowError, of as observableOf,  Observable } from 'rxjs';
import { TestBed, async, ComponentFixture, tick, fakeAsync } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { TestConductHeaderComponent } from '../shared/test-conduct-header/test-conduct-header.component';
import { TestConductFooterComponent } from '../shared/test-conduct-footer/test-conduct-footer.component';
import { CoreModule } from '../../core/core.module';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ConductService } from '../conduct.service';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { MaterialModule } from '@angular/material';
import { TestAttendees } from '../register/register.model';
import { ResponseOptions } from '@angular/http';
import { ConnectionService } from '../../core/connection.service';

class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate() { return true; }
    isActive() { return true; }
}

class Error {
    status = 404;
}

describe('Testing of conduct-register component:-', () => {
    let fixture: ComponentFixture<RegisterComponent>;
    let registerComponent: RegisterComponent;
    let router: Router;
    let urls: any[];
    let testLink = '1Pu48OQy6d';
    let testAttendee = new TestAttendees();

    testAttendee.email = 'Suparna@promactinfo.com';
    testAttendee.firstName = 'suparna';
    testAttendee.lastName = 'acharya';
    testAttendee.rollNumber = 'cse-055';
    testAttendee.contactNumber = '9874563210';

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                RegisterComponent,
                TestConductHeaderComponent,
                TestConductFooterComponent
            ],
            providers: [
                ConnectionService,
                { provide: Router, useClass: RouterStub },
                { provide: ConductService, useClass: ConductService },
                { provide: ActivatedRoute, useclass: ActivatedRoute }
            ],
            imports: [BrowserModule, FormsModule, MaterialModule, RouterModule, CoreModule]
        }).compileComponents();
    }));
    beforeEach(() => {
        fixture = TestBed.createComponent(RegisterComponent);
        registerComponent = fixture.componentInstance;
        router = TestBed.get(Router);
    });

    it('should register a testattendee for a test and navigate to instructions page', fakeAsync(() => {
        spyOn(ConductService.prototype, 'registerTestAttendee').and.callFake(() => {
            return observableOf(testAttendee);
        });
        spyOn(ConnectionService.prototype, 'sendReport').and.callFake(() => { });
        spyOn(ConnectionService.prototype, 'registerAttendee').and.callFake(() => { });
        spyOn(Router.prototype, 'navigate').and.callFake(function (url: any[]) {
            urls = url;
            expect(urls[0]).toBe('/conduct/' + testLink + '/instructions');
        });

        registerComponent.registerTestAttendee();        
    }));

    it('should throw error message if registration fails', () => {
        spyOn(ConductService.prototype, 'registerTestAttendee').and.callFake(() => {
            return observableThrowError(new Error());
        });
        registerComponent.registerTestAttendee();
        expect(registerComponent.isErrorMessage).toBeTruthy();
    });

    it('should throw different error message if error status is different', () => {
        let error = new Error();
        error.status = 500;
        spyOn(ConductService.prototype, 'registerTestAttendee').and.callFake(() => {
            return observableThrowError(error);
        });
        registerComponent.registerTestAttendee();
        expect(registerComponent.registerTestAttendee).toThrowError();
    });
});
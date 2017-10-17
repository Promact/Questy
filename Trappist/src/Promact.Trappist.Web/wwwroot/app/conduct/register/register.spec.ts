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
import { Observable } from 'rxjs/Rx';

class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate() { return true; }
    isActive() { return true; }
}

describe('testting of conduct-registration:-', () => {
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
            return Observable.of(testAttendee);
        });
        registerComponent.registerTestAttendee();
        spyOn(Router.prototype, 'navigate').and.callFake(function (url: any[]) {
            urls = url;
            expect(urls[0]).toBe('/conduct/' + testLink + '/instructions');
        });
    }));
});
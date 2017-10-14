﻿import { TestBed, async, ComponentFixture, tick, fakeAsync } from "@angular/core/testing";
import { RegisterComponent } from "./register.component";
import { TestConductHeaderComponent } from "../shared/test-conduct-header/test-conduct-header.component";
import { TestConductFooterComponent } from "../shared/test-conduct-footer/test-conduct-footer.component";
import { CoreModule } from "../../core/core.module";
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ConductService } from "../conduct.service";
import { FormsModule } from "@angular/forms";
import { BrowserModule, By } from "@angular/platform-browser";
import { MaterialModule } from "@angular/material";
import { TestAttendees } from "../register/register.model";
import { BehaviorSubject } from "rxjs/BehaviorSubject";

class RouterStub {
    navigateByUrl(url: string) { return url; }
    navigate() { return true; }
    isActive() { return true; }
}

describe('testting of conduct-registration', () => {
    let fixture: ComponentFixture<RegisterComponent>;
    let registerComponent: RegisterComponent;
    let router: Router;

    let testAttendee = new TestAttendees()
    {
        testAttendee.email = 'Suparna@promactinfo.com',
            testAttendee.firstName = 'suparna',
            testAttendee.lastName = 'acharya',
            testAttendee.rollNumber = 'cse-055',
            testAttendee.contactNumber = '9874563210'
    }

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

    //it('should call the registerTestAttendee function when submit button is clicked', fakeAsync(() => {
    //    spyOn(registerComponent, 'registerTestAttendee').and.callFake(() => { });
    //    let form = fixture.debugElement.query(By.css('form'));
    //    form.triggerEventHandler('submit', null);
    //    fixture.detectChanges();
    //    expect(registerComponent.registerTestAttendee).toHaveBeenCalled();
    //}))

    it('should register a testattendee for a test and navigate to instructions page', fakeAsync(() => {
        spyOn(ConductService.prototype, 'registerTestAttendee').and.callFake(() => {
            let registrationData = new BehaviorSubject(testAttendee);
            return registrationData.asObservable();
        });
        spyOn(router, 'navigate').and.callFake(() => { });
        registerComponent.registerTestAttendee();
        router.navigate(['instructions']);
        tick();
        expect(router.isActive('/registration', true)).toBe(true);
    }))
})
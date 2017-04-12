import { Component, OnInit, ViewChild } from '@angular/core';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'test-settings',
    templateUrl: 'test-settings.html'
})

export class TestSettingsComponent implements OnInit {
    testSettings: Test;
    testId: number;
    validEndDate: boolean;
    endDate: string;
    validTime: boolean;
    validStartDate: boolean;
    currentDate: Date;
    editName: boolean;
    testNameUpdatedMessage: string;
    testSettingsUpdatedMessage: string;

    constructor(private testService: TestService, private router: Router, private route: ActivatedRoute) {
        this.testSettings = new Test();
        this.validEndDate = false;
        this.validTime = false;
        this.validStartDate = false;
        this.currentDate = new Date();
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestSettings(id: number) {
        this.testService.getTestSettings(id).subscribe((response) => {
            this.testSettings = (response);
        });
    }

    /**
     * Checks the End Date and Time is valid or not
     * @param endDate contains ths the value of the field End Date and Time
     */
    isEndDateValid(endDate: Date) {
        if (this.testSettings.startDate > endDate) {
            this.validEndDate = true;
            this.validStartDate = false;
        }
        else
            this.validEndDate = false;
    }

    /**
     * Checks whether the Start Date selected is valid or not
     */
    isStartDateValid() {
        if ((new Date(this.testSettings.startDate)) < this.currentDate || this.testSettings.startDate > this.testSettings.endDate) {
            this.validStartDate = true;
            this.validEndDate = false;
        }
        else
            this.validStartDate = false;
    }

    /**
     * Checks whether the Warning Time set is valid
     */
    isWarningTimeValid() {
        this.validTime = this.testSettings.warningTime >= this.testSettings.duration ? true : false;
    }
}

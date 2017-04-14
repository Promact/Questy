import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { DeleteTestDialogComponent } from './delete-test-dialog.component';
import { TestService } from '../tests.service';
import { Test } from '../tests.model';
import { ActivatedRoute, Router, provideRoutes } from '@angular/router';
import { Http } from '@angular/http';
import { TestSettingsComponent } from '../../tests/test-settings/test-settings.component';
import { TestCreateDialogComponent } from './test-create-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'tests-dashboard',
    templateUrl: 'tests-dashboard.html'
})

export class TestsDashboardComponent {
    showSearchInput: boolean;
    Tests: Test[] = new Array<Test>();
    searchTest: string;
    isDeleteAllowed: boolean;

    constructor(public dialog: MdDialog, private testService: TestService) {
        this.getAllTests();
    }
    // get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => { this.Tests = (response); });
    }
    // open Create Test Dialog
    createTestDialog() {
        let dialogRef = this.dialog.open(TestCreateDialogComponent);
        dialogRef.afterClosed().subscribe(test => {
            if (test)
                this.Tests.push(test);
        });
    }

    /**
    * Open Delete Test Dialog
    * @param test: Object of Test class that is to be deleted
    */
    deleteTestDialog(test: Test) {
        // Check if there is any one who is giving the test
        this.testService.isTestAttendeeExist(test.id).subscribe((isTestAttendee) => {
            this.isDeleteAllowed = false;
            let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent).componentInstance;
            deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
        },
            err => {
                this.isDeleteAllowed = true;
                let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent).componentInstance;
                deleteTestDialog.testToDelete = test;
                deleteTestDialog.testArray = this.Tests;
                deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
            });
    }
}

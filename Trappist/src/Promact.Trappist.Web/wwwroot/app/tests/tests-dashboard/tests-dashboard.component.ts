import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog, MdDialogRef } from '@angular/material';
import { DeleteTestDialogComponent } from './delete-test-dialog.component';
import { TestService } from '../tests.service';
import { Test } from '../tests.model';
import { ActivatedRoute, Router, provideRoutes } from '@angular/router';
import { Http } from '@angular/http';
import { TestSettingsComponent } from '../../tests/test-settings/test-settings.component';
import { TestCreateDialogComponent } from './test-create-dialog.component';
import { DuplicateTestDialogComponent } from './duplicate-test-dialog.component';

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
    loader: boolean;
    test: Test;

    constructor(public dialog: MdDialog, private testService: TestService, private router:Router) {
        this.loader = true;
        this.getAllTests();
    }
    // get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => {
            this.Tests = (response);
            this.isTestAttendeeExist();
        });
        this.loader = false;
    }
    // open Create Test Dialog
    createTestDialog() {
        let dialogRef = this.dialog.open(TestCreateDialogComponent);
        dialogRef.afterClosed().subscribe(test => {
            if (test)
                this.Tests.unshift(test);
        });
    }

    /**
    * Open Delete Test Dialog
    * @param test: Object of Test class that is to be deleted
    */
    deleteTestDialog(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((response) => {
            this.isDeleteAllowed = response.response ? false : true;
            let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent).componentInstance;
            deleteTestDialog.testToDelete = test;
            deleteTestDialog.testArray = this.Tests;
            deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
        });
    }

    /**
     * Open duplicate test dialog
     * @param test: an object of Test class
     */
    duplicateTestDialog(test: Test) {
        let newTestObject = (JSON.parse(JSON.stringify(test)));
        let duplicateTestDialog = this.dialog.open(DuplicateTestDialogComponent).componentInstance;
        duplicateTestDialog.testName = newTestObject.testName + '_copy';
        duplicateTestDialog.testArray = this.Tests;
        duplicateTestDialog.testToDuplicate = test;
    }

    /**
     * Checks if any candidate has taken the test
     */
    isTestAttendeeExist() {
        this.Tests.forEach(x => {
            this.testService.isTestAttendeeExist(x.id).subscribe((response) => {
                if (response.response) {
                    x.isEditTestEnable = false;
                }
                else {
                    x.isEditTestEnable = true;
                }
            });
        });
    }

    /**
     * Navigates the user to the select sections page 
     * @param test: an object of Test class
     */
    editTest(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((response) => {
            if (!response.response) {
                this.router.navigate(['/tests/' + test.id + '/sections']);
            }
        });
    }
}

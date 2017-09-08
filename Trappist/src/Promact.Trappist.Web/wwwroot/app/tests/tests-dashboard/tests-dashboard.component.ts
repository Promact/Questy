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
import { QuestionsService } from '../../questions/questions.service';

@Component({
    moduleId: module.id,
    selector: 'tests-dashboard',
    templateUrl: 'tests-dashboard.html'
})

export class TestsDashboardComponent implements OnInit {
    showSearchInput: boolean;
    tests: Test[];
    searchTest: string;
    isDeleteAllowed: boolean;
    loader: boolean;
    count: number;
    duplicatedTestName: string;

    constructor(private questionsService: QuestionsService,public dialog: MdDialog, private testService: TestService, private router: Router) {
        this.tests = new Array<Test>();

    }
    ngOnInit() {
        this.loader = true;
        this.getAllTests();
    }

    // get All The Tests From Server
    getAllTests() {
        this.testService.getTests().subscribe((response) => {
            this.tests = (response);
            this.disableEditForTheTestsIfAttendeesExist();
            this.loader = false;
        });
    }

    // open Create Test Dialog
    createTestDialog() {
        let dialogRef = this.dialog.open(TestCreateDialogComponent, { disableClose: true, hasBackdrop: true });
        dialogRef.afterClosed().subscribe(test => {
            if (test)
                this.tests.unshift(test);
        });
    }

    /**
    * Open Delete Test Dialog
    * @param test: Object of Test class that is to be deleted
    */
    deleteTestDialog(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((res) => {
            this.isDeleteAllowed = res.response ? false : true;
            let deleteTestDialog = this.dialog.open(DeleteTestDialogComponent, { disableClose: true, hasBackdrop: true }).componentInstance;
            deleteTestDialog.testToDelete = test;
            deleteTestDialog.testArray = this.tests;
            deleteTestDialog.isDeleteAllowed = this.isDeleteAllowed;
        });
    }

    /**
     * Open duplicate test dialog
     * @param test: an object of Test class
     */
    duplicateTestDialog(test: Test) {
        test.isPaused = test.isLaunched = false;
        let newTestObject = (JSON.parse(JSON.stringify(test)));
        let duplicateTestDialog = this.dialog.open(DuplicateTestDialogComponent, { disableClose: true, hasBackdrop: true }).componentInstance;
        console.log(newTestObject.id, newTestObject.testName);
        this.testService.setTestCopiedNumber(newTestObject.id, newTestObject.testName).subscribe((response) => {
            console.log(response);
            //this.duplicatedTestName = response;
            //newTestObject.testName = this.duplicatedTestName;
            
            //console.log(this.duplicatedTestName);
        });
        duplicateTestDialog.testName = newTestObject.testName;
        duplicateTestDialog.testArray = this.tests;
        duplicateTestDialog.testToDuplicate = test;
    }

    /**
     * Checks if any candidate has taken the test
     */
    disableEditForTheTestsIfAttendeesExist() {
        this.tests.forEach(test => {
            test.isEditTestEnabled = !(test.numberOfTestAttendees > 0);
        });
    }

    /**
     * Navigates the user to the select sections page 
     * @param test: an object of Test class
     */
    editTest(test: Test) {
        // Checks if any candidate has taken the test
        this.testService.isTestAttendeeExist(test.id).subscribe((res) => {
            if (!res.response) {
                this.router.navigate(['/tests/' + test.id + '/sections']);
            }
        });
    }

    /**
     * Selects the search text area on clicking of the search button
     * @param $event is of type Event and is used to call stopPropagation()
     * @param search is of type any
     */
    selectTextArea($event: any, search: any) {
        $event.stopPropagation();
        setTimeout(() => {
            search.select();
        }, 0);
    }

}

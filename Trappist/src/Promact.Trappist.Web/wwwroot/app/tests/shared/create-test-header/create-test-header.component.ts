import { Component, OnInit, ViewChild } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { Test } from '../../tests.model';
import { TestService } from '../../tests.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';


@Component({
    moduleId: module.id,
    selector: 'create-test-header',
    templateUrl: 'create-test-header.html',
})
export class CreateTestHeaderComponent implements OnInit {
    testSettings: Test;
    testId: number;
    editName: string;
    testNameUpdatedMessage: string;
    isTestNameExist: boolean;
    testNameRef: string;
    isEditButtonVisible: boolean;
    id: number;

    constructor(private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
        this.testNameUpdatedMessage = 'Test Name has been updated successfully';
        this.isTestNameExist = false;
        this.isEditButtonVisible = true;
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
    }

    /**
     * Open snackbar
     * @param message contains the message to be displayed when the snackbar gets opened
     */
    openSnackBar(message: string) {
        let snackBarRef = this.snackbarRef.open(message, 'Dismiss', {
            duration: 4000,
        });
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
     * Selects the Test Name from the text box containing it on focus
     * @param $event is used to select the contents of the target text box
     */
    selectAllContent($event: any) {
        $event.target.select();
    }

    /**
     * Hides the edit button and makes the check and close buttons visible
     */
    hideEditButton() {
        this.isEditButtonVisible = false;
    }

    /**
     * Makes the edit button visible and the valid Test name is binded to the label containing the Test name
     * @param testName contains the value of the text box containing the Test name
     */
    showEditButton(testName: string) {
        this.isEditButtonVisible = true;
        this.id = this.testSettings.id;
        this.testService.getTestSettings(this.id).subscribe((name) => {
            this.testSettings = (name);
        });
        if (testName === '' || !testName.match(RegExp('^[a-zA-Z0-9_@ $#%&_*^{}[\]\|.?-]*$')) || this.isTestNameExist === true) {
            testName = this.testSettings.testName;
            this.isTestNameExist = false;
        }
    }

    /**
     * Updates the edited Test name in the database
     * @param id contains the value of the Id from the route
     * @param testObject is an object of the class Test
     */
    updateTestName(id: number, testObject: Test) {
        this.testNameRef = this.testSettings.testName;
        this.testService.IsTestNameUnique(this.testNameRef, id).subscribe((isTestNameUnique) => {
            if (isTestNameUnique) {
                this.testService.updateTestName(id, testObject).subscribe((response) => {
                    this.openSnackBar(this.testNameUpdatedMessage);
                });
            }
            else {
                this.isTestNameExist = true;
            }
        },
        );
    }

    /**
     * Method to toggle error message
     */
    changeErrorMessage() {
        this.isTestNameExist = false;
    }
} 

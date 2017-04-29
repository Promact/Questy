import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { TestService } from '../../tests.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { Test } from '../../tests.model';

@Component({
    moduleId: module.id,
    selector: 'create-test-header',
    templateUrl: 'create-test-header.html',
})

export class CreateTestHeaderComponent implements OnInit {
    testId: number;
    editName: string;
    testNameUpdatedMessage: string;
    isTestNameExist: boolean;
    testNameRef: string;
    isEditButtonVisible: boolean;
    isLabelVisible: boolean;
    id: number;
    testName: string;
    editedTestName: string;
    isWhiteSpaceError: boolean;
    nameOfTest: string;
    @Input('testDetails')
    public testDetails: Test;
    @Input('testNameReference')
    public testNameReference: string;

    constructor(private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testNameUpdatedMessage = 'Test Name has been updated successfully';
        this.isTestNameExist = false;
        this.isEditButtonVisible = true;
        this.isLabelVisible = true;
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
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
        this.isLabelVisible = false;
        if (this.editedTestName) {
            this.testDetails.testName = this.editedTestName;
        }
    }

    /**
     * Makes the edit button visible and the valid Test name is binded to the label containing the Test name
     * @param testName contains the value of the text box containing the Test name
     */
    showEditButton(testName: string) {
        this.isEditButtonVisible = true;
        this.isLabelVisible = true;
        this.nameOfTest = this.testNameReference;
        if (testName === '' || !testName.match(RegExp('^[a-zA-Z0-9_@ $#%&_*^{}[\]\|.?-]*$')) || this.isTestNameExist === true) {
            if (this.nameOfTest)
                this.testDetails.testName = this.nameOfTest;
            testName = this.testDetails.testName;
            this.isTestNameExist = false;
        }
        else if (this.editedTestName)
            this.testDetails.testName = this.editedTestName;
        else
            this.testDetails.testName = this.testNameReference;
    }

    /**
     * Updates the edited Test name in the database
     * @param id contains the value of the Id from the route
     * @param testObject is an object of the class Test
     */
    updateTestName(id: number, testObject: Test) {
        this.testNameRef = this.testDetails.testName.trim();
        if (this.testNameRef) {
            this.testService.IsTestNameUnique(this.testNameRef, id).subscribe((isTestNameUnique) => {
                if (isTestNameUnique) {
                    this.testService.updateTestName(id, testObject).subscribe((response) => {
                        this.testName = response.testName;
                        this.editedTestName = this.testName;
                        this.openSnackBar(this.testNameUpdatedMessage);
                        this.isLabelVisible = true;
                        this.isEditButtonVisible = true;
                    });
                }
                else {
                    this.isTestNameExist = true;
                    this.isLabelVisible = false;
                }
            },
            );
        }
        else
            this.isWhiteSpaceError = true;
    }

    /**
     * Method to toggle error message
     */
    changeErrorMessage() {
        this.isTestNameExist = false;
        this.isWhiteSpaceError = false;
    }
} 

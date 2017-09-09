import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MdSnackBar } from '@angular/material';
import { TestService } from '../../tests.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { Test } from '../../tests.model';
import { PopoverModule } from 'ngx-popover';
import { TestPreviewComponent } from '../../test-preview/test-preview.compponent';
declare let screenfull: any;

@Component({
    
    selector: 'create-test-header',
    templateUrl: './create-test-header.html'
})

export class CreateTestHeaderComponent implements OnInit {
    testId: number;
    testLink: string;
    editName: string;
    testObject: Test;
    testNameUpdatedMessage: string;
    isTestNameExist: boolean;
    testNameRef: string;
    isLabelVisible: boolean;
    id: number;
    testPreviewPage: boolean;
    testName: string;
    editedTestName: string;
    isWhiteSpaceError: boolean;
    nameOfTest: string;
    @Input('testDetails')
    public testDetails: Test;
    @Input('testNameReference')
    public testNameReference: string;
    isButtonClicked: boolean;
    tooltipMessage: string;
    copiedContent: boolean;
    @Input()
    disablePreview: boolean;
   

    constructor(private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testNameUpdatedMessage = 'Test name has been updated successfully.';
        this.isTestNameExist = false;
        this.isLabelVisible = true;
        this.isButtonClicked = false;
        this.tooltipMessage = 'Copy to Clipboard';
        this.testPreviewPage = false;
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];

    }

    /**
     * Converts the magic string obtained to the format of a link
     */
    getTestLink() {
        let linkOfTest = this.testDetails.link;
        let domain = window.location.origin;
        this.testLink = domain + '/conduct/' + linkOfTest;
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
        this.isLabelVisible = false;
        if (this.editedTestName) {
            this.testDetails.testName = this.editedTestName;
        }
    }

    changeComponent() {
        if (screenfull.enabled) {
            screenfull.toggle();
        }
        this.router.navigate(['..','tests',this.testDetails.link,'preview']);
    }

    /**
     * Makes the edit button visible and the valid Test name is binded to the label containing the Test name
     * @param testName contains the value of the text box containing the Test name
     */
    showEditButton(testName: string) {
        this.isLabelVisible = true;
        if (this.editedTestName)
            this.nameOfTest = this.editedTestName;
        else
            this.nameOfTest = this.testNameReference;
        if (testName === '' || !testName.match(RegExp('^[a-zA-Z0-9_ @]*$')) || this.isTestNameExist === true) {
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
        this.isButtonClicked = true;
        this.testNameRef = this.testDetails.testName.trim();
        if (this.testNameRef) {
            this.testService.IsTestNameUnique(this.testNameRef, id).subscribe((isTestNameUnique) => {
                this.isButtonClicked = false;
                if (isTestNameUnique) {
                    this.isButtonClicked = true;
                    this.testService.updateTestName(id, testObject).subscribe((response) => {
                        this.isButtonClicked = false;
                        this.testName = response.testName;
                        this.editedTestName = this.testName;
                        this.openSnackBar(this.testNameUpdatedMessage);
                        this.isLabelVisible = true;
                    },
                        errorHandling => {
                            this.isButtonClicked = false;
                            this.openSnackBar('Something went wrong.Please try again later.');
                        });
                }
                else {
                    this.isTestNameExist = true;
                    this.isLabelVisible = false;
                }
            });
        }
        else
            this.isWhiteSpaceError = true;
    }

    /**
     * On pressing the enter key the test name will be updated if test name is valid
     * @param testName contains the test name 
     */
    onEnter(testName: string) {
        if (!this.isButtonClicked && testName)
            this.updateTestName(this.testId, this.testDetails);
    }

    /**
     * Method to toggle error message
     */
    changeErrorMessage() {
        this.isTestNameExist = false;
        this.isWhiteSpaceError = false;
    }

    /**
    * Displays the tooltip message
    * @param $event is of type Event and is used to call stopPropagation()
    * @param testLink contains the link of the test
    */
    showTooltipMessage($event: Event, testLink: any) {
        $event.stopPropagation();
        setTimeout(() => {
            testLink.select();
        }, 0);
        this.tooltipMessage = 'Copied';
    }

    /**
     * Changes the tooltip message
     */
    changeTooltipMessage() {
        this.tooltipMessage = 'Copy to Clipboard';
    }
} 

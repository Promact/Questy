import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MdDialog, MdSnackBar } from '@angular/material';
import { Test } from '../../tests.model';
import { TestService } from '../../tests.service';
import { ActivatedRoute } from '@angular/router';
import { TestLaunchDialogComponent } from '../../test-settings/test-launch-dialog.component';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';


@Component({
    moduleId: module.id,
    selector: 'create-test-footer',
    templateUrl: 'create-test-footer.html',
})
export class CreateTestFooterComponent implements OnInit {
    testSettings: Test;
    testId: number;
    testSettingsUpdatedMessage: string;
    isTestSection: boolean;
    isTestQuestion: boolean;
    isTestSettings: boolean;
    @Input('settingsForm')
    public settingsForm: NgForm;
    @Input('validStartDate')
    public validStartDate: boolean;
    @Input('validEndDate')
    public validEndDate: boolean;
    @Input('validTime')
    public validTime: boolean;

    constructor(public dialog: MdDialog, private testService: TestService, public router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
        this.testSettingsUpdatedMessage = 'The settings of the Test has been updated successfully';
        this.isTestSection = false;
        this.isTestQuestion = false;
        this.isTestSettings = false;
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestSettings(this.testId);
        this.getComponent();
    }

    /**
     * Displays the Component whose route matches that of the url
     */
    getComponent() {
        this.isTestSection = this.router.url === '/tests/sections/' + this.testId ? true : false;
        this.isTestQuestion = this.router.url === '/tests/questions/' + this.testId ? true : false;
        this.isTestSettings = this.router.url === '/tests/' + this.testId + '/settings' ? true : false;
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
     *  Updates the settings edited for the selected Test and redirects to the test dashboard after the settings of the selected Test has been successfully updated
     * @param id contains the value of the Id from the route
     * @param testObject is an object of the class Test
     */
    updateTestSettings(id: number, testObject: Test) {
        this.testService.updateTestSettings(id, testObject).subscribe((response) => {
            let snackBarRef = this.snackbarRef.open('Saved changes successfully', 'Dismiss', {
                duration: 3000,
            });
            snackBarRef.afterDismissed().subscribe(() => {
                this.router.navigate(['/tests']);
            });
        });
    }

    /**
     * Launches the Test Dialog Box and also updates the Settings edited for the selected Test
     * @param id contains the value of the Id from the route
     * @param testObject is an object of class Test
     */
    launchTestDialog(id: number, testObject: Test) {
        this.testService.updateTestSettings(id, testObject).subscribe((response) => {
            this.openSnackBar(this.testSettingsUpdatedMessage);
        });
        let instance = this.dialog.open(TestLaunchDialogComponent).componentInstance;
        instance.testSettingObject = testObject;
    }
}

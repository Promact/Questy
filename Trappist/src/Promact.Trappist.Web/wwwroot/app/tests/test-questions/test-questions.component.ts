import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Test } from '../tests.model';
import { TestService } from '../tests.service';
import { MdDialog, MdSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { FormGroup } from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'test-questions',
    templateUrl: 'test-questions.html'
})

export class TestQuestionsComponent implements OnInit{
    testSettings: Test;
    testId: number;
    constructor(public dialog: MdDialog, private testService: TestService, private router: Router, private route: ActivatedRoute, private snackbarRef: MdSnackBar) {
        this.testSettings = new Test();
    }

    /**
    * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
    */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getTestById(this.testId);
    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id contains the value of the Id from the route
     */
    getTestById(id: number) {
        this.testService.getTestById(id).subscribe((response) => {
            this.testSettings = (response);
        });
    }
    
}

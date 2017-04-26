import { Component, Inject, OnInit } from '@angular/core';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { TestService } from '../tests.service';
import { Test } from '../tests.model';

@Component({
    moduleId: module.id,
    selector: 'incomplete-test-creation',
    templateUrl: 'incomplete-test-creation-dialog.html'
})

export class IncompleteTestCreationDialogComponent implements OnInit {
    isQuestionMissing: boolean;
    testId: number;
    test: Test;

    constructor(public testService: TestService, public dialogRef: MdDialogRef<IncompleteTestCreationDialogComponent>, @Inject(MD_DIALOG_DATA) public data: any, public route: ActivatedRoute) {
        this.test = new Test();           
    }

    ngOnInit() {
        this.test = this.data;
        this.testId = this.test.id;
        this.isQuestionMissing = this.test.isQuestionMissisng;          
    }

    closeDialog() {
        this.dialogRef.close();
    }
}
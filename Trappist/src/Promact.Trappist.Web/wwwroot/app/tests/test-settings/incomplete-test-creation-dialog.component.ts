import { Component, Inject, OnInit } from '@angular/core';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Test } from '../tests.model';

@Component({
    moduleId: module.id.toString(),
    selector: 'incomplete-test-creation',
    templateUrl: 'incomplete-test-creation-dialog.html'
})

export class IncompleteTestCreationDialogComponent implements OnInit {  
    testId: number;
    isQuestionMissing: boolean;

    constructor(public dialogRef: MdDialogRef<IncompleteTestCreationDialogComponent>, @Inject(MD_DIALOG_DATA) public data: any, public route: ActivatedRoute) {          
    }

    ngOnInit() {
        let test = new Test();
        test= this.data;        
        this.testId = test.id;
        this.isQuestionMissing = test.isQuestionMissing;      
    }

    closeDialog() {
        this.dialogRef.close();
    }
}
import { Component, OnInit, Inject } from '@angular/core';
import { Test } from '../tests.model';
import { Router } from '@angular/router';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'test-launch-dialog',
    templateUrl: 'test-launch-dialog.html'
})

export class RandomQuestionSelectionDialogComponent implements OnInit {


    constructor(public dialogRef: MdDialogRef<RandomQuestionSelectionDialogComponent>,
        @Inject(MD_DIALOG_DATA) public data: any) { }

    ngOnInit() { }
}


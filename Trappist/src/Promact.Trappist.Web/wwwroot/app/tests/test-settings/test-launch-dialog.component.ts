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

    isErrorMessageVisible: boolean;
    isPatternMismatched: boolean;

    constructor(public dialogRef: MdDialogRef<RandomQuestionSelectionDialogComponent>,
        @Inject(MD_DIALOG_DATA) public data: any) {
        this.isErrorMessageVisible = false;
    }

    ngOnInit() { }

    isNumberOfQuestionsEnteredValid(numberOfQuestionsToBeSelectedRandomly: number) {
        this.isErrorMessageVisible = +numberOfQuestionsToBeSelectedRandomly > this.data.numberOfQuestionsInSelectedCategory;
        this.isPatternMismatched = !(/^[0-9]*$/.test(numberOfQuestionsToBeSelectedRandomly.toString()));
    }
}


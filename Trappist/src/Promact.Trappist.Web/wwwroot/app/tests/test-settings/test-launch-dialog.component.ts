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

    /**
     * Checks whether the number of questions to be selected randomly is lesser or greater than the number of questions present in the selected category
     * @param numberOfQuestionsToBeSelectedRandomly contains the number of questions entered for selecting randomly
     */
    isNumberOfQuestionsEnteredValid(numberOfQuestionsToBeSelectedRandomly: number) {
        this.isErrorMessageVisible = +numberOfQuestionsToBeSelectedRandomly > this.data.numberOfQuestionsInSelectedCategory;
        this.isPatternMismatched = !(/^[0-9]*$/.test(numberOfQuestionsToBeSelectedRandomly.toString()));
    }
}


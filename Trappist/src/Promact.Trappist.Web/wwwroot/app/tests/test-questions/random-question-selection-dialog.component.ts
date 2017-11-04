import { Component, Inject } from '@angular/core';
import { Test } from '../tests.model';
import { Router } from '@angular/router';
import { MdDialog, MdDialogRef, MD_DIALOG_DATA } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'random-question-selection-dialog',
    templateUrl: 'random-question-selection-dialog.html'
})

export class RandomQuestionSelectionDialogComponent {

    isErrorMessageVisible: boolean;
    isPatternMismatched: boolean;


    constructor(public dialogRef: MdDialogRef<RandomQuestionSelectionDialogComponent>,
        @Inject(MD_DIALOG_DATA) public data: any) {
        this.isErrorMessageVisible = false;
    }

    /**
     * Checks whether the number of questions to be selected randomly is lesser or greater than the number of questions present in the selected category
     * @param numberOfQuestionsToBeSelectedRandomly contains the number of questions entered for selecting randomly
     */
    isNumberOfQuestionsEnteredValid(numberOfQuestionsToBeSelectedRandomly: number) {
        this.isErrorMessageVisible = +numberOfQuestionsToBeSelectedRandomly > this.data.numberOfQuestionsInSelectedCategory;
        this.isPatternMismatched = !(/^[0-9]*$/.test(numberOfQuestionsToBeSelectedRandomly.toString()));
    }

    /**
     * Closes the dialog box on pressing enter key and also passes the number of questions to be selected randomly
     * @param numberOfQuestionsEnteredToBeSelectedRandomly contains the number of questions entered for selecting randomly
     */
    onEnter(numberOfQuestionsEnteredToBeSelectedRandomly: number) {
        if (!this.isErrorMessageVisible && !this.isPatternMismatched && numberOfQuestionsEnteredToBeSelectedRandomly)
            this.dialogRef.close(numberOfQuestionsEnteredToBeSelectedRandomly);
    }
}


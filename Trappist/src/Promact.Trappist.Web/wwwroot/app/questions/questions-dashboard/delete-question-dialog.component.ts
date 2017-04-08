import { Component } from '@angular/core';
import { Question } from '../question.model';
import { QuestionsService } from '../questions.service';
import { MdDialogRef, MdSnackBar } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: 'delete-question-dialog',
    templateUrl: 'delete-question-dialog.html'
})
export class DeleteQuestionDialogComponent {
    private response: any;

    isQuestionExist: boolean;
    successMessage: string;
    errorMessage: string;
    question: Question;

    constructor(private questionService: QuestionsService, private dialogRef: MdDialogRef<DeleteQuestionDialogComponent>, public snackBar: MdSnackBar) {
        this.isQuestionExist = false;
        this.successMessage = 'Question Deleted Successfully';
        this.errorMessage = 'Some Error Occured!!Question Cannot be Deleted';
    }

    /**
    * Open a Snackbar 
    * @param message:To show at Snackbar
    * @param enableRouting:Redirect to page
    */
    private openSnackBar(message: string, enableRouting: boolean = false) {
        let snackBarAction = this.snackBar.open(message, 'Dismiss', {
            duration: 3000
        });
    }

    /**
     * Method to delete Question
     * @param question:Question object
     */
    deleteQuestion(question: Question) {
        this.questionService.deleteQuestion(question.id).subscribe(
            response => {
                this.dialogRef.close(question);
                this.openSnackBar(this.successMessage);
            },
            err => {
                if (err.status === 400) {
                    this.response = err.json();
                    this.errorMessage = this.response['error'][0];
                    this.dialogRef.close(null);
                    this.openSnackBar(this.errorMessage);
                }
                this.dialogRef.close(null);
                this.openSnackBar(this.errorMessage);
            }
        );
    }
}
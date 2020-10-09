import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog/dialog-ref';
import { Router } from '@angular/router';


@Component({    
    selector: 'tests-programming-guide-dialog',
    templateUrl: 'tests.programming-guide.html'
})

export class TestsProgrammingGuideDialogComponent {
    response: any;
    isDeleteAllowed: boolean;
    errorMessage: string;
    successMessage: string;

    constructor(public dialog: MatDialogRef<any>, public snackBar: MatSnackBar, private router: Router) {
    }
}

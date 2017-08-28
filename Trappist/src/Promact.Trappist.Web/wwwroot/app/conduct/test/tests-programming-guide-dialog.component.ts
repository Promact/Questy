import { Component, OnInit } from '@angular/core';
import { MdDialogRef, MdSnackBar } from '@angular/material';
import { Router } from '@angular/router';


@Component({
    moduleId: module.id,
    selector: 'tests-programming-guide-dialog',
    templateUrl: 'tests.programming-guide.html'
})

export class TestsProgrammingGuideDialogComponent {
    response: any;
    isDeleteAllowed: boolean;
    errorMessage: string;
    successMessage: string;

    constructor(public dialog: MdDialogRef<any>, public snackBar: MdSnackBar, private router: Router) {
    }
}

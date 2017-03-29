import { Component } from '@angular/core';
import { MdDialog } from '@angular/material';
import { TestLaunchDialogComponent } from '../test-settings/test-launch-dialog.component';
import { DeleteTestDialogComponent } from '../tests-dashboard/delete-test-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'test-view',
    templateUrl: 'test-view.html'
})

export class TestViewComponent {
    constructor(public dialog: MdDialog) { }

    // Open Delete Test Dialog
    deleteTestDialog() {
        this.dialog.open(DeleteTestDialogComponent);
    }
}

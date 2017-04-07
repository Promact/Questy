import { Component } from '@angular/core';
import { Test } from '../tests.model';

@Component({
    moduleId: module.id,
    selector: 'duplicate-test-dialog',
    templateUrl: 'duplicate-test-dialog.html'
})
export class DuplicateTestDialogComponent {
    testName: string;
    testArray: Test[] = new Array<Test>();

}

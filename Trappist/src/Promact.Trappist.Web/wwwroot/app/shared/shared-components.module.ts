import { NgModule } from '@angular/core';
import { TestComponent } from '../conduct/test/test.component';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MdSelectModule, MdDialogModule, MdSnackBarModule, MdCheckboxModule, MdRadioModule } from '@angular/material';
import { AceEditorModule } from 'ng2-ace-editor';
import { ConnectionService } from '../core/connection.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MdSelectModule,
        MdDialogModule,
        MdSnackBarModule,
        MdCheckboxModule,
        MdRadioModule,
        AceEditorModule
    ],
    providers: [
        ConnectionService
    ],
    declarations: [
        TestComponent,
        PageNotFoundComponent
    ],
    exports: [
        CommonModule,
        FormsModule,
        MdSelectModule,
        MdDialogModule,
        MdSnackBarModule,
        MdCheckboxModule,
        MdRadioModule,
        AceEditorModule,
        TestComponent,
        PageNotFoundComponent
    ]
})
export class SharedComponentsModule { }

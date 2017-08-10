import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { Md2AccordionModule, Md2DataTableModule } from 'md2';
import { CKEditorModule } from 'ng2-ckeditor';
import { ChartsModule } from 'ng2-charts';
import { PopoverModule } from 'ngx-popover';
import { AceEditorModule } from 'ng2-ace-editor';
import { ClipboardModule } from 'ngx-clipboard';
import { TestComponent } from '../conduct/test/test.component';



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        BrowserAnimationsModule,
        MaterialModule,
        RouterModule,
        Md2AccordionModule.forRoot(),
        Md2DataTableModule.forRoot(),
        CKEditorModule,
        PopoverModule,
        ChartsModule,
        AceEditorModule,
        ClipboardModule
    ],
    declarations: [
        TestComponent
    ],
    exports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        Md2AccordionModule,
        Md2DataTableModule,
        CKEditorModule,
        ChartsModule,
        PopoverModule,
        AceEditorModule,
        ClipboardModule,
        TestComponent
    ]
})
export class SharedModule { }

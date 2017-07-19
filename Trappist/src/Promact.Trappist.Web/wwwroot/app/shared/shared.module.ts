import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { Md2AccordionModule, Md2DataTableModule } from 'md2';
import { CKEditorModule } from 'ng2-ckeditor';
import { PopoverModule } from 'ngx-popover';

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
        PopoverModule
    ],
    declarations: [
    ],
    exports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        Md2AccordionModule,
        Md2DataTableModule,
        CKEditorModule,
        PopoverModule
    ]
})
export class SharedModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { Md2AccordionModule, Md2DataTableModule, Md2TooltipModule } from 'md2';
import { CKEditorModule } from 'ng2-ckeditor';
import { ChartsModule } from 'ng2-charts';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        BrowserAnimationsModule,
        MaterialModule.forRoot(),
        RouterModule,
        Md2AccordionModule.forRoot(),
        Md2DataTableModule.forRoot(),
        Md2TooltipModule.forRoot(),
        CKEditorModule,
        ChartsModule
    ],
    declarations: [
    ],
    exports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        Md2AccordionModule,
        Md2DataTableModule,
        Md2TooltipModule,
        CKEditorModule,
        ChartsModule
    ]
})
export class SharedModule { }

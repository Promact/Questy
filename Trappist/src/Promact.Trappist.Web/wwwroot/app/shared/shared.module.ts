import { NgModule } from '@angular/core';
import { SharedComponentsModule } from './shared-components.module';
import { MaterialModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { Md2AccordionModule, Md2DataTableModule } from 'md2';
import { CKEditorModule } from 'ng2-ckeditor';
import { ChartsModule } from 'ng2-charts';
import { PopoverModule } from 'ngx-popover';
import { ClipboardModule } from 'ngx-clipboard';
import { SelectTextAreaDirective } from '../tests/directive';


@NgModule({
    imports: [
        SharedComponentsModule,
        BrowserAnimationsModule,
        MaterialModule,
        RouterModule,
        Md2AccordionModule,
        Md2DataTableModule,
        CKEditorModule,
        PopoverModule,
        ChartsModule,
        ClipboardModule,
    ],
    declarations: [
        SelectTextAreaDirective
    ],
    exports: [
        SharedComponentsModule,
        MaterialModule,
        Md2AccordionModule,
        Md2DataTableModule,
        CKEditorModule,
        ChartsModule,
        PopoverModule,
        ClipboardModule,
        SelectTextAreaDirective
    ]
})
export class SharedModule { }


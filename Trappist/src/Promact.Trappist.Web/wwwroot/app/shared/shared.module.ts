﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { Md2AccordionModule } from 'md2';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule.forRoot(),
        RouterModule,
        Md2AccordionModule.forRoot()
    ],
    declarations: [
    ],
    exports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        Md2AccordionModule
    ]
})
export class SharedModule { }
import { Directive, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
    selector: '[defaultSelect]'
})

export class SelectTextAreaDirective implements AfterViewInit {
    constructor(private elRef: ElementRef) {
    }
    ngAfterViewInit(): void {
        let input = this.elRef.nativeElement;
        setTimeout(() => {
            input.select();
        }, 500);
    }
} 
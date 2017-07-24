import { Component, ElementRef, HostListener } from '@angular/core';

@Component({
    moduleId: module.id.toString(),
    selector: 'test-link-popover',
    templateUrl: 'test-link-popover.html'
})
export class TestLinkPopoverComponent {
    public elementRef:any;

    constructor(myElement: ElementRef) {
        this.elementRef = myElement;
    }

    // Checks whether clicked inside this component or outside
    @HostListener('document:click', ['$event'])
    handleClick(event:any) {
        let clickedComponent = event.target;
        let inside = false;
        do {
            if (clickedComponent === this.elementRef.nativeElement) {
                inside = true;
            }
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
            console.log('inside');
        } else {
            console.log('outside');
        }
    }
}

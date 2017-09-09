import { Component } from '@angular/core';

@Component({
    
    selector: 'page-not-found',
    templateUrl: './page-not-found.html'
})

export class PageNotFoundComponent {
    constructor() {
        if (window.location.href.indexOf(window.location.origin + '/conduct/') > -1)
            window.location.href = '/pagenotfound';
    }
}

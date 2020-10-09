import { Component, OnInit } from '@angular/core';

@Component({    
    selector: 'test-end',
    templateUrl: 'test-end.html',
})
export class TestEndComponent implements OnInit {    
    ngOnInit() {
        history.pushState(null, null, null);
        window.addEventListener('popstate', (event) => {
                history.pushState(null, null, null);
            });
    }
}

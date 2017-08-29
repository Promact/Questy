import { Component } from '@angular/core';
import { ConductService } from "../conduct/conduct.service";

@Component({
    moduleId: module.id,
    selector: 'page-not-found',
    templateUrl: 'page-not-found.html'
})

export class PageNotFoundComponent {
    constructor(private conductService: ConductService) {
        this.conductService.disableHeader.next(true);
    }
}

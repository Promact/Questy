import { Injectable } from '@angular/core';

@Injectable()
export class MockRouteService {

    getCurrentUrl(router:any) {
        return router.url;
    }
}
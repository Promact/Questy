import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class MockRouteService {

    constructor(private router: Router) {

    }

    getOnlySingleType() {
        let url = 'http://localhost:50805/questions/single-answer';
        
    }

}
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/toPromise';

@Injectable()

export class HttpService {

    constructor(private http: Http, defaultOptions: RequestOptions) {

        //Prevent request caching for internet explorer
        defaultOptions.headers.append('Cache-control', 'no-cache');
        defaultOptions.headers.append('Cache-control', 'no-store');
        defaultOptions.headers.append('Pragma', 'no-cache');
        defaultOptions.headers.append('Expires', '0');     
    }

    get(url: string) {
        return this.http.get(url).map(res => {
            // If request fails, throw an Error that will be caught
            if (res.status === 400) {
                throw new Error('This request has failed ' + res.status);
            }
            // If everything went fine, return the response
            else {
                return res.json();
            }
        });
    }

    post(url: string, body: any) {
        let jsonBody = JSON.stringify(body);
        let headers = new Headers({ 'Content-Type': 'application/json; charset=utf-8' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, jsonBody, options).map(res => {
            switch (res.status) {
                case 200: return res.json();
                case 404: return Observable.throw('unauthorized');
                default: throw new Error('This request has failed ' + res.status);
            }                
        });
    }

    put(url: string, body: any) {
        let jsonBody = JSON.stringify(body);
        let headers = new Headers({ 'Content-Type': 'application/json; charset=utf-8' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put(url, jsonBody, options).map(res => res.json());
    }

    delete(url: string) {
        return this.http.delete(url).map((res) => res);
    }
}
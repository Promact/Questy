import { Http, Headers, RequestOptions, Response } from "@angular/http";
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs/Observable';

@Injectable()

export class HttpService {

    constructor(private http: Http) {

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

        return this.http.post(url, jsonBody, options).map(res => res.json());
    }

    put(url: string, body: any) {
        let jsonBody = JSON.stringify(body);
        let headers = new Headers({ 'Content-Type': 'application/json; charset=utf-8' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put(url, jsonBody, options).map(res => res.json());
    }

    delete(url: string) {
        return this.http.delete(url).map(res => res.json());
    }

}
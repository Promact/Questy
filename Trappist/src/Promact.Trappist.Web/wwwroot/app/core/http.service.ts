import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class HttpService {

    constructor(private readonly http: HttpClient, private readonly headers: HttpHeaders) {

        //Prevent request caching for internet explorer
        headers.append('Cache-control', 'no-cache,no-store');
        headers.append('Content-Type', 'application/json');
        headers.append('Pragma', 'no-cache');
        headers.append('Expires', '0');
    }

    get<T>(url: string): Observable<T> {
        return this.http.get<T>(url,{headers:this.headers});
    }

    post<T>(url: string, body: T): Observable<T> {               
        return this.http.post<T>(url, body ,{headers:this.headers});
    }

    put<T>(url: string, body: T): Observable<T> {         
        return this.http.put<T>(url, body, {headers:this.headers});
    }

    delete<T>(url: string): Observable<T> {
        return this.http.delete<T>(url,{headers:this.headers});
    }
}
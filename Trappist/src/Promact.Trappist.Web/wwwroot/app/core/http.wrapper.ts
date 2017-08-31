import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, ConnectionBackend, RequestOptionsArgs, Request, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

declare let MiniProfiler;

@Injectable()
export class HttpWrapper extends Http {

    constructor(_backend: ConnectionBackend, _defaultOptions: RequestOptions) {
        super(_backend, _defaultOptions);
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return super.request(url, options)
            .map(r => {
                if (typeof MiniProfiler !== 'undefined' && r && r.headers) {
                    this.makeMiniProfilerRequests(r.headers);
                }
                return r;
            });
    }

    private makeMiniProfilerRequests(headers: Headers) {
        var miniProfilerHeaders = headers.getAll('x-miniprofiler-ids');
        if (!miniProfilerHeaders) {
            return;
        }
        miniProfilerHeaders.forEach(miniProfilerIdHeaderValue => {
            var ids = JSON.parse(miniProfilerIdHeaderValue) as string[];
            MiniProfiler.fetchResults(ids);
        });
    }
}
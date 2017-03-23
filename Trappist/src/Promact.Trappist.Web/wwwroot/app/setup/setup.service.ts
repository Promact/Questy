import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()
export class SetupService {

    constructor(private httpService: HttpService) {
    }

    /**
     * This method used for validating connection string
     * @param url
     * @param model
     */
    validateConnectionString(url: string, model: any) {
        return this.httpService.post(url, model);
    }

    /**
     * This method used for verifying email Settings
     * @param url
     * @param model
     */
    validateEmailSettings(url: string, model: any) {
        return this.httpService.post(url, model);
    }

    /**
     * This method used for Creating user
     * @param url
     * @param model
     */
    createUser(url: string, model: any) {
        return this.httpService.post(url, model);
    }
}
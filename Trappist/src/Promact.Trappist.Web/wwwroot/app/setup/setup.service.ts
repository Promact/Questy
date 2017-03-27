import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';

@Injectable()
export class SetupService {
    private connectionStringUrl: string = 'api/setup/connectionstring';
    private emailSettingsUrl: string = 'api/setup/mailsettings';
    private createUserUrl: string = 'api/setup/createuser';

    constructor(private httpService: HttpService) {
    }

   /**
    * This method used for validating connection string
    * @param model
    */
    validateConnectionString(model: any) {
        return this.httpService.post(this.connectionStringUrl, model);
    }

    /**
     * This method used for verifying email Settings
     * @param model
     */
    validateEmailSettings(model: any) {
        return this.httpService.post(this.emailSettingsUrl, model);
    }

    /**
     * This method used for Creating user
     * @param model
     */
    createUser(model: any) {
        return this.httpService.post(this.createUserUrl, model);
    }
}
import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { Test } from "./tests.model";

@Injectable()

export class TestSettingService {

    private settingsApiUrl = "api/settings";

    constructor(private httpService: HttpService) {

    }

    /**
     * Gets the Settings saved for a particular Test
     * @param id is used to get the Settings of a Test by its Id
     */
    getSettings(id : number) {
        return this.httpService.get(this.settingsApiUrl+"/"+id);
    }

    /**
     * Updates the changes made to the Settings of a Test
     * @param id is used to access the Settings of that Test
     * @param body is used as an object for the Model Test
     */
    updateSettings(id: number, body: Test) {
        return this.httpService.put(this.settingsApiUrl + "/" + id, body);
    }
}
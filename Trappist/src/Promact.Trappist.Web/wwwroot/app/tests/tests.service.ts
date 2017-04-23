import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";

@Injectable()

export class TestService {

    private testsApiUrl = "api/TestDashboard";

    constructor(private httpService: HttpService) {

    }

    /**
     * get list of tests
     */
    getTests() {
        return this.httpService.get(this.testsApiUrl);
    }

}

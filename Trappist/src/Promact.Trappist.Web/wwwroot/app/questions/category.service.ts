import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
@Injectable()

export class CategoriesService {

    private categoryApiUrl = "api/Category";


    constructor(private httpService: HttpService) {

    }


    removeCategory(categoryId: number) {
        return this.httpService.delete(this.categoryApiUrl + "/" + categoryId);
    }
    /**
     * get list of questions
     */
    getCategory() {
        return this.httpService.get(this.categoryApiUrl);
    }

}
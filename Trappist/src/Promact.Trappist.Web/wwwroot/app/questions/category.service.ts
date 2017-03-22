import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";

@Injectable()

export class CategoryService {

    private categoriesApiUrl = "api/category";

    constructor(private httpService: HttpService) {

    }
    //get all categories
    getAllCategories() {
        return this.httpService.get(this.categoriesApiUrl);
    }
    // send delete request for Remove a category
    removeCategory(categoryId: number) {
        return this.httpService.delete(this.categoriesApiUrl + "/" + categoryId);
    }
}   
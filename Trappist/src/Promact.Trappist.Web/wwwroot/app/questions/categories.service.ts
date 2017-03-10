import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { Category } from "./category.model";

@Injectable()

export class CategoriesService
{

    private categoriesApiUrl = "api/Categories";

    constructor(private httpService: HttpService) {

    }

    addCategory(category:Category)
    {
        return this.httpService.post("this.categoriesApiUrl",category);
    }
}
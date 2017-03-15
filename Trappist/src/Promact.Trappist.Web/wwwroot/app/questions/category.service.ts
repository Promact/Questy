import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { Category} from "./category.model";

@Injectable()
export class CategoryService {

    private categoryApiUrl = "api/Category";

    constructor(private httpService: HttpService) {

    }
    addCategory(category:Category) {
        return this.httpService.post("this.categoryApiUrl",category);
    }
    editCategory(category: Category)
    {
        return this.httpService.put("this.categoryApiUrl",category);
    }
}
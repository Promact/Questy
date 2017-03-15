
import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { QuestionsDashboardComponent} from "./questions-dashboard/questions-dashboard.component";
import { Category} from "./category.model";
@Injectable()

export class CategoryService {

    private categoriesApiUrl = "api/category";

    constructor(private httpService: HttpService) {

    }
    //get all categories
    getAllCategories() {
        return this.httpService.get(this.categoriesApiUrl);
    }

    //add Category
    addCategory(category:Category) {
        return this.httpService.post(this.categoriesApiUrl,category);
    }
    ////edit Category
    //editCategory(category: Category)
    //{
    //    return this.httpService.put(this.categoriesApiUrl+"/",Category)
    //}
}
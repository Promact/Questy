import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { QuestionsDashboardComponent } from "./questions-dashboard/questions-dashboard.component";
import { Category } from "./category.model";
@Injectable()
export class CategoryService {
    private categoriesApiUrl = "api/category";
    constructor(private httpService: HttpService) {
    }

    // get all categories
    getAllCategories() {
        return this.httpService.get(this.categoriesApiUrl);
    }

    /*
    * post Method
    * method to Add a Category
    *<param name="category">category object contains Category details </param>
    */
    addCategory(category: Category) {
        return this.httpService.post(this.categoriesApiUrl, category);
    }

    /*
    * Method to check  same categoryName exists or not
    *<param name="categoryName">categoryName which willl be checked</param>
    */
    checkDuplicateCategoryName(categoryName: string) {
        return this.httpService.post(this.categoriesApiUrl + "/checkduplicatecategoryname", categoryName);
    }

    /*
    * Update Category
    *<param name="id">id whose property is to be updated</param>
    *<param name="category">category object contains Category details </param>
    */
    updateCategory(id: number, category: Category) {
        return this.httpService.put(this.categoriesApiUrl + "/" + id, category);
    }
}
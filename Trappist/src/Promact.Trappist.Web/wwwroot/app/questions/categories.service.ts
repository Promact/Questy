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

    /**
     * Method to Add Category
     * @param category category object contains Category details
     */
    addCategory(category: Category) {
        return this.httpService.post(this.categoriesApiUrl, category);
    }

    /**
     * Method to Check DupliCate Category Name
     * @param categoryName :Category Name
     */
    checkDuplicateCategoryName(categoryName: string) {
        return this.httpService.post(this.categoriesApiUrl + "/checkduplicatecategory", categoryName);
    }

    /**
     * Method to Update Category
     * @param id:primary key of the Category whose Value will be Changed
     * @param category:Category object contains Category Object
     */
    updateCategory(id: number, category: Category) {
        return this.httpService.put(this.categoriesApiUrl + "/" + id, category);
    }
}
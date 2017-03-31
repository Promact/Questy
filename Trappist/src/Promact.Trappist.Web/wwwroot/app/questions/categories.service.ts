import { Injectable } from '@angular/core';
import { HttpService } from '../core/http.service';
import { QuestionsDashboardComponent } from './questions-dashboard/questions-dashboard.component';
import { Category } from './category.model';

@Injectable()
export class CategoryService {
    private categoriesApiUrl = 'api/category';
    constructor(private httpService: HttpService) {
    }
    //To get all the Categories
    getAllCategories() {
        return this.httpService.get(this.categoriesApiUrl);
    }

    /**
     *Api to add Category
     * @param category category object contains category details
     */
    addCategory(category: Category) {
        return this.httpService.post(this.categoriesApiUrl, category);
    }

    /**
     * Api to update Category
     * @param id:Primary key of the Category whose value will be changed
     * @param category:category object contains Category details
     */
    updateCategory(id: number, category: Category) {
        return this.httpService.put(this.categoriesApiUrl + '/' + id, category);
    }
}
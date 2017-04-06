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
     * @param category: Category object
     */
    addCategory(category: Category) {
        return this.httpService.post(this.categoriesApiUrl, category);
    }

    /**
     * Api to update Category
     * @param id: Category whose value will be changed
     * @param category:Category object
     */
    updateCategory(id: number, category: Category) {
        return this.httpService.put(this.categoriesApiUrl + '/' + id, category);
    }

    /**
    * API to remove Category
    * @param categoryId: Category id
    */
    removeCategory(categoryId: number) {
        return this.httpService.delete(this.categoriesApiUrl + '/' + categoryId);
    }
}
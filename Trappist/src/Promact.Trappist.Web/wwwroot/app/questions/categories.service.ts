import { Injectable } from '@angular/core';
import { HttpService} from '../core/http.service';
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
     * Method to Add Category
     * @param category category object contains Category details
     */
    addCategory(category: Category) {
        return this.httpService.post(this.categoriesApiUrl, category);
    }

    /**
     * Method to Update Category
     * @param id:primary key of the Category whose Value will be Changed
     * @param category:Category object contains Category Object
     */
    updateCategory(id: number, category: Category) {
        return this.httpService.put(this.categoriesApiUrl + '/' + id, category);
    }
}
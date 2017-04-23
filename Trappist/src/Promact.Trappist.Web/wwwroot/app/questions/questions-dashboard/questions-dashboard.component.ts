import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { MdDialog } from '@angular/material';
import { CategoriesService } from '../category.service';

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {
    categoyList: Array<category> = [];
    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoriesService) {
        this.getAllQuestions();
        this.getCategories();
    }

    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            console.log(questionsList);
        });
    }

    // Open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }

    getCategories() {
        this.categoryService.getCategory().subscribe((Response: category[]) => { this.categoyList = (Response) });
    }
    removeCategory(categoryId: number) {
              this.categoryService.removeCategory(categoryId).subscribe((Response) => Response.json());
    }

}

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent { }
class category {

    id: number;
    createdDateTime: Date;
    updateDateTime: Date;
    categoryName: string;
}
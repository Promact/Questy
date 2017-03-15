
import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Category } from "../category.model";
import { Location } from '@angular/common';

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent implements OnInit {
    private category: Category = new Category();
    categoryArray: string[] = new Array<string>();

    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService) {

    }

    ngOnInit() {
        this.getAllQuestions();
        this.getAllCategories();
    }
	//To Get All The categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        });
    }
    //add Category
    CategoryAdd(category: Category) {
        this.categoryService.addCategory(category).subscribe((response) => {
            if (response.ok) {
                this.categoryArray = response.json;
                //this.category = new Category;
            }
        });
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

    // Open Delete Category Dialog
    deleteCategoryDialog() {
      this.dialog.open(DeleteCategoryDialogComponent);
    }

}

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent {
    private category: Category = new Category();
    categoryArray: string[] = new Array<string>();
    constructor(private categoryService: CategoryService, private dialog: MdDialog) { }

    CategoryAdd(category: Category) {
        this.categoryService.addCategory(category).subscribe((response) => {
                this.categoryArray = response.json;
                this.dialog.closeAll();
                //this.category = new Category;
        });
    }
}

@Component({
  moduleId: module.id,
  selector: 'delete-category-dialog',
  templateUrl: "delete-category-dialog.html"
})
export class DeleteCategoryDialogComponent { }

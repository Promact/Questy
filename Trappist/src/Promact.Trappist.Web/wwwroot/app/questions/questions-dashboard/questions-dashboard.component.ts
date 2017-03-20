import { Component, OnInit, ViewChild } from "@angular/core";
import { AddCategoryDialogComponent } from "./add-category-dialog.component";
import { DeleteCategoryDialogComponent } from "./delete-category-dialog.component";
import { DeleteQuestionDialogComponent } from "./delete-question-dialog.component";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Category } from "../category.model";
import { RenameCategoryDialogComponent } from "./rename-category-dialog.component";

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent implements OnInit {
    category: Category = new Category();
    categoryArray: Category[] = new Array<Category>();

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

    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            console.log(questionsList);
        });
    }
    // Open Add Category Dialog
    addCategoryDialog() {
        this.dialog.open(AddCategoryDialogComponent);
    }
    //open Rename Category Dialog
    renameCategoryDialog(cat: any) {
        var prop = this.dialog.open(RenameCategoryDialogComponent).componentInstance;
        prop.category = JSON.parse(JSON.stringify(cat));
    }
    // Open Delete Category Dialog
    deleteCategoryDialog() {
        this.dialog.open(DeleteCategoryDialogComponent);
    }
}
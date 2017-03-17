
import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { CategoryService } from "../categories.service";
import { MdDialog } from '@angular/material';
import { Category } from "../category.model";

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
    //open Edit Category Dialog
    editCategoryDialog(cat: any) {
        var prop = this.dialog.open(EditCategoryDialogComponent).componentInstance;
        prop.category = JSON.parse(JSON.stringify(cat));
    }
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
    isNameExist: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

    /*
    *Add category in Cateogry Model
    */
    CategoryAdd(category: Category) {
        this.categoryService.addCategory(category).subscribe((response) => {
            this.dialog.closeAll();
        });
    }

    /*To check Whether CategoryName Exists or not
    *If categoryName Exists it will return true and button will be disabled
    */
    CheckDuplicateCategoryName(categoryName: string)
    {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isNameExist = result;
        });
    }

}

@Component({
    moduleId: module.id,
    selector: 'edit-category-dialog',
    templateUrl: "edit-category-dialog.html"
})

export class EditCategoryDialogComponent {
    category: Category = new Category();
    isNameExist: boolean = false;
    constructor(private categoryService: CategoryService, private dialog: MdDialog) {
    }

     /*
    * edit category from Cateogry Model
    */
    categoryedit(category: Category) {
        this.categoryService.editCategory(category.id, category).subscribe((response) => {
            this.dialog.closeAll();
        });
    }

    /* to check Whether CategoryName Exists or not
    * if categoryName Exists it will return true and button will be disabled
    */
    CheckDuplicateCategoryName(categoryName: string) {
        this.categoryService.checkDuplicateCategoryName(categoryName).subscribe((result) => {
            this.isNameExist = result;
        });
    }
}

@Component({
  moduleId: module.id,
  selector: 'delete-category-dialog',
  templateUrl: "delete-category-dialog.html"
})
export class DeleteCategoryDialogComponent { }

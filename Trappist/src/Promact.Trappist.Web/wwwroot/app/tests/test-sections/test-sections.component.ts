import { Component, OnInit } from '@angular/core';
import { Category } from '../../questions/category.model';
import { CategoryService } from '../../questions/categories.service';
import { Test, TestCategory, TestQuestion } from '../tests.model';
import { TestCreateDialogComponent } from '../tests-dashboard/test-create-dialog.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import 'rxjs/add/operator/switchMap';
import { Observable } from 'rxjs/Observable';
import { TestService } from '../tests.service';
import { MdDialog, MdDialogRef } from '@angular/material';
import { DeselectCategoryDialogComponent } from "../test-sections/deselect-category-dialog.component";

@Component({
    moduleId: module.id,
    selector: 'test-sections',
    templateUrl: 'test-sections.html'
})

export class TestSectionsComponent implements OnInit {
    editName: boolean;
    Categories: Category[] = new Array<Category>();
    testCategories: Array<TestCategory>;
    test: Test;
    testCategoryObj: TestCategory;
    testQuestion: TestQuestion;
    deselectCategoryError: boolean;

    constructor(private categoryService: CategoryService, private testService: TestService, private route: ActivatedRoute, private router: Router, public dialog: MdDialog) {
        this.getAllCategories();
        this.test = new Test();
        this.testCategoryObj = new TestCategory();
        this.testCategories = [];
        this.testQuestion = new TestQuestion();
    }

    ngOnInit() {
        let id = +this.route.snapshot.params['id'];
        this.testService.getTestSettings(id)
            .subscribe((test: Test) => { this.test = test });
    }

    /**
     * To get All categories
     */
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((response) => { this.Categories = (response); });
    }

    /**
     * To Select or Deselect a Category from list
     * @param category
     */
    onSelect(category: Category) {
        if (!category.isSelect)
            category.isSelect = true;
        else
            if (this.testQuestion.isSelect) { 
                this.deselectCategoryError = true;
                let dialogRef = this.dialog.open(DeselectCategoryDialogComponent);
            }
            else
                category.isSelect = false;
    }

    /**
     * To Save the Selected Categories and redirect user further for question selection
     */
    SaveNext() {
        this.SaveCategory();
        //this.router.navigateByUrl('/tests/questions');
    }

    /**
     * Save the Selected Categories for Question Selection and redirect the user to the test dashboard
     */
    SaveExit() {
        this.SaveCategory();
        this.router.navigateByUrl('/tests');
    }

    SaveCategory() {
        let categories = this.Categories.filter(function (x) {
            return (x.isSelect);
        });
        for (let category of categories) {
            this.testCategoryObj.categoryId = category.id;
            this.testCategoryObj.testId = this.test.id;
            this.testCategoryObj.category = category;
            this.testCategoryObj.test = this.test;
            this.testCategories.push(this.testCategoryObj);
            this.testCategoryObj = new TestCategory()
        }
        this.testService.addSelectedCategories(this.testCategories).subscribe();
    }
}
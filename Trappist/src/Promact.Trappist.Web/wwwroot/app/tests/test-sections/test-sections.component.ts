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
import { Question } from '../../questions/question.model';
import { TestDetails } from '../test-details';

@Component({
    moduleId: module.id,
    selector: 'test-sections',
    templateUrl: 'test-sections.html'
})

export class TestSectionsComponent implements OnInit {
    editName: boolean;
    Categories: Category[];
    testCategories: Array<TestCategory>;
    test: Test;
    testCategoryObj: TestCategory;
    testQuestion: Array<TestQuestion>;
    deselectCategoryError: boolean;
    question: Question[] = new Array<Question>();
    testDetailsObj: TestDetails;

    constructor(private categoryService: CategoryService, private testService: TestService, private route: ActivatedRoute, private router: Router, public dialog: MdDialog) {
        this.test = new Test();
        this.testCategoryObj = new TestCategory();
        this.testCategories = [];
        this.testQuestion = [];
        this.testDetailsObj = new TestDetails();
        this.getTestSectionsDetails();
        this.Categories = this.testDetailsObj.category;
    }

    ngOnInit() {
        let id = +this.route.snapshot.params['id'];
        this.testService.getTestSettings(id)
            .subscribe((test: Test) => { this.test = test });

    }

    /**
     * To get All categories
     */
    getTestSectionsDetails() {
        let id = +this.route.snapshot.params['id'];
        this.testService.getTestSettings(id)
            .subscribe((test: Test) => { this.test = test });
        this.testService.getTestDetails(id).subscribe((response) => {
            this.testDetailsObj = response;
            console.log(this.testDetailsObj);
        });
    }

    /**
     * To Select or Deselect a Category from list
     * @param category
     */
    onSelect(category: any) {
        this.testDetailsObj.category = category;
        if (!category.isSelect)
            category.isSelect = true;
        //else {
        //    for (let testquestion of this.testQuestion) {
        //        if (testquestion.testCategoryId = this.testCategoryObj.categoryId) {
        //            let dialogRef = this.dialog.open(DeselectCategoryDialogComponent);
        //        }
        else
            category.isSelect = false;
    }



    /**
     * To Save the Selected Categories and redirect user for question selection
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
        let categories = this.testDetailsObj.category.filter(function (x) {
            return (x.isSelect);
        });
        for (let category of categories) {
            this.testCategoryObj.categoryId = category.id;
            this.testCategoryObj.testId = this.test.id;
            this.testCategories.push(this.testCategoryObj);
            this.testCategoryObj = new TestCategory()
        }
         this.testService.addSelectedCategories(this.testCategories).subscribe();
    }
}

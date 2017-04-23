import { Component, OnInit, ViewChild } from "@angular/core";
import { QuestionsService } from "../questions.service";
import { MdDialog } from '@angular/material';

@Component({
    moduleId: module.id,
    selector: "questions-dashboard",
    templateUrl: "questions-dashboard.html"
})

export class QuestionsDashboardComponent {
    
    constructor(private questionsService: QuestionsService, private dialog: MdDialog) {
        this.getAllQuestions();
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

}

@Component({
    moduleId: module.id,
    selector: 'add-category-dialog',
    templateUrl: "add-category-dialog.html"
})
export class AddCategoryDialogComponent { }

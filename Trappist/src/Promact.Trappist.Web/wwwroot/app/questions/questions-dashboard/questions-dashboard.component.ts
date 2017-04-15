import { Component, OnInit, ViewChild } from '@angular/core';
import { MdDialog } from '@angular/material';
import { AddCategoryDialogComponent } from './add-category-dialog.component';
import { DeleteCategoryDialogComponent } from './delete-category-dialog.component';
import { DeleteQuestionDialogComponent } from './delete-question-dialog.component';
import { QuestionsService } from '../questions.service';
import { CategoryService } from '../categories.service';
import { QuestionDisplay } from '../../questions/question-display';
import { DifficultyLevel } from '../../questions/enum-difficultylevel';
import { QuestionType } from '../../questions/enum-questiontype';
import { Category } from '../../questions/category.model';
import { RenameCategoryDialogComponent } from './rename-category-dialog.component';
import { Router } from '@angular/router';
import { Question } from '../../questions/question.model';
import { UpdateCategoryDialogComponent } from './update-category-dialog.component';

@Component({
    moduleId: module.id,
    selector: 'questions-dashboard',
    templateUrl: 'questions-dashboard.html'
})

export class QuestionsDashboardComponent implements OnInit {
    category: Category;
    showSearchInput: boolean;
    easy: number;
    medium: number;
    hard: number;
    selectedCategory: Category;
    questionDisplay: QuestionDisplay[];
    question: QuestionDisplay[];
    categoryArray: Category[];
    //To enable enum difficultylevel in template
    DifficultyLevel = DifficultyLevel;
    // to enable enum questiontype in template 
    QuestionType = QuestionType;
    optionName: string[];
    selectedDifficulty: DifficultyLevel;
    matchString: string;
    isAllQuestionsSelected: boolean;

    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService, private router: Router) {
        this.category = new Category();
        this.selectedCategory = new Category();
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
        this.categoryArray = new Array<Category>();
        this.question = new Array<QuestionDisplay>();
        this.questionDisplay = new Array<QuestionDisplay>();
        this.matchString = '';
        this.easy = 0;
        this.medium = 0;
        this.hard = 0;
    }

    ngOnInit() {
        this.getAllQuestions();
        this.getAllCategories();
    }

    //To check whether the option is correct or not
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }

    //To get all the Categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
        });
    }

    //To get all the Questions
    getAllQuestions() {
        this.questionsService.getQuestions().subscribe((questionsList) => {
            this.question = questionsList;
            this.selectedCategory = new Category();
            this.isAllQuestionsSelected = true;
            this.countQuestion();
            this.questionDisplay = this.filterQuestion(this.question, this.selectedCategory, this.selectedDifficulty, this.matchString);
        });
    }

    /**
     * To set the Category active
     * @param category
     */
    isCategorySelected(category: Category) {
        this.isAllQuestionsSelected = false;
        if (category.categoryName === this.selectedCategory.categoryName)
            return 'active';
    }

    /**
     * To select the Category
     * @param category
     */
    categoryWiseFilter(category: Category) {
        this.selectedCategory = category;
        this.countQuestion();
        this.questionDisplay = this.filterQuestion(this.question, this.selectedCategory, this.selectedDifficulty, this.matchString);
    }

    /**
     * To select the DifficultyLevel
     * @param difficulty
     */
    difficultyWiseSearch(difficulty: string) {
        this.selectedDifficulty = DifficultyLevel[difficulty];
        this.questionDisplay = this.filterQuestion(this.question, this.selectedCategory, this.selectedDifficulty, this.matchString);
    }

    /**
     * To get the Search criteria from the user
     * @param matchString
     */
    getQuestionsMatchingSearchCriteria(matchString: string) {
        if (matchString.length > 2 || matchString.length === 0) {
            this.matchString = matchString;
            this.countQuestion();
            this.questionDisplay = this.filterQuestion(this.question, this.selectedCategory, this.selectedDifficulty, this.matchString);
        }
    }

    // Open Add Category Dialog
    addCategoryDialog() {
        let adddialogRef = this.dialog.open(AddCategoryDialogComponent);
        adddialogRef.afterClosed().subscribe(categoryToAdd => {
            if (categoryToAdd !== null && categoryToAdd !== undefined)
                this.categoryArray.unshift(categoryToAdd);
        });
    }

    // Open update Category Dialog
    updateCategoryDialog(category: Category) {
        let categoryToUpdate = this.categoryArray.find(x => x.id === category.id);
        let updateDialogRef = this.dialog.open(UpdateCategoryDialogComponent);
        updateDialogRef.componentInstance.category = JSON.parse(JSON.stringify(category));
        updateDialogRef.afterClosed().subscribe(updatedCategory => {
            if (updatedCategory !== null && updatedCategory !== undefined) {
                categoryToUpdate.categoryName = updatedCategory.categoryName;
                this.question.forEach(x => {
                    if (x.category.id === categoryToUpdate.id) {
                        x.category.categoryName = categoryToUpdate.categoryName;
                    }
                });
            }
        });
    }

    // Open delete category dialog and set the property of DeleteCategoryDialogComponent class
    deleteCategoryDialog(category: Category) {
        let deleteCategoryDialog = this.dialog.open(DeleteCategoryDialogComponent).componentInstance;
        deleteCategoryDialog.category = category;
        deleteCategoryDialog.categoryArray = this.categoryArray;
    }

    // Open delete question dialog
    deleteQuestionDialog() {
        this.dialog.open(DeleteQuestionDialogComponent);
    }

    /**
     * Filter Questions depending on DifficultyLevel, Category and QuestionDetails. 
     * @param question
     * @param category
     * @param difficultyLevel
     * @param matchString
     */
    filterQuestion(question: QuestionDisplay[], category: Category = null, difficultyLevel: DifficultyLevel = null, matchString: string = null): QuestionDisplay[] {
        let tempQuestion: QuestionDisplay[] = [];

        if (category !== {} && category !== null) {
            question.forEach(x => {
                if (x.category.id === category.id || category.id === undefined) {
                    tempQuestion.push(x);
                }
            });
            return this.filterQuestion(tempQuestion, null, difficultyLevel, matchString);
        }

        if (difficultyLevel !== null && difficultyLevel !== undefined) {
            question.forEach(x => {
                if (x.difficultyLevel === difficultyLevel) {
                    tempQuestion.push(x);
                }
            });
            return this.filterQuestion(tempQuestion, category, null, matchString);
        }

        if (matchString.length > 0) {
            question.forEach(x => {
                if (x.questionDetail.toLowerCase().includes(matchString.toLowerCase())) {
                    tempQuestion.push(x);
                }
            });
            return this.filterQuestion(tempQuestion, category, difficultyLevel, '');
        }
        return question;
    }

    /**
     * To count the number of Questions
     */
    countQuestion() {
        this.easy = this.medium = this.hard = 0;
        let questionList = this.filterQuestion(this.question, this.selectedCategory, null, this.matchString);
        questionList.forEach(x => {
            switch (x.difficultyLevel) {
                case 0:
                    this.easy++;
                    break;
                case 1:
                    this.medium++;
                    break;
                case 2:
                    this.hard++;
                    break;
            }
        });
    }

    /**
     * Redirect to edit question page
     * @param question
     */
    updateQuestion(question: Question) {
        if (question.questionType === 0) {
            this.router.navigate(['questions/edit-single-answer' + '/' + question.id]);
        }
        if (question.questionType === 1) {
            this.router.navigate(['questions/edit-multiple-answers' + '/' + question.id]);
        }
    }
}
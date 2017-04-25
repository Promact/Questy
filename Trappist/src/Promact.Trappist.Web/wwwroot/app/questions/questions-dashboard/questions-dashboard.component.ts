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
import { Router } from '@angular/router';
import { UpdateCategoryDialogComponent } from './update-category-dialog.component';
import { Question } from '../question.model';

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

        //Scroll to top when navigating back from other components.
        window.scrollTo(0, 0);
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

    // Open delete Category dialog
    deleteCategoryDialog(category: Category) {
        let deleteDialogRef = this.dialog.open(DeleteCategoryDialogComponent);
        deleteDialogRef.componentInstance.category = category;
        deleteDialogRef.afterClosed().subscribe(
            deletedCategory => {
                if (deletedCategory) {
                    this.categoryArray.splice(this.categoryArray.indexOf(deletedCategory), 1);
                    this.getAllQuestions();
                }
            });
    }

    // Open delete question dialog
    deleteQuestionDialog(questionToDelete: Question) {
        let deleteDialogRef = this.dialog.open(DeleteQuestionDialogComponent);
        deleteDialogRef.componentInstance.question = questionToDelete;
        deleteDialogRef.afterClosed().subscribe(
            deletedQuestion => {
                if (deletedQuestion) {
                    this.question.splice(this.question.indexOf(deletedQuestion), 1);
                    this.questionDisplay.splice(this.questionDisplay.indexOf(deletedQuestion), 1);
                    this.countQuestion();
                }
            });
    }

    /**
     * Routes to respective components for editing Question
     * @param question: QuestionDisplay object
     */
    editQuestion(question: QuestionDisplay) {
        if (question.questionType === QuestionType.codeSnippetQuestion) {
            this.router.navigate(['questions', 'programming', question.id]);
        }
        else {
            let questionType = question.questionType === 0 ? 'edit-single-answer' : 'edit-multiple-answers';
            this.router.navigate(['questions', questionType, question.id]);
        }
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
     * Method to duplicate Question
     * @param question:QuestionDisplay object
     */
    duplicateQuestion(question: QuestionDisplay) {
        if (question.questionType === QuestionType.codeSnippetQuestion) {
            this.router.navigate(['questions', 'programming', 'duplicate', question.id]);
        }
        else if (question.questionType === QuestionType.singleAnswer) {
            this.router.navigate(['questions', 'single-answer', 'duplicate', question.id]);
        }
        else {
            this.router.navigate(['questions', 'multiple-answers', 'duplicate', question.id]);
        }
    }
}
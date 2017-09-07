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
import { Router, ActivatedRoute } from '@angular/router';
import { UpdateCategoryDialogComponent } from './update-category-dialog.component';
import { Question } from '../question.model';
import { QuestionCount } from '../numberOfQuestion';

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
    loader: boolean;
    numberOfQuestions: QuestionCount;
    isAllQuestionsHaveCome: boolean;
    id: number;
    difficultyLevel: string;
    categroyId: number;
    isCategoryPresent: boolean;
    showName: string;
    selectedCategoryName: string;
    SelectedDifficultyLevel: string;
    selectedCategoryId: number;
    isSelected: boolean;
    isAllQuestionsSectionSelected: boolean;

    constructor(private questionsService: QuestionsService, private dialog: MdDialog, private categoryService: CategoryService, private router: Router, private route: ActivatedRoute) {
        this.category = new Category();
        this.selectedCategory = new Category();
        this.numberOfQuestions = new QuestionCount();
        this.optionName = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'];
        this.categoryArray = new Array<Category>();
        this.question = new Array<QuestionDisplay>();
        this.questionDisplay = new Array<QuestionDisplay>();
        this.matchString = '';
        this.easy = 0;
        this.medium = 0;
        this.hard = 0;
        this.isAllQuestionsHaveCome = false;
        this.id = 0;
        this.showName = 'All Questions';
        this.selectedDifficulty = DifficultyLevel['All'];
        this.difficultyLevel = 'All';
        this.categroyId = 0;
        this.isCategoryPresent;
        this.isSelected = false;
        this.isAllQuestionsSectionSelected = false;
    }

    ngOnInit() {
        this.loader = true;
        this.getQuestionsOnScrolling();
        this.selectedCategoryName = this.route.snapshot.params['categoryName'];
        this.SelectedDifficultyLevel = this.route.snapshot.params['difficultyLevelName'];
        this.getAllCategories();
        this.countTheQuestion();
        //Scroll to top when navigating back from other components.
        window.scrollTo(0, 0);
    }

    //To check whether the option is correct or not
    isCorrectAnswer(isAnswer: boolean) {
        if (isAnswer) {
            return 'correct';
        }
    }

    /**
     * select difficulty and category and filter as per selection
     * @param difficulty difficulty selected while adding
     * @param categoryName catergory selected while adding
     */
    SelectCategoryDifficulty(difficulty: string, categoryName: string) {
        this.selectedDifficulty = DifficultyLevel[difficulty];
        this.selectedCategory.categoryName = categoryName;
        this.difficultyLevel = difficulty;
        this.categoryArray.forEach(x => {
            if (x.categoryName === this.selectedCategoryName)
                this.selectedCategoryId = x.id;
        });
        this.categoryWiseFilter(this.selectedCategoryId, this.selectedCategoryName, difficulty);
    }

    //To get all the Categories
    getAllCategories() {
        this.categoryService.getAllCategories().subscribe((CategoriesList) => {
            this.categoryArray = CategoriesList;
            this.isCategoryPresent = this.categoryArray.length === 0 ? false : true;
            if ((this.selectedCategoryName !== undefined && this.SelectedDifficultyLevel !== undefined))
                this.SelectCategoryDifficulty(this.SelectedDifficultyLevel, this.selectedCategoryName);
            else if (this.selectedCategoryName !== undefined)
                this.SelectCategoryDifficulty('All', this.selectedCategoryName);
            else if (this.SelectedDifficultyLevel !== undefined)
                this.SelectCategoryDifficulty(this.SelectedDifficultyLevel, 'AllCategory');
            this.sortCategory();
        });
    }

    // get All questions
    getAllQuestions() {
        this.loader = true;
        this.categroyId = 0;
        this.countTheQuestion();
        this.difficultyLevel = 'All';
        this.showName = 'All Questions';
        this.questionDisplay = new Array<QuestionDisplay>();
        this.id = 0;
        this.questionsService.getQuestions(this.id, this.categroyId, this.difficultyLevel, this.matchString).subscribe((questionsList) => {
            this.question = questionsList;
            this.questionDisplay = this.questionDisplay.concat(this.question);
            if (this.questionDisplay.length !== 0)
                this.id = this.questionDisplay[this.questionDisplay.length - 1].id;
            this.selectedDifficulty = DifficultyLevel[this.difficultyLevel];
            this.router.navigate(['questions/dashboard', 'AllCategory', this.difficultyLevel]);
            this.loader = false;
            this.id++;
            this.selectedCategory = new Category();
            this.isAllQuestionsHaveCome = false;
            this.matchString = '';
        });
    }

    /**
     * sort category in alphabatical order
     */
    sortCategory() {
        this.categoryArray.sort(function (a, b) {
            if (a.categoryName.toLowerCase() < b.categoryName.toLowerCase())
                return -1;
            if (a.categoryName.toLowerCase() > b.categoryName.toLowerCase())
                return 1;
            return 0;
        });
    }

    //To get Questions while scrolling
    getQuestionsOnScrolling() {
        this.isAllQuestionsHaveCome = true;
        this.questionsService.getQuestions(this.id, this.categroyId, this.difficultyLevel, this.matchString).subscribe((questionsList) => {
            this.question = questionsList;
            if (this.question.length === 0)
                this.isAllQuestionsHaveCome = true;
            else
                this.isAllQuestionsHaveCome = false;
            this.questionDisplay = this.questionDisplay.concat(this.question);
            if (this.questionDisplay.length !== 0)
                this.id = this.questionDisplay[this.questionDisplay.length - 1].id;
            this.loader = false;
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
    categoryWiseFilter(categoryId: number, categoryName: string, difficultyLevel: string) {
        this.loader = true;
        this.showName = categoryName;
        window.scrollTo(0, 0);
        this.difficultyLevel = difficultyLevel;
        if (categoryName === 'AllCategory') {
            this.showName = 'All Questions';
            categoryId = 0;
            this.isAllQuestionsSectionSelected = true;
        }
        else
            this.isAllQuestionsSectionSelected = false;
        this.categroyId = categoryId;
        this.countTheQuestion();
        this.id = 0;
        this.isAllQuestionsHaveCome = false;
        this.questionsService.getQuestions(this.id, this.categroyId, this.difficultyLevel, this.matchString).subscribe((questionsList) => {
            this.questionDisplay = questionsList;
            if (this.questionDisplay.length !== 0)
                this.id = this.questionDisplay[this.questionDisplay.length - 1].id;
            this.selectedDifficulty = DifficultyLevel[this.difficultyLevel];
            this.selectedCategory.categoryName = categoryName;
            this.selectedCategory.id = categoryId;
            this.router.navigate(['questions/dashboard', categoryName, difficultyLevel]);
            this.loader = false;
            this.matchString = '';
        });
    }

    /**
     * To select the DifficultyLevel
     * @param difficulty
     */
    difficultyWiseSearch(difficulty: string) {
        this.loader = true;
        window.scrollTo(0, 0);
        this.id = 0;
        this.difficultyLevel = difficulty;
        if (this.selectedCategory.categoryName === 'AllCategory' || this.selectedCategoryName === undefined || this.selectedCategory.categoryName === undefined) {
            this.selectedCategory.categoryName = 'AllCategory';
            this.categroyId = 0;
            this.isAllQuestionsSectionSelected = true;
        }
        else {
            this.isAllQuestionsSectionSelected = false;
        }
        this.isAllQuestionsHaveCome = false;
        this.questionsService.getQuestions(this.id, this.categroyId, this.difficultyLevel, this.matchString).subscribe((questionsList) => {
            this.questionDisplay = questionsList;
            if (this.questionDisplay.length !== 0)
                this.id = this.questionDisplay[this.questionDisplay.length - 1].id;
            this.selectedDifficulty = DifficultyLevel[difficulty];
            this.router.navigate(['questions/dashboard', this.selectedCategory.categoryName, difficulty]);
            this.loader = false;
        });
    }

    /**
     * To get the Search criteria from the user
     * @param matchString
     */
    getQuestionsMatchingSearchCriteria(matchString: string) {
        this.matchString = matchString;
        if (matchString.trim().length > 2) {
            this.id = 0;
            this.isAllQuestionsHaveCome = false;
            this.questionsService.getQuestions(this.id, this.categroyId, this.difficultyLevel, this.matchString).subscribe((questionsList) => {
                this.questionDisplay = questionsList;
                if (this.questionDisplay.length !== 0)
                    this.id = this.questionDisplay[this.questionDisplay.length - 1].id;
                this.countTheQuestion();
            });
        }
        else if (matchString.trim().length === 0) {
            this.id = 0;
            this.isAllQuestionsHaveCome = false;
            this.countTheQuestion();
            this.questionDisplay = new Array<QuestionDisplay>();
            this.getQuestionsOnScrolling();
        }
    }

    /**
     * To determine whether search field will be visible or hidden
     */
    showStatus() {
        return this.showSearchInput = this.matchString.length > 0;
    }

    // Open Add Category Dialog
    addCategoryDialog() {
        let adddialogRef = this.dialog.open(AddCategoryDialogComponent, { disableClose: true, hasBackdrop: true });
        adddialogRef.afterClosed().subscribe(categoryToAdd => {
            if (categoryToAdd !== '' && categoryToAdd !== undefined) {
                this.categoryArray.push(categoryToAdd);
                this.sortCategory();
            }
            this.isCategoryPresent = this.categoryArray.length === 0 ? false : true;
        });
    }

    // Open update Category Dialog
    updateCategoryDialog(category: Category) {
        let categoryToUpdate = this.categoryArray.find(x => x.id === category.id);
        let updateDialogRef = this.dialog.open(UpdateCategoryDialogComponent, { disableClose: true, hasBackdrop: true });
        updateDialogRef.componentInstance.category = JSON.parse(JSON.stringify(category));
        updateDialogRef.afterClosed().subscribe(updatedCategory => {
            if (updatedCategory !== '' && updatedCategory !== undefined) {
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
        let deleteDialogRef = this.dialog.open(DeleteCategoryDialogComponent, { disableClose: true, hasBackdrop: true });
        deleteDialogRef.componentInstance.category = category;
        deleteDialogRef.afterClosed().subscribe(
            deletedCategory => {
                if (deletedCategory) {
                    this.categoryArray.splice(this.categoryArray.indexOf(deletedCategory), 1);
                    this.isCategoryPresent = this.categoryArray.length === 0 ? false : true;
                }
            });
    }

    // Open delete question dialog
    deleteQuestionDialog(questionToDelete: Question) {
        let deleteDialogRef = this.dialog.open(DeleteQuestionDialogComponent, { disableClose: true, hasBackdrop: true });
        deleteDialogRef.componentInstance.question = questionToDelete;
        deleteDialogRef.afterClosed().subscribe(
            deletedQuestion => {
                if (deletedQuestion) {
                    this.question.splice(this.question.indexOf(deletedQuestion), 1);
                    this.questionDisplay.splice(this.questionDisplay.indexOf(deletedQuestion), 1);
                    this.countTheQuestion();
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
     * Calls server Api to count the number of questions
     */
    countTheQuestion() {
        this.questionsService.countTheQuestion(this.categroyId, this.matchString).subscribe((numberOfAllTypesOfQuestions) => {
            this.numberOfQuestions = numberOfAllTypesOfQuestions;
            this.easy = this.numberOfQuestions.easyCount;
            this.medium = this.numberOfQuestions.mediumCount;
            this.hard = this.numberOfQuestions.hardCount;
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

    /**
    * Selects the search text area on clicking of the search button
    * @param $event is of type Event and is used to call stopPropagation()
    * @param search is of type any
    */
    selectTextArea($event: any, search: any) {
        $event.stopPropagation();
        setTimeout(() => {
            search.select();
        }, 0);
    }

    /**
     * Select selected catgeory and difficulty level and pass it to route while adding question
     * @param questiontype type of the question
     */
    selectSelectionAndDifficultyType(questiontype: string) {
        let categoryName = this.selectedCategory.categoryName;
        let difficultyLevel = DifficultyLevel[this.selectedDifficulty];
        if (categoryName === undefined)
            categoryName = 'AllCategory';
        if (questiontype === 'single-answer')
            this.router.navigate(['questions', 'single-answer', 'add', categoryName, difficultyLevel]);
        else if (questiontype === 'multiple-answer')
            this.router.navigate(['questions', 'multiple-answers', 'add', categoryName, difficultyLevel]);
        else if (questiontype === 'programming')
            this.router.navigate(['questions', 'programming', 'add', categoryName, difficultyLevel]);
    }
}
﻿import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { Test } from '../../tests.model';
import { TestService } from '../../tests.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'create-test-footer',
    templateUrl: 'create-test-footer.html',
})

export class CreateTestFooterComponent implements OnInit {
    testId: number;
    isTestSection: boolean;
    isTestQuestion: boolean;
    isTestSettings: boolean;
    @Input('settingsForm')
    public settingsForm: NgForm;
    @Input('validStartDate')
    public validStartDate: boolean;
    @Input('validEndDate')
    public validEndDate: boolean;
    @Input('validTime')
    public validTime: boolean;
    @Output() saveTestSettings: any;
    @Output() launchTestDialog: any;
    @Output() SaveExit: any;
    @Output() SaveNext: any;

    constructor(private testService: TestService, public router: Router, private route: ActivatedRoute) {
        this.isTestSection = false;
        this.isTestQuestion = false;
        this.isTestSettings = false;
        this.saveTestSettings = new EventEmitter();
        this.launchTestDialog = new EventEmitter();
        this.SaveExit = new EventEmitter();
        this.SaveNext = new EventEmitter();
    }

    /**
     * Gets the Id of the Test from the route and fills the Settings saved for the selected Test in their respective fields
     */
    ngOnInit() {
        this.testId = this.route.snapshot.params['id'];
        this.getComponent();
    }

    /**
     * Displays the Component whose route matches that of the url
     */
    getComponent() {
        this.isTestSection = this.router.url === '/tests/sections/' + this.testId ? true : false;
        this.isTestQuestion = this.router.url === '/tests/' + this.testId +'/questions' ? true : false;
        this.isTestSettings = this.router.url === '/tests/' + this.testId + '/settings' ? true : false;
    }

    /**
     * Emits the event saveTestSettings 
     */
    updateTestSettings() {
        this.saveTestSettings.emit();
    }

    /**
     * Emits the event launchTestDialog
     */
    launchTestDialogBox() {
        this.launchTestDialog.emit();
    }
    /**
     * Emits the event SaveExit in test-questions.component
     */
    saveAndExit() {
        this.SaveExit.emit();
    }
    /**
     * Emits the event AddTestQuestionin test-questions.component
     */
    addTestQuestions() {
        this.SaveNext.emit();
    }
}

﻿import { Component, OnInit } from '@angular/core';
import { Test } from '../../tests/tests.model';
import { ConductService } from '../conduct.service';
import { ActivatedRoute } from '@angular/router';
import { Instruction } from '../instruction.model';
import { Router } from '@angular/router';


@Component({
    moduleId: module.id,
    selector: 'instructions',
    templateUrl: 'instructions.html',
})
export class InstructionsComponent implements OnInit {
    instruction: Instruction;
    loader: boolean;
    isErrorMessage: boolean;
    magicString: string;
    registrationUrl: string;

    constructor(private conductService: ConductService, private route: ActivatedRoute) {
        this.instruction = new Instruction();
    }

    /**
     *Gets the link from the url and pass it as parameter to a specific method to fetch all the information of a particular test
     */
    ngOnInit() {
        let url = window.location.pathname;
        let magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/instructions'));
        this.getTestDetailsByLink(magicString);
    }

    /**
     * This method is used to get all the instruction details before starting of a particular test
     * @param link Contains the link to fetch all test-instruction details related to a particular test
     */
    getTestDetailsByLink(testLink: string) {
        this.conductService.getTestDetailsByLink(testLink).subscribe(
            response => {
                this.instruction = response;
            });
    }
}

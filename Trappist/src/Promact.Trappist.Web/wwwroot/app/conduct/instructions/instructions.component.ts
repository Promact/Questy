import { Component, OnInit } from '@angular/core';
import { Test } from '../../tests/tests.model';
import { ConductService } from '../conduct.service';
import { ActivatedRoute } from '@angular/router';
import { TestInstructions } from '../testInstructions.model';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'instructions',
    templateUrl: 'instructions.html',
})

export class InstructionsComponent implements OnInit {
    testInstructions: TestInstructions;
    constructor(private conductService: ConductService, private route: ActivatedRoute, private router: Router) {
        this.testInstructions = new TestInstructions();
    }

    /**
     *Gets the link from the url and pass it as parameter to a specific method to fetch all the information of a particular test
     */
    ngOnInit() {
        let url = window.location.pathname;
        let magicString = url.substring(url.indexOf('/conduct/') + 9, url.indexOf('/instructions'));
        this.getTestInstructionsByLink(magicString);
    }

    /**
     * This method is used to get all the instructions before starting of a particular test
     * @param testLink Contains the link to fetch instructions related to a particular test
     */
    getTestInstructionsByLink(testLink: string) {
        this.conductService.getTestInstructionsByLink(testLink).subscribe(
            response => {
                this.testInstructions = response;
            });
    }

    startTest() {
        this.router.navigate(['test']);
    }
}

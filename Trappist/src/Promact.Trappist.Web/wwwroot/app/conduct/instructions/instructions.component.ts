import { Component, OnInit } from '@angular/core';
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
    testLink: string;
    loader: boolean;
    isErrorMessage: boolean;
    magicString: string;
    registrationUrl: string;

    constructor(private conductService: ConductService, private route: ActivatedRoute) {
        this.instruction = new Instruction();
    }

    /**
     *Gets the link from the route and pass it as parameter to the method for fetching all information of a particular test
     */
    ngOnInit() {
        this.testLink = this.route.snapshot.params['link'];
        this.getAllTestInformation(this.testLink);
    }

    /**
     * This method is used to get all the instruction details before starting of a particular test
     * @param link Contains the link to fetch all test-instruction details related to a particular test
     */
    getAllTestInformation(link: string) {
        this.conductService.getAllTestInformation(link).subscribe(
            response => {
                this.instruction = response;
            });
    }
}

import { Pipe, PipeTransform } from '@angular/core';
import { Test } from '../tests.model';

@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {

    transform(allTests: Test[], searchedTest: string) : any {
        if (searchedTest === undefined)
            return allTests;
        return allTests.filter(function (currentTest: Test) {
            let searchedTestLower = searchedTest.toLowerCase();
            let currentTestName = currentTest.testName.toLowerCase();
            if (currentTest.link === null)
                return (currentTestName.includes(searchedTestLower));
            else
                return (currentTestName.includes(searchedTestLower) || currentTest.link.toLowerCase().includes(searchedTestLower));
        });
    }
}
import { Pipe, PipeTransform } from '@angular/core';
import { Test } from '../tests.model';

@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {

    transform(allTests: Test[], searchedTest: string): any {

        if (!searchedTest || !searchedTest.trim())
            return allTests;
        return allTests.filter(function (currentTest: Test) {
            let searchedTestLower = searchedTest.toLowerCase();
            let currentTestName = currentTest.testName.toLowerCase();
            if (!currentTest.link)
                return (currentTestName.includes(searchedTestLower));
            else
                return (currentTestName.includes(searchedTestLower) || currentTest.link.toLowerCase().includes(searchedTestLower));
        });
    }
}
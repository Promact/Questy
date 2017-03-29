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
            if (currentTest.link === null)
                return (currentTest.testName.toLowerCase().includes(searchedTest.toLowerCase()));
            else
            return (currentTest.testName.toLowerCase().includes(searchedTest.toLowerCase()) || currentTest.link.toLowerCase().includes(searchedTest.toLowerCase()));
        });
    }
}
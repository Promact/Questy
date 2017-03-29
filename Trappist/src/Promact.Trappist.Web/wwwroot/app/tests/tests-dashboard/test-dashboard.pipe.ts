import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {

    transform(allTests: any, searchedTest: any): any {
        if (searchedTest === undefined)
            return allTests;
        return allTests.filter(function (currentTest: any) {
            if (currentTest.link === null)
                return (currentTest.testName.toLowerCase().includes(searchedTest.toLowerCase()))
            else
            return (currentTest.testName.toLowerCase().includes(searchedTest.toLowerCase()) || currentTest.link.toLowerCase().includes(searchedTest.toLowerCase()));
        });
    }
}
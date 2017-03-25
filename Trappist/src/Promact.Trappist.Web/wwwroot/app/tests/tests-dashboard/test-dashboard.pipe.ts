import { Pipe, PipeTransform } from '@angular/core';
@Pipe({

    name: 'filter'

})
export class FilterPipe implements PipeTransform {

    transform(value: any, term: any): any {
        if (term === undefined)
            return value;
        return value.filter(function (x: any) {
            if (x.link === null)
                return (x.testName.toLowerCase().includes(term.toLowerCase()));
            else
                return (x.testName.toLowerCase().includes(term.toLowerCase()) || x.link.toLowerCase().includes(term.toLowerCase()));
        });
    }
}
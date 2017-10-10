import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { Response, ResponseOptions } from '@angular/http';
import { MockTestData } from "../Mock_Data/test_data.mock";




export class TestServicesMock {



    getTests() {
        return Observable.of(MockTestData);
    }


}

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { SetupModule } from './setup/setup.module';

import 'rxjs/Rx';

platformBrowserDynamic().bootstrapModule(SetupModule);
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { ConductModule } from './conduct/conduct.module';

import 'rxjs/Rx';

platformBrowserDynamic().bootstrapModule(ConductModule);

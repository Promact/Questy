import { platformBrowser } from '@angular/platform-browser';

import { ConductModuleNgFactory } from './conduct/conduct.module.ngfactory';

platformBrowser().bootstrapModuleFactory(ConductModuleNgFactory);
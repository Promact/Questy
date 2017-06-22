import { platformBrowser } from '@angular/platform-browser';

import { SetupModuleNgFactory } from './setup/setup.module.ngfactory';

platformBrowser().bootstrapModuleFactory(SetupModuleNgFactory);
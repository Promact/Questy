import { platformBrowser } from '@angular/platform-browser';

import { SetupModuleNgFactory } from '../aot/app/setup/setup.module.ngfactory';

platformBrowser().bootstrapModuleFactory(SetupModuleNgFactory);
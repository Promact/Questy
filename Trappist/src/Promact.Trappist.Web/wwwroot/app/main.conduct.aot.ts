﻿import { platformBrowser } from '@angular/platform-browser';

import { ConductModuleNgFactory } from '../aot/app/conduct/conduct.module.ngfactory';

platformBrowser().bootstrapModuleFactory(ConductModuleNgFactory);
import { enableProdMode } from '@angular/core';

import { platformBrowser } from '@angular/platform-browser';

import { ConductModuleNgFactory } from '../aot/app/conduct/conduct.module.ngfactory';

enableProdMode();

platformBrowser().bootstrapModuleFactory(ConductModuleNgFactory);
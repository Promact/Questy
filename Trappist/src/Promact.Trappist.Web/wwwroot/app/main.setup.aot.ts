import { enableProdMode } from '@angular/core';

import { platformBrowser } from '@angular/platform-browser';

import { SetupModuleNgFactory } from './setup/setup.module.ngfactory';

enableProdMode();

platformBrowser().bootstrapModuleFactory(SetupModuleNgFactory);
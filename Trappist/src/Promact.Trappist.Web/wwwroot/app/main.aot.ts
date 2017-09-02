﻿import { enableProdMode } from "@angular/core";

import { platformBrowser } from '@angular/platform-browser';

import { AppModuleNgFactory } from '../aot/app/app.module.ngfactory';

platformBrowser().bootstrapModuleFactory(AppModuleNgFactory);

enableProdMode();
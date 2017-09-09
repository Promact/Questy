import 'reflect-metadata';
import 'zone.js';

import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { ConductModule } from './conduct/conduct.module';

if (module.hot) {
    module.hot.accept();
    module.hot.dispose(() => {
        modulePromise.then(appModule => {
            return appModule.destroy();
        });
    });
} else {
    enableProdMode();
}

const modulePromise = platformBrowserDynamic().bootstrapModule(ConductModule);
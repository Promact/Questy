import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { ConductModule } from './conduct/conduct.module';
import { enableProdMode } from '@angular/core';
import { environment } from './environments/environment';


if(environment.production) {
    enableProdMode();
}
platformBrowserDynamic().bootstrapModule(ConductModule);
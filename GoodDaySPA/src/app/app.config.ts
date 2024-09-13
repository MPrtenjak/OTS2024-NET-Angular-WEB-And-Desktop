import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HttpClient } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '@app/auth.interceptor';

import { routes } from '@app/app.routes';

import { ConfigService } from '@app/config.service';

import * as dayjs from 'dayjs';
import 'dayjs/locale/sl';
import 'dayjs/locale/en';

export function initializeApp(configService: ConfigService) {
  return async () => {
    await configService.loadConfig();
    dayjs.locale('sl'); 
  };
}

export function translateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: translateLoader,
          deps: [HttpClient]
        }
      })
    ), 
    { 
      provide: HTTP_INTERCEPTORS, 
      useClass: AuthInterceptor, 
      multi: true 
    },
    ConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [ConfigService],
      multi: true
    }    
  ]
};

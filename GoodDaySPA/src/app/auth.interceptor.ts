import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '@app/auth.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private translate: TranslateService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var currentLang = this.translate.currentLang || 'si';
    currentLang = currentLang === 'sl' ? 'si' : currentLang;

    var cloned = req.clone({
      headers: req.headers
        .set('Content-Type', 'application/json')
        .set('Accept', 'application/json')
        .set('Accept-Language', `${currentLang},en;q=0.5`)
    });

    console.log('AuthInterceptor: intercept I', cloned);
    const token = this.authService.getToken();
    if (token) {
      cloned = cloned.clone({
        headers: cloned.headers.set('Authorization', `Bearer ${token}`)
      });
    } 

    console.log('AuthInterceptor: intercept II', cloned);
    return next.handle(cloned);
  }
}

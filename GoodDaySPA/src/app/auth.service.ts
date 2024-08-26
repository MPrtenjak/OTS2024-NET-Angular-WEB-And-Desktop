import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { AuthInfo, userNotAuthenticated, userAuthenticated } from '@rest_data/authInfo';  

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper = new JwtHelperService();

  private authInfoSubject = new BehaviorSubject<AuthInfo>(userNotAuthenticated());
  authInfo$ = this.authInfoSubject.asObservable();

  constructor(private router: Router) {}

  storeAuthInfo(authInfo: AuthInfo): void {
    localStorage.setItem('jwtToken', authInfo.token!);
    localStorage.setItem('userName', authInfo.user!);
    localStorage.setItem('env', authInfo.environment!);
    localStorage.setItem('need-login', authInfo.NeedLogin.toString());

    this.authInfoSubject.next(authInfo);
  }
  
  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  isAuthenticated(): boolean {
    const authenticated = this.checkIfAuthenticated();    
    const env = this.getEnv();
    if (authenticated) {
      const token = this.getToken();
      const userName = this.getUserName();
      const needLogin = this.getNeedLogin();
      const authInfo = userAuthenticated(needLogin, token!, userName!, env!);

      this.authInfoSubject.next(authInfo);
    } else {
      this.authInfoSubject.next(userNotAuthenticated(env!));
    }

    return authenticated;
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('userName');    
    localStorage.removeItem('env');    
    localStorage.removeItem('need-login');

    this.authInfoSubject.next(userNotAuthenticated());

    this.router.navigate(['/login']);
  }

  private checkIfAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }

    const decodedToken = this.decodeToken(token);
    if (!decodedToken) {
      return false;
    }

    const expirationDate = decodedToken.exp * 1000;
    const currentDate = new Date().getTime();

    return expirationDate > currentDate;
  }

  private decodeToken(token: string): any {
    try {
      return this.jwtHelper.decodeToken(token);
    } catch(Error) {
      return null;
    }
  }

  private getNeedLogin(): boolean {
    const needLoginStr = localStorage.getItem('need-login');
    return needLoginStr === "true";
  }

  private getEnv(): string | null {
    return localStorage.getItem('env');
  }

  private getUserName(): string | null {
    return localStorage.getItem('userName');
  }
}
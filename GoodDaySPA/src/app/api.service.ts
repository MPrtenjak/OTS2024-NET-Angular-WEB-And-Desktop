import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ConfigService } from '@app/config.service';
import { AuthService } from '@app/auth.service';
import { Gratitude } from '@rest_data/gratitude';
import { map } from 'rxjs/operators';
import { userAuthenticated, userNotAuthenticated } from '@rest_data/authInfo';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService, private authService: AuthService) {
    this.apiUrl = configService.apiUrl;
  }

  userTryLogin(): Observable<any> {
    return this.tryLoginAndGetAdminInfo().pipe(
      tap((response: any) => this.storeAuthInfo(false, response)),
      map((response: any) => response[0])
    );
  }

  userLogin(username: string, password: string): Observable<any> {
    return this.loginAndGetAdminInfo(username, password).pipe(
      tap((response: any) => this.storeAuthInfo(true, response)),
      map((response: any) => response[0])
    );
  }
 
  userSignUp(username: string, password1: string, password2: string): Observable<any> {
    var body = {
       "userName": username, 
       "password1": password1, 
       "password2": password2 
    };

    return this.http.post(`${this.apiUrl}/users/sign-up`, body);
  }

  adminInfo(): Observable<any> {
    return this.http.get(`${this.apiUrl}/admin/info`);
  }  

  adminRandomizeData(): Observable<any> {
    return this.http.post(`${this.apiUrl}/admin/randomize-data`, {});
  }  

  gratitude(month: number, year: number): Observable<Gratitude[]> {
    const formattedMonth = month.toString().padStart(2, '0');

    return this.http.get<any[]>(`${this.apiUrl}/gratitude/${year}-${formattedMonth}`)
      .pipe(
        map(response => response.map(item => ({
          ...item,
          date: new Date(item.date) // Convert date string to Date object if necessary
        }) as Gratitude))
      );
  }


  saveGratitude(day: string, content: string[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/gratitude`, { date: day, content });    
  }

  loginAndGetAdminInfo(username: string, password: string): Observable<any> {
    const callLogin = this.http.post(`${this.apiUrl}/users/login`, { username, password }).pipe(
      catchError(error => of(error))
    );    

    const callAdminInfo = this.http.get(`${this.apiUrl}/admin/info`).pipe(
      catchError(error => of(error))
    );    

    return forkJoin([callLogin, callAdminInfo]);
  }  

  tryLoginAndGetAdminInfo(): Observable<any> {
    const callTryLogin = this.http.post(`${this.apiUrl}/users/try-login`, {}).pipe(
      catchError(error => of(error))
    );    

    const callAdminInfo = this.http.get(`${this.apiUrl}/admin/info`).pipe(
      catchError(error => of(error))
    );    

    return forkJoin([callTryLogin, callAdminInfo]);
  }  

  private storeAuthInfo(needLogin: boolean, loginResponse: any): void {
    const loginFunctionResponse: any = loginResponse[0];
    const adminInfoResponse: any = loginResponse[1];

    const loginError = (loginFunctionResponse instanceof HttpErrorResponse);
    const adminInfoError = (adminInfoResponse instanceof HttpErrorResponse);

    const token: string = (loginError) ? '' : loginFunctionResponse.token;
    const userName: string = (loginError) ? '' : loginFunctionResponse.userName;
    const environmentWithoutSpaces: string = (adminInfoError) ? '' : adminInfoResponse.currentEnvironment;
    const environment: string = this.splitAndExcludeLast(environmentWithoutSpaces);

    const authInfo = (loginError) 
      ? userNotAuthenticated(environment)
      : userAuthenticated(needLogin, token, userName, environment);

    this.authService.storeAuthInfo(authInfo);
  }

  private splitAndExcludeLast(str: string): string {
    const splitStr = str.split(/(?=[A-Z])/).slice(0, -1);
    return splitStr.join(' ');
  }  
}
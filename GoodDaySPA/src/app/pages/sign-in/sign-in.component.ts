import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '@app/api.service';
import { InfoComponent } from '@pages/info/info.component';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

interface SignUpError {
  detail: string;
  UI: {
    UserName: string | null;
    Password1: string | null;
    Password2: string | null;
  };
}

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [CommonModule, InfoComponent, FormsModule],
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  username: string = '';
  password1: string = '';
  password2: string = '';

  usernameError: string | null = null;
  password1Error: string | null = null;
  password2Error: string | null = null;
  signInError: string | null = null;

  constructor(private apiService: ApiService, private router: Router) {}  

  ngOnInit(): void {
    this.apiService.userTryLogin().subscribe(
      {
        next: response => {
          if (!(response instanceof HttpErrorResponse))
            this.router.navigate(['/notes'])
        }
      }
    );
  }  

  signIn() {
    this.usernameError = null;
    this.password1Error = null;
    this.password2Error = null;
    this.signInError = null;

    this.apiService.userSignUp(this.username, this.password1, this.password2).subscribe(
      response => {
        this.apiService.userLogin(this.username, this.password1).subscribe(
          response => {
            this.router.navigate(['/notes']);
          }
        );
      },
      (error: any) => {
        const signUpError: SignUpError = error.error;
        if (!signUpError) 
          return;

        this.signInError = signUpError.detail;
        this.usernameError = signUpError.UI.UserName;
        this.password1Error = signUpError.UI.Password1;
        this.password2Error = signUpError.UI.Password2;
      }
    );
  }
}
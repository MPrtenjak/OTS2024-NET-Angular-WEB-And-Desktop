import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '@app/api.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';  
  loginError: string | null = null;

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

  login() {
    this.loginError = null;

    this.apiService.userLogin(this.username, this.password).subscribe(
      {
        next: response => {
          if (response instanceof HttpErrorResponse)
            this.loginError = response.error.detail;
          else
            this.router.navigate(['/notes']);
        }
      }
    );
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: any;

  constructor(private http: HttpClient) { }

  async loadConfig() {
    const response = await fetch('assets/config.json');
    this.config = (response.ok) 
      ? await response.json()
      : { apiUrl: window.location.origin };
  }

  get apiUrl(): string {
    return this.config.apiUrl;
  }
}

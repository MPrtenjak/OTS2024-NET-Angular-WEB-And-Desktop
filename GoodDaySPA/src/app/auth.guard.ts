import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '@app/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  private loginUrls: string[] = ['/login', '/sign-in'];  
  private publicUrls: string[] = ['/slideshow'];

  private matchesPath(path: string, paths: string[]): boolean {
    return paths.some(p => path.startsWith(p));
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.isAuthenticated() 
      || this.matchesPath(state.url, this.publicUrls)) {
      return true;
    }

    if (!this.loginUrls.includes(state.url)) {
      this.router.navigate(['/login']);
    }      
      
    return true;
  }
}
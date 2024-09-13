import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common'; 
import { AuthService } from '@app/auth.service';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { AuthInfo, userNotAuthenticated } from '@rest_data/authInfo';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { GooddayNavbarComponent } from '@components/goodday-navbar/goodday-navbar.component';
import * as dayjs from 'dayjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    CollapseModule,
    GooddayNavbarComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  authInfo: AuthInfo = userNotAuthenticated();
  isCollapsed: boolean = false;
  private authInfoSubscription: Subscription | null = null;
  private routerSubscription: Subscription | null = null;  
  isSlideshowRoute: boolean = false;

  constructor(private authService: AuthService, private router: Router, private translate: TranslateService) {
    translate.addLangs(['sl', 'en']); 
    translate.setDefaultLang('sl'); 
  }

  changeLanguage(lang: string) {
    this.translate.use(lang);
  }

  ngOnInit(): void {
    this.authInfoSubscription = this.authService.authInfo$.subscribe(authInfo => {
      this.authInfo = authInfo;
    });

    this.routerSubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event) => {
      const navigationEndEvent = event as NavigationEnd;
      this.isSlideshowRoute = navigationEndEvent.url.includes('/slideshow');
    });
    
    /*
    // does not work!
    ).subscribe((event: NavigationEnd) => {
      this.isSlideshowRoute = event.url.includes('/slideshow');
    });
    */
  }  

  ngOnDestroy(): void {
    if (this.authInfoSubscription) {
      this.authInfoSubscription.unsubscribe();
    }
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }    
  }

  logout(): void {
    this.authService.logout();
    this.authInfo = userNotAuthenticated();
  }  
}

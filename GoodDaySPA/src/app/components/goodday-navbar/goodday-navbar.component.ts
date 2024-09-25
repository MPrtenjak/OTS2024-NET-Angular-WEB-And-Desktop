
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { AuthInfo } from '@rest_data/authInfo';
import { TranslateModule} from '@ngx-translate/core';
import { AppComponent } from '@app/app.component';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-goodday-navbar',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterModule],
  templateUrl: './goodday-navbar.component.html',
  styleUrl: './goodday-navbar.component.scss'
})
export class GooddayNavbarComponent {
  @Input() authInfo!: AuthInfo;
  @Output() logoutClick = new EventEmitter<void>();
  cnt: number = 0;

  constructor(private appComponent: AppComponent, private router: Router) {} // Inject AppComponent

  isCollapsed: boolean = true;

  logout(): void {
    this.logoutClick.emit();
  }

  lang(): void {
    this.cnt = ++this.cnt % 2;

    this.appComponent.changeLanguage((this.cnt === 1) ? 'en' : 'sl');
  }

  reroute(newRoute: string): void {
    this.router.navigate(['/notes']);
  }
}

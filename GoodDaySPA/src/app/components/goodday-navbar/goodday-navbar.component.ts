
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { AuthInfo } from '@rest_data/authInfo';
import { TranslateModule} from '@ngx-translate/core';
import { AppComponent } from '@app/app.component';

@Component({
  selector: 'app-goodday-navbar',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './goodday-navbar.component.html',
  styleUrl: './goodday-navbar.component.scss'
})
export class GooddayNavbarComponent {
  @Input() authInfo!: AuthInfo;
  @Output() logoutClick = new EventEmitter<void>();
  cnt: number = 0;

  constructor(private appComponent: AppComponent) {} // Inject AppComponent

  isCollapsed: boolean = true;

  logout(): void {
    this.logoutClick.emit();
  }

  lang(): void {
    this.cnt = ++this.cnt % 2;

    this.appComponent.changeLanguage((this.cnt === 0) ? 'en' : 'sl');
  }
}

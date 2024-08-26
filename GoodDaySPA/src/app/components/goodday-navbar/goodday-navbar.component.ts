
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { AuthInfo } from '@rest_data/authInfo';

@Component({
  selector: 'app-goodday-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './goodday-navbar.component.html',
  styleUrl: './goodday-navbar.component.scss'
})
export class GooddayNavbarComponent {
  @Input() authInfo!: AuthInfo;
  @Output() logoutClick = new EventEmitter<void>();

  isCollapsed: boolean = true;

  logout(): void {
    this.logoutClick.emit();
  }
}

import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/api.service';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-info',
  standalone: true,
  imports: [CommonModule, TranslateModule],  
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {
  infoData: any;
  error: string | null = null;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.apiService.adminInfo().subscribe(
      data => {
        this.infoData = data;
      },
      error => {
        this.error = 'Error fetching info data';
        console.error('Error fetching info data', error);
      }
    );
  }
}
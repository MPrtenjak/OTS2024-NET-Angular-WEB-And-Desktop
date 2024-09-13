import { Component, Input, Output, EventEmitter, OnInit, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '@app/api.service';
import dayjs, { Dayjs } from 'dayjs'
import { GratitudeByDate } from '@rest_data/gratitudeByDate';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-gratitude',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './gratitude.component.html',
  styleUrl: './gratitude.component.scss'
})
export class GratitudeComponent implements OnInit {
  @Input() gratitudeByDate!: GratitudeByDate;
  @Output() gratitudeUpdated: EventEmitter<void> = new EventEmitter();
  gratitude1: string = '';  
  gratitude2: string = '';  
  gratitude3: string = '';  
  gratitudes: string[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['gratitudeByDate']) {
      this.gratitude1 = this.gratitudeByDate.content[0] || '';
      this.gratitude2 = this.gratitudeByDate.content[1] || '';
      this.gratitude3 = this.gratitudeByDate.content[2] || '';
      this.gratitudes = [this.gratitude1, this.gratitude2, this.gratitude3]
        .filter((value) => value.trim() !== '');
    }    
  }  

  get active_date(): Dayjs {
    return dayjs(this.gratitudeByDate.date);
  }

  get formatedActiveDate(): string {
    return this.active_date.format('YYYY-MM-DD');
  }

  get formatedActiveDateSi(): string {
    return this.active_date.format('dddd, DD. MM. YYYY');
  }

  get isReadOnly(): boolean {
    const today = dayjs();
    const minDate = today.add(-3, 'day');

    return this.active_date.isBefore(minDate, 'day') || this.active_date.isAfter(today, 'day');
  }

  get showComponent(): boolean {
    if (!this.isReadOnly) 
      return true;

    return this.gratitude1.length > 0
      || this.gratitude2.length > 0
      || this.gratitude3.length > 0;
  }

  saveData() : void {
    const content = [this.gratitude1, this.gratitude2, this.gratitude3];
    this.apiService.saveGratitude(this.formatedActiveDate, content).subscribe({
      next: (response) => {
        this.gratitudeUpdated.emit(); 
      },
      error: (error) => {
        console.error('Error saving gratitude', error);
      }
    });
  }  
}
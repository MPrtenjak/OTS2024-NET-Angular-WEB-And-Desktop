import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { MonthDaysComponent } from "@components/month-days/month-days.component";
import { GratitudeComponent } from "@components/gratitude/gratitude.component";
import { GratitudeByDate } from '@rest_data/gratitudeByDate';
import dayjs, { Dayjs } from 'dayjs'

@Component({
  selector: 'app-notes',
  standalone: true,
  imports: [MonthDaysComponent, GratitudeComponent, CommonModule],
  templateUrl: './notes.component.html',
  styleUrl: './notes.component.scss'
})
export class NotesComponent {
}

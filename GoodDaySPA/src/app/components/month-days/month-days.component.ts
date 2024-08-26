import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '@app/api.service';
import { Gratitude } from '@rest_data/gratitude';
import { GratitudeComponent } from '@components/gratitude/gratitude.component';
import { GratitudeByDate, transformGratitudeToGratitudeByDate, transformDateToString } from '@rest_data/gratitudeByDate';
import { firstValueFrom } from 'rxjs';
import dayjs, { Dayjs } from 'dayjs'

@Component({
  selector: 'app-month-days',
  standalone: true,
  imports: [CommonModule, GratitudeComponent],
  templateUrl: './month-days.component.html',
  styleUrl: './month-days.component.scss'
})
export class MonthDaysComponent {
  private _active_date: Dayjs = dayjs();
  gratitudes: Gratitude[] = [];
  gratitudesByDate: GratitudeByDate[] = [];
  gratitudesByDateMap: Map<string, GratitudeByDate> = new Map();
  gratitudesOnActiveDay: GratitudeByDate = { date: '', content: [] };
  weeks:(Date | null)[][] = [];
  nameOfDays: string[] = [];
  today = dayjs();
  last_read_month: number = -1;

  get active_date(): Dayjs {
    return this._active_date;
  }

  set active_date(value: Dayjs) {
    if (!this._active_date.isSame(value)) {
      this._active_date = value;
      this.onActiveDateChange();
    }
  }

  constructor(private apiService: ApiService) {
    this.getDayNames();
    this.onActiveDateChange();
  }

  private getDayNames() {
    for (let i = 1; i <= 7; i++) {
      let dayName = dayjs().day(i).format('ddd');
      dayName = dayName.charAt(0).toUpperCase() + dayName.slice(1);
      if (dayName.endsWith('.')) {
        dayName = dayName.slice(0, -1);
      }

      this.nameOfDays.push(dayName);
    }
  }

  get monthName(): string {
    return this.active_date.format('MMMM YYYY').toLocaleUpperCase();
  }

  get prevMonthName(): string {
    return this.active_date.add(-1, 'month').format('MMMM')
  }

  get nextMonthName(): string {
    return this.active_date.add(1, 'month').format('MMMM')
  }  

  onDateClick(date: Date | null) {
    if (date) {
      const gbd = this.getGratitudesByDate(date);
      this.active_date = dayjs(gbd.date);
    }
  }  

  onPrevMonthClick(): void {
    this.active_date = this.active_date.add(-1, 'month')    
  }

  onNextMonthClick(): void {
    this.active_date = this.active_date.add(1, 'month')    
  }

  isNextMonthDisabled(): boolean {
    const today = new Date();

    return dayjs(today).month() <= this.active_date.month() && dayjs(today).year() <= this.active_date.year();
  }  

  onActiveDateChange(): void {
    this.gratitudesOnActiveDay = this.activeGratitudeByDate();

    if (this.last_read_month == this.active_date.month()) 
      return;

    this.handleGratitudeUpdate();
  }

  generateMonthDays(): void {
    const month = this.active_date.month();

    const firstDayOfMonth = this.active_date.startOf('month').toDate();
    const lastDayOfMonth = this.active_date.endOf('month').toDate();

    let currentDay = new Date(firstDayOfMonth);
    currentDay.setDate(currentDay.getDate() - currentDay.getDay() + (currentDay.getDay() === 0 ? -6 : 1)); 

    this.weeks = [];
    while (currentDay <= lastDayOfMonth) {
      const week: (Date | null)[] = [];
      for (let i = 0; i < 7; i++) { 
        if (currentDay.getMonth() === month) {
          week.push(new Date(currentDay));
        } else {
          week.push(null);
        }

        currentDay.setDate(currentDay.getDate() + 1);
      }
      this.weeks.push(week);
    }

    this.last_read_month = this.active_date.month();
  }

  async readGratitudes(): Promise<void> {
    try {
      this.gratitudes = await firstValueFrom(this.apiService.gratitude(this.active_date.month() + 1, this.active_date.year()));
      this.gratitudesByDate = transformGratitudeToGratitudeByDate(this.gratitudes);

      this.gratitudesByDateMap.clear();
      this.gratitudesByDate.forEach(gratitude => {
        this.gratitudesByDateMap.set(gratitude.date, gratitude);
      });      
    } catch (error) {
      console.error('Error fetching gratitudes:', error);
    }
  }

  getGratitudesByDate(date: Date): GratitudeByDate {
    return this.gratitudesByDateMap.get(transformDateToString(date)) || { date: transformDateToString(date), content: [] };
  }

  countNumberOfGratitudes(date: Date): number {
    const gbd = this.getGratitudesByDate(date);
    return gbd.content.length;
  }

  getClassForDay(day: Date) : string[] {
    var classes: string[] = ['btn', 'position-relative'];

    if (dayjs(day).isSame(this.today, 'day')) {
      classes.push('btn-success');
    }
    else
    {
      if (dayjs(day).isBefore(this.today, 'day')) {
        if (this.isDayEditable(day))
          classes.push('editable');          

        classes.push('btn-warning');
      }
      else
        classes.push('btn-warning-primary');
      }

      if (dayjs(day).isSame(this.active_date, 'day')) {
        classes.push('btn-lg current');      
      }

      return classes;
  }

  isDayDisabled(day: Date) : boolean {
    return (dayjs(day).isAfter(this.today, 'day')); 
  }

  isDayEditable(day: Date): boolean {
    const minDate = this.today.add(-3, 'day');

    return !(dayjs(day).isBefore(minDate, 'day') || dayjs(day).isAfter(this.today, 'day'));
  }  

  activeGratitudeByDate(): GratitudeByDate {
    return this.getGratitudesByDate(this.active_date.toDate());
  }  

  handleGratitudeUpdate(): void {
    this.readGratitudes().then(() => {
      this.generateMonthDays();
    });
  }

  async setRandomData(): Promise<void> {
    try {
      await firstValueFrom(this.apiService.adminRandomizeData());
      this.handleGratitudeUpdate();
    } catch (error) {
      console.error('Error fetching random data:', error);
    }
  }  
}
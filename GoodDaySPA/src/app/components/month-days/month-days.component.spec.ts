import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthDaysComponent } from '@components/month-days.component';

describe('MonthDaysComponent', () => {
  let component: MonthDaysComponent;
  let fixture: ComponentFixture<MonthDaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonthDaysComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthDaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

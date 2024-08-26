import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GooddayNavbarComponent } from './goodday-navbar.component';

describe('GooddayNavbarComponent', () => {
  let component: GooddayNavbarComponent;
  let fixture: ComponentFixture<GooddayNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GooddayNavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GooddayNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

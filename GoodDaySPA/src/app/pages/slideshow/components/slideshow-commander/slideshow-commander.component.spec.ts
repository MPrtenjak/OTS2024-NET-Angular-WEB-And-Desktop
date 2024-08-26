import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlideshowCommanderComponent } from './slideshow-commander.component';

describe('SlideshowCommanderComponent', () => {
  let component: SlideshowCommanderComponent;
  let fixture: ComponentFixture<SlideshowCommanderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SlideshowCommanderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SlideshowCommanderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlideshowPresenterComponent } from './slideshow-presenter.component';

describe('SlideshowPresenterComponent', () => {
  let component: SlideshowPresenterComponent;
  let fixture: ComponentFixture<SlideshowPresenterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SlideshowPresenterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SlideshowPresenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

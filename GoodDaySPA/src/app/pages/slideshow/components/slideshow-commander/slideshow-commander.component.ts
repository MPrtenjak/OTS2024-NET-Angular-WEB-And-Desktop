import { Component, EventEmitter, Input, Output, HostListener } from '@angular/core';
import { SlideShowPosition, initialSlideshowPosition } from '../../data/slideshowData';
import { Router } from '@angular/router';

@Component({
  selector: 'app-slideshow-commander',
  standalone: true,
  imports: [],
  templateUrl: './slideshow-commander.component.html',
  styleUrl: './slideshow-commander.component.scss'
})
export class SlideshowCommanderComponent {
  @Input() slideShowPosition: SlideShowPosition = initialSlideshowPosition;
  @Output() zoomChange = new EventEmitter<number>();
  @Output() slideChange = new EventEmitter<number>();

  constructor(private router: Router) {}  

  @HostListener('window:keydown', ['$event'])
  handleKeyDown(event: KeyboardEvent) {
    if (event.key === 'ArrowRight' || event.key === 'ArrowUp' || event.key === 'PageUp') this.nextSlide();
    if (event.key === 'ArrowLeft' || event.key === 'ArrowDown' || event.key === 'PageDown') this.previousSlide();

    if (event.key === 'Home' || event.key === 'AudioVolumeUp') this.zoomIn();
    if (event.key === 'End' || event.key === 'AudioVolumeDown') this.zoomOut();

    if (event.key === 'Escape') this.gotoNotesPage();
  }  

  get ZoomInEnabled(): boolean {
    return this.slideShowPosition.currentZoom < this.slideShowPosition.maxZoom;
  }

  get ZoomOutEnabled(): boolean {
    return this.slideShowPosition.currentZoom > this.slideShowPosition.minZoom;
  }

  get NextSlideEnabled(): boolean {
    return this.slideShowPosition.currentSlide < this.slideShowPosition.lastSlide;
  }

  get PreviousSlideEnabled(): boolean {
    return this.slideShowPosition.currentSlide > 1;
  }

  zoomIn(): void {
    if (this.slideShowPosition.currentZoom >= this.slideShowPosition.maxZoom) { 
      return; 
    }

    this.zoomChange.emit(this.slideShowPosition.currentZoom + 10);
  }

  zoomOut(): void {
    if (this.slideShowPosition.currentZoom <= this.slideShowPosition.minZoom) { 
      return;
    }

    this.zoomChange.emit(this.slideShowPosition.currentZoom - 10);
  } 

  nextSlide(): void {
    if (this.slideShowPosition.currentSlide >= this.slideShowPosition.lastSlide) { 
      return;
    }

    this.slideChange.emit(this.slideShowPosition.currentSlide + 1);
  } 

  previousSlide(): void {
    if (this.slideShowPosition.currentSlide <= 1) { 
      return;
    }

    this.slideChange.emit(this.slideShowPosition.currentSlide - 1);
  } 

  gotoNotesPage(): void {
    this.router.navigate(['/notes']);
  }
}


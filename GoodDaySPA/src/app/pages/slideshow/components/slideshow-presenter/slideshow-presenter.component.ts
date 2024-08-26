import { CommonModule } from '@angular/common';
import { Component, Input, ViewChild, ElementRef, SimpleChanges } from '@angular/core';
import { SlideData } from '../../data/slideshowData';
import hljs from 'highlight.js';

@Component({
  selector: 'app-slideshow-presenter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './slideshow-presenter.component.html',
  styleUrl: './slideshow-presenter.component.scss'
})
export class SlideshowPresenterComponent {
  @Input() currentSlide: SlideData | null = null;
  @ViewChild('slideTitle', { static: false }) slideTitle!: ElementRef;

  // execute code whenever the current slide changes
  ngAfterViewChecked(): void {
    this.triggerAnimation();

    document.querySelectorAll('pre').forEach((el) => {
      hljs.highlightElement(el as HTMLElement);
    });
  }

  triggerAnimation(): void {
    if (!this.slideTitle) 
      return;
    
    const element = this.slideTitle?.nativeElement;
    element.classList.remove('animate__animated', 'animate__zoomIn');

    // Trigger reflow to restart the animation
    void element.offsetWidth;
    element.classList.add('animate__animated', 'animate__zoomIn');
  }  
}

import { CommonModule, Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { SlideshowCommanderComponent } from './components/slideshow-commander/slideshow-commander.component';
import { SlideshowPresenterComponent } from './components/slideshow-presenter/slideshow-presenter.component';
import { SlideShowPosition, SlideData, initialSlideshowPosition } from './data/slideshowData';
import { TranslateService, LangChangeEvent, TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-slideshow',
  standalone: true,
  imports: [CommonModule, CarouselModule, SlideshowCommanderComponent, SlideshowPresenterComponent, TranslateModule],
  templateUrl: './slideshow.component.html',
  styleUrls: ['./slideshow.component.scss']
})
export class SlideshowComponent {
  slideData: SlideData[] = [];
  slideShowPosition: SlideShowPosition = initialSlideshowPosition;

  constructor(private route: ActivatedRoute, private location: Location, private translate: TranslateService) {
    this.loadSlides(translate.currentLang);
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      // wait until the slides are loaded
      if (this.slideData.length === 0) {
        setTimeout(() => this.ngOnInit(), 100);
        return;
      }

      const slideId = this.getSlideIdFromRoute(params);
      this.slideShowPosition.currentSlide = slideId;
    });

    this.translate.onLangChange.subscribe((event: LangChangeEvent) => {
      this.loadSlides(event.lang);
    });
  }

  async loadSlides(lang: string | null = null) {
    const language = lang || 'sl'; 
    const language_file = `/assets/slides-${language}.xml`;
    const response = await fetch(language_file);
    if (response.ok) {
      const xmlText = await response.text();
      const parser = new DOMParser();
      const xmlDoc = parser.parseFromString(xmlText, 'application/xml');
      const slides = xmlDoc.getElementsByTagName('slide');
      this.slideData = Array.from(slides).map(slide => {
        const caption = slide.getElementsByTagName('caption')[0].innerHTML || '';
        const content = slide.getElementsByTagName('content')[0].innerHTML || '';
        return { caption, content };
      });
    } else {
      this.slideData = [];
    }

    this.slideShowPosition.lastSlide = this.slideData.length;      

    const savedZoom = localStorage.getItem('zoom');
    if (savedZoom) {
      this.slideShowPosition.currentZoom = parseInt(savedZoom);
    }
  }  

  get Zoom(): string {
    return this.slideShowPosition.currentZoom + "%";
  }  

  get CurrentSlide(): SlideData | null {
    const index = this.slideShowPosition.currentSlide - 1;
    if (this.slideData[index]) {
      return this.slideData[index];
    }

    return null;
  }

  getSlideIdFromRoute(params: ParamMap): number {
    if (!params.has('slide-id'))
      return 1;

    const slideIdFromUrl : string = params.get('slide-id') || '1';
    var slideIdNumber : number = parseInt(slideIdFromUrl);
    slideIdNumber = (isNaN(slideIdNumber)) ? 1 : slideIdNumber;
    slideIdNumber = (slideIdNumber < 1) ? 1 : slideIdNumber;
    slideIdNumber = (slideIdNumber > this.slideShowPosition.lastSlide) ? this.slideShowPosition.lastSlide : slideIdNumber;

    return slideIdNumber;
  }

  zoomChange(zoom: number): void {
    this.slideShowPosition.currentZoom = zoom;
    localStorage.setItem('zoom', zoom.toString());
  }

  slideChange(slide: number): void {
    this.slideShowPosition.currentSlide = slide;

    this.location.replaceState(`/slideshow/${slide}`);    
  }
}

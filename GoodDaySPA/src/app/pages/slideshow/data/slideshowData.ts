export interface SlideShowPosition {
    currentSlide: number;
    lastSlide: number;

    currentZoom: number;
    minZoom: number;
    maxZoom: number;
}

export interface SlideData {
    caption: string;
    content: string;
}

export const initialSlideshowPosition: SlideShowPosition = {
    currentSlide: 1,
    lastSlide: 4,
    currentZoom: 160,
    minZoom: 50,
    maxZoom: 500
};

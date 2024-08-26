import { Gratitude } from '@rest_data/gratitude';
import dayjs from 'dayjs'

export interface GratitudeByDate {
  date: string; // yyyy-MM-dd
  content: string[];
}

export function transformGratitudeToGratitudeByDate(gratitudes: Gratitude[]): GratitudeByDate[] {
  const gratitudeMap = new Map<string, string[]>();

  gratitudes.forEach(gratitude => {
    const dateKey = transformDateToString(gratitude.date);
    if (!gratitudeMap.has(dateKey)) {
      gratitudeMap.set(dateKey, [gratitude.content]);
    } else {
      gratitudeMap.get(dateKey)?.push(gratitude.content);
    }
  });

  const result: GratitudeByDate[] = [];
  gratitudeMap.forEach((content, date) => {
    result.push({
      date,
      content
    });
  });

  return result;
}

export function transformDateToString(date: Date): string {
  return dayjs(date).format('YYYY-MM-DD');
}
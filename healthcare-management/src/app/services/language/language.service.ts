import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {isPlatformBrowser} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class LanguageService
{
  private readonly isBrowser: boolean;
  constructor(private readonly translateService: TranslateService,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public setLanguage(): void
  {
    if(!this.isBrowser)
      return;

    const language = localStorage.getItem('language') || this.getDefaultLanguage();

    localStorage.setItem('language', language);
    this.translateService.use(language);
    document.documentElement.lang = language
  }

  public changeLanguage(language: string): void
  {
    if(!this.isBrowser)
      return;

    localStorage.setItem('language', language);
    location.reload();
  }

  public getLanguage(): string
  {
    if(!this.isBrowser)
      return "";

    return this.translateService.currentLang;
  }

  public getLanguages(): Map<string, string>
  {
    return new Map<string, string>([
      ['en', 'English'],
      ['ro', 'Română'],
    ]);
  }

  private getDefaultLanguage(): string
  {
    return 'en';
  }
}

import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {MatIcon} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatTooltip} from '@angular/material/tooltip';
import {LanguageService} from '../../services/language/language.service';
import {MatMenu, MatMenuModule, MatMenuTrigger} from '@angular/material/menu';
import {KeyValuePipe, NgClass, NgFor} from '@angular/common';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-language-picker',
  imports: [
    MatIcon,
    MatButtonModule,
    MatMenuModule,
    MatIcon,
    MatTooltip,
    MatMenu,
    NgClass,
    KeyValuePipe,
    NgFor,
    MatMenuTrigger,
    TranslatePipe
  ],
  templateUrl: './language-picker.component.html',
  styleUrl: './language-picker.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class LanguagePickerComponent implements OnInit{

  public languages!: Map<string,string>;

  constructor(private readonly languageService: LanguageService) {
  }

  ngOnInit(): void
  {
    this.languages = this.languageService.getLanguages();
  }

  public changeLanguage(language: string): void
  {
    this.languageService.changeLanguage(language);
  }

  public isLanguageSelected(language: string): boolean
  {
    return this.languageService.getLanguage() === language;
  }
}

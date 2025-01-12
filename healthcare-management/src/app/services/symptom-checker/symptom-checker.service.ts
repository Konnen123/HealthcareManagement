import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {firstValueFrom} from 'rxjs';
import {SymptomClient} from '../../clients/symptom.client';

@Injectable({
  providedIn: 'root'
})
export class SymptomService
{
  private readonly isBrowser: boolean
  constructor(private readonly symptomClient: SymptomClient,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public async predictAsync(symptomsList: any): Promise<any> {
    try {
      return await firstValueFrom(this.symptomClient.predict(symptomsList));
    } catch (error){
      throw error;
    }
  }

}

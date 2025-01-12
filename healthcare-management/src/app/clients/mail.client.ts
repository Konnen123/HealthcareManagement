import {Inject, Injectable} from '@angular/core';
import {APP_SERVICE_CONFIG} from '../app-config/app.config';
import {AppConfig} from '../app-config/app.config.interface';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, throwError} from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class MailClient {
  private readonly baseUrl: string;
  constructor(@Inject(APP_SERVICE_CONFIG) readonly config: AppConfig, readonly http: HttpClient)
  {
    this.baseUrl = this.config.apiEndpoint + '/v1/Mail'
  }

  public sendForgotPasswordEmail(email: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/forgot-password`, { email }, { responseType: 'text' }).pipe(
      map(response => {
        return { message: response };
      }),
      catchError(error => {
        if (typeof error.error === 'string') {
          try {
            const parsedError = JSON.parse(error.error);
            return throwError(() => parsedError);
          } catch (parseError) {
            return throwError(() => error);
          }
        }
        return throwError(() => error);
      })
    );
  }


}

import {HttpEvent, HttpInterceptorFn} from '@angular/common/http';
import {catchError, from, mergeMap, Observable, tap, throwError} from 'rxjs';
import {AuthenticationService} from '../services/authentication/authentication.service';
import {inject} from '@angular/core';
import {Router} from '@angular/router';

export const authenticationInterceptor: HttpInterceptorFn = (req, next): Observable<HttpEvent<unknown>> =>
{
  const authenticationService = inject(AuthenticationService);
  const router = inject(Router);

  const token = authenticationService.getCookie('token');
  const refreshToken = authenticationService.getCookie('refreshToken');

  if(!token)
    return next(req);

  const newRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(newRequest).pipe(
    tap(() => {}),
    catchError((err: any) => {
      if (!err.error) {
        return throwError(() => 'Unknown error occurred.');
      }

      if (err.status === 401 && refreshToken) {
        return from(authenticationService.refreshTokenAsync(refreshToken)).pipe(
          mergeMap((response: any) => {
            const newToken = response.accessToken;
            const newRefreshToken = response.refreshToken;

            authenticationService.setCookie('token', newToken);
            authenticationService.setCookie('refreshToken', newRefreshToken);
            const refreshedRequest = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });

            return next(refreshedRequest);
          }),
          catchError((refreshError) => {
            console.error('Error handling expired access token:', refreshError);
            return throwError(() => refreshError);
          })
        );
      }

      if (err.status === 403) {
        router.navigate(['/access-denied']);
        return throwError(() => 'Access denied!');
      }

      return throwError(() => err);
    })
  );
};

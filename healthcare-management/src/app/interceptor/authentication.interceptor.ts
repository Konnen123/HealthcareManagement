import {HttpEvent, HttpInterceptorFn} from '@angular/common/http';
import {catchError, Observable, tap, throwError} from 'rxjs';
import {AuthenticationService} from '../services/authentication/authentication.service';
import {inject} from '@angular/core';
import {Router} from '@angular/router';

export const authenticationInterceptor: HttpInterceptorFn = (req, next): Observable<HttpEvent<unknown>> =>
{
  const authenticationService = inject(AuthenticationService);
  const router = inject(Router);

  const token = authenticationService.getCookie('token');

  if(!token)
    return next(req);

  const newRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(newRequest).pipe(tap(()=>{}),
    catchError((err: any) => {
      if(!err.error || !err.error.detail)
        return throwError(()=> 'Unknown error occurred.');

      if(err.status === 403)
      {
        router.navigate(['/access-denied']);
        return throwError(()=> 'Access denied!');
      }

      return throwError(()=> err);
    }));
};

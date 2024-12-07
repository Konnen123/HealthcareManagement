import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {JwtHelperService} from '@auth0/angular-jwt';
import {isPlatformBrowser} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService
{
  private readonly isBrowser: boolean
  constructor(private http: HttpClient,
              private router: Router,
              private jwtService: JwtHelperService,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  logout()
  {
    if(!this.isBrowser)
      return;

    document.cookie = 'token=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    this.router.navigate(['/login']);
  }

  isTokenValid(): boolean
  {
    if(!this.isBrowser)
      return false;

    return this.isAuthenticated() && this.getDecodedToken() && !this.isTokenExpired();
  }

  isAuthenticated(): boolean
  {
    if(!this.isBrowser)
      return false;

    const token = this.getCookie('token');
    return !!token;
  }

  isTokenExpired(): boolean
  {
    if(!this.isBrowser)
      return false;

    const token = this.getCookie('token');
    try{
      return this.jwtService.isTokenExpired(token);
    }
    catch (exception)
    {
      return true;
    }
  }

  private getDecodedToken()
  {
    if(!this.isBrowser)
      return null;

    const jwtToken = this.getCookie('token');
    try{
      return jwtToken ? this.jwtService.decodeToken(jwtToken) : undefined;
    }
    catch (exception)
    {
      return undefined;
    }
  }

  getCookie(name: string): string | null
  {
    if(!this.isBrowser)
      return null;

    const matches = document.cookie.match(new RegExp(
      `(?:^|; )${name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1')}=([^;]*)`
    ));
    return matches ? decodeURIComponent(matches[1]) : null;
  }
}

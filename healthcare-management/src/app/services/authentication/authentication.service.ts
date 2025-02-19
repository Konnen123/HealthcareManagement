import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {Router} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {isPlatformBrowser} from '@angular/common';
import {firstValueFrom} from 'rxjs';
import {AuthenticationClient} from '../../clients/authentication.client';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService
{
  private readonly isBrowser: boolean
  constructor(private readonly authenticationClient: AuthenticationClient,
              private readonly router: Router,
              private readonly jwtService: JwtHelperService,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }


  public async registerAsync(userData: any): Promise<any> {
    return await firstValueFrom(this.authenticationClient.register(userData));

  }

  public async loginAsync(userData: any): Promise<any> {
    return await firstValueFrom(this.authenticationClient.login(userData));
  }

  public async refreshTokenAsync(refreshToken: string): Promise<any> {
    return await firstValueFrom(this.authenticationClient.refreshToken(refreshToken));
  }

  logout()
  {
    if(!this.isBrowser)
      return;

    document.cookie = 'token=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = 'refreshToken=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    this.router.navigate(['/login']);
  }

  isTokenValid(): boolean
  {
    if(!this.isBrowser)
      return false;

    return this.isAuthenticated() && this.getDecodedToken() && !this.isTokenExpired() && !this.isRefreshTokenExpired();
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

  public isRefreshTokenExpired(): boolean
  {
    if (!this.isBrowser)
      return false;

    const refreshToken = this.getCookie('refreshToken');
    try {
      return refreshToken == null;
    } catch (exception) {
      return true;
    }
  }

  public getUserRole(): string
  {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] : "";
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
      `(?:^|; )${name.replace(/([.$?*|{}()\[\]\\/+^])/g, '\\$1')}=([^;]*)`
    ));
    return matches ? decodeURIComponent(matches[1]) : null;
  }

  setCookie(name: string, value: string): void {
    if (!this.isBrowser) return;

    document.cookie = `${name}=${value}; path=/`;
  }

  public async resetPasswordAsync(userData: any): Promise<any> {
    return await firstValueFrom(this.authenticationClient.resetPassword(userData));
  }

}

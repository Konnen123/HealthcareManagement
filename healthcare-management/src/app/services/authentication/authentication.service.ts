import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {JwtHelperService} from '@auth0/angular-jwt';
import {isPlatformBrowser} from '@angular/common';
import {firstValueFrom} from 'rxjs';
import {UserClient} from '../../clients/user.client';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService
{
  private readonly isBrowser: boolean
  constructor(private http: HttpClient,
              private readonly userClient: UserClient,
              private router: Router,
              private jwtService: JwtHelperService,
              @Inject(PLATFORM_ID) platformId: object)
  {
    this.isBrowser = isPlatformBrowser(platformId);
  }


  public async registerAsync(userData: any): Promise<any> {
    try {
      const result = await firstValueFrom(this.userClient.register(userData));
      console.log('Server response in the service :', result);
      return result;
    } catch (error){
      console.error('Error while registering in service', error);
      throw error;
    }

  }

  public async loginAsync(userData: any): Promise<any> {
    try {
      return await firstValueFrom(this.userClient.login(userData));
    } catch (error){
      console.error('Error while logging in service', error);
      throw error;
    }
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
      `(?:^|; )${name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1')}=([^;]*)`
    ));
    return matches ? decodeURIComponent(matches[1]) : null;
  }

  setCookie(name: string, value: string): void {
    if (!this.isBrowser) return;

    document.cookie = `${name}=${value}; path=/`;
  }
}

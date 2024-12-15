import {ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot
} from "@angular/router";
import {AuthenticationService} from '../../services/authentication/authentication.service';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate, CanActivateChild {
  constructor(private readonly authenticationService: AuthenticationService, private readonly router: Router) {
  }

  async canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean>
  {
    return this.checkAuthorization();
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean>
  {
    return this.checkAuthorization();
  }

  private async checkAuthorization(): Promise<boolean> {
    if(!this.authenticationService.isAuthenticated())
    {
      const isValid = await this.isRefreshTokenValid();
      if(isValid)
        return true;

      this.router.navigate(['/login']);
      return false;
    }
    if(!this.authenticationService.isTokenValid())
    {
      const isValid = await this.isRefreshTokenValid();
      if(isValid)
        return true;

      this.authenticationService.logout();
      return false;
    }
    return true;
  }

  private async isRefreshTokenValid(): Promise<boolean> {
    if(this.authenticationService.isRefreshTokenExpired())
      return false;

      const refreshToken = this.authenticationService.getCookie('refreshToken');
      if(!refreshToken)
      {
        return false;
      }

      try
      {
        const result = await this.authenticationService.refreshTokenAsync(refreshToken);
        this.authenticationService.setCookie('token', result.accessToken);
        this.authenticationService.setCookie('refreshToken', result.refreshToken);
        return true;

      }
      catch (error)
      {
        return false;
      }
  }


}

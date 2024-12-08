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

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate, CanActivateChild {
  constructor(private authenticationService: AuthenticationService, private router: Router) {
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult>
  {
    return this.checkAuthorization();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult>
  {
    return this.checkAuthorization();
  }

  private checkAuthorization(): boolean {
    if(!this.authenticationService.isAuthenticated())
    {
      this.router.navigate(['/login']);
      return false;
    }
    if(!this.authenticationService.isTokenValid())
    {
      this.authenticationService.logout();
      return false;
    }
    return true;
  }


}

import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import {Injectable} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate
{
  constructor(private readonly authenticationService: AuthenticationService, private readonly router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean
  {
    const expectedRole : string[] = route.data['expectedRole'];
    const userRole = this.authenticationService.getUserRole();
    console.log(userRole);
    if(!userRole)
      return false;

    if (!this.authenticationService.isAuthenticated()) {
      this.router.navigate(['/not-found']);
      return false;
    }
    if(!expectedRole.includes(userRole))
    {
      this.router.navigate(['/access-denied']);
      return false;
    }

    return true;
  }


}

import { TestBed } from '@angular/core/testing';
import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from '@angular/router';

import { AuthenticationGuard } from './authentication.guard';
import { AuthenticationService } from '../../services/authentication/authentication.service';

describe('AuthenticationGuard', () => {
  let authGuard: AuthenticationGuard;
  let authenticationService: jasmine.SpyObj<AuthenticationService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    authenticationService = jasmine.createSpyObj('AuthenticationService', [
      'isAuthenticated',
      'isTokenValid',
      'isRefreshTokenExpired',
      'getCookie',
      'refreshTokenAsync',
      'setCookie',
      'logout'
    ]);

    router = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthenticationGuard,
        { provide: AuthenticationService, useValue: authenticationService },
        { provide: Router, useValue: router },
      ],
    });

    authGuard = TestBed.inject(AuthenticationGuard);
  });

  it('should be created', () => {
    expect(authGuard).toBeTruthy();
  });

  it('should allow activation when the user is authenticated and token is valid', async () => {
    authenticationService.isAuthenticated.and.returnValue(true);
    authenticationService.isTokenValid.and.returnValue(true);

    const mockRoute = {} as ActivatedRouteSnapshot;
    const mockState = {} as RouterStateSnapshot;

    const result = await authGuard.canActivate(mockRoute, mockState);

    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should deny activation when refresh token renewal fails', async () => {
    authenticationService.isAuthenticated.and.returnValue(false);
    authenticationService.isRefreshTokenExpired.and.returnValue(false);
    authenticationService.getCookie.and.returnValue('invalid-refresh-token');
    authenticationService.refreshTokenAsync.and.returnValue(Promise.reject('Token renewal failed'));

    const mockRoute = {} as ActivatedRouteSnapshot;
    const mockState = {} as RouterStateSnapshot;

    const result = await authGuard.canActivate(mockRoute, mockState);

    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should allow activation when the user is authenticated but token is invalid and refresh succeeds', async () => {
    authenticationService.isAuthenticated.and.returnValue(true);
    authenticationService.isTokenValid.and.returnValue(false);
    authenticationService.isRefreshTokenExpired.and.returnValue(false);
    authenticationService.getCookie.and.returnValue('valid-refresh-token');
    authenticationService.refreshTokenAsync.and.returnValue(Promise.resolve({
      accessToken: 'new-access-token',
      refreshToken: 'new-refresh-token'
    }));

    const mockRoute = {} as ActivatedRouteSnapshot;
    const mockState = {} as RouterStateSnapshot;

    const result = await authGuard.canActivate(mockRoute, mockState);

    expect(result).toBeTrue();
    expect(authenticationService.setCookie).toHaveBeenCalledWith('token', 'new-access-token');
    expect(authenticationService.setCookie).toHaveBeenCalledWith('refreshToken', 'new-refresh-token');
    expect(router.navigate).not.toHaveBeenCalled();
  });
});

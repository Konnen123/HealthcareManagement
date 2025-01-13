import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router } from '@angular/router';

import { RoleGuard } from './role.guard';
import { AuthenticationService } from '../../services/authentication/authentication.service';

describe('RoleGuard', () => {
  let roleGuard: RoleGuard;
  let authenticationService: any;
  let router: any;

  beforeEach(() => {
    authenticationService = jasmine.createSpyObj('AuthenticationService', ['getUserRole', 'isAuthenticated']);
    router = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        RoleGuard,
        { provide: AuthenticationService, useValue: authenticationService },
        { provide: Router, useValue: router }
      ]
    });

    roleGuard = TestBed.inject(RoleGuard);
  });

  it('should be created', () => {
    expect(roleGuard).toBeTruthy();
  });


  it('should return false and navigate to "/access-denied" if user role is not in expectedRole', () => {
    authenticationService.isAuthenticated.and.returnValue(true);
    authenticationService.getUserRole.and.returnValue('user');
    const mockRoute: any = { data: { expectedRole: ['admin'] } };
    const mockState: any = {};

    const result = roleGuard.canActivate(mockRoute, mockState);

    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/access-denied']);
  });

  it('should return true if user role matches expectedRole', () => {
    authenticationService.isAuthenticated.and.returnValue(true);
    authenticationService.getUserRole.and.returnValue('admin');
    const mockRoute: any = { data: { expectedRole: ['admin'] } };
    const mockState: any = {};

    const result = roleGuard.canActivate(mockRoute, mockState);

    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should return true if expectedRole includes the userRole', () => {
    authenticationService.isAuthenticated.and.returnValue(true);
    authenticationService.getUserRole.and.returnValue('admin');
    const mockRoute: any = { data: { expectedRole: ['user', 'admin'] } };
    const mockState: any = {};

    const result = roleGuard.canActivate(mockRoute, mockState);

    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });
});


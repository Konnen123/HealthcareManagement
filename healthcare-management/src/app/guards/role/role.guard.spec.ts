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

  // Add more tests here
});

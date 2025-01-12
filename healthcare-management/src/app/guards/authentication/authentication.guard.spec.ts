import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router } from '@angular/router';

import { AuthenticationGuard } from './authentication.guard';
import { AuthenticationService } from '../../services/authentication/authentication.service';

describe('RoleGuard', () => {
  let authGuard: AuthenticationGuard;
  let authenticationService: any;
  let router: any;

  beforeEach(() => {
    authenticationService = jasmine.createSpyObj('AuthenticationService', ['getUserRole', 'isAuthenticated']);
    router = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthenticationGuard,
        { provide: AuthenticationService, useValue: authenticationService },
        { provide: Router, useValue: router }
      ]
    });

    authGuard = TestBed.inject(AuthenticationGuard);
  });

  it('should be created', () => {
    expect(authGuard).toBeTruthy();
  });

  // Add more tests here
});
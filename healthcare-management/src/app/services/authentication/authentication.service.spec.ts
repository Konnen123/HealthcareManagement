import { TestBed } from '@angular/core/testing';
import { AuthenticationService } from './authentication.service';
import { AuthenticationClient } from '../../clients/authentication.client';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PLATFORM_ID } from '@angular/core';
import { of } from 'rxjs';
import {K, T} from '@angular/cdk/keycodes';

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let mockAuthenticationClient: jasmine.SpyObj<AuthenticationClient>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockJwtHelperService: jasmine.SpyObj<JwtHelperService>;

  beforeEach(() => {
    mockAuthenticationClient = jasmine.createSpyObj('AuthenticationClient', [
      'register',
      'login',
      'refreshToken',
      'resetPassword',
    ]);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockJwtHelperService = jasmine.createSpyObj('JwtHelperService', [
      'isTokenExpired',
      'decodeToken',
    ]);

    TestBed.configureTestingModule({
      providers: [
        AuthenticationService,
        { provide: AuthenticationClient, useValue: mockAuthenticationClient },
        { provide: Router, useValue: mockRouter },
        { provide: JwtHelperService, useValue: mockJwtHelperService },
        { provide: PLATFORM_ID, useValue: 'browser' }, // Simulate browser environment
      ],
    });

    service = TestBed.inject(AuthenticationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call registerAsync with correct data', async () => {
    const mockUserData = { email: 'test@example.com', password: 'password' };
    const mockResponse = { success: true };
    mockAuthenticationClient.register.and.returnValue(of(mockResponse));

    const result = await service.registerAsync(mockUserData);

    expect(mockAuthenticationClient.register).toHaveBeenCalledWith(mockUserData);
    expect(result).toEqual(mockResponse);
  });

  it('should call loginAsync with correct data', async () => {
    const mockUserData = { email: 'test@example.com', password: 'password' };
    const mockResponse = { token: 'jwt-token' };
    mockAuthenticationClient.login.and.returnValue(of(mockResponse));

    const result = await service.loginAsync(mockUserData);

    expect(mockAuthenticationClient.login).toHaveBeenCalledWith(mockUserData);
    expect(result).toEqual(mockResponse);
  });

  it('should call refreshTokenAsync with correct refresh token', async () => {
    const mockRefreshToken = 'refresh-token';
    const mockResponse = { token: 'new-jwt-token' };
    mockAuthenticationClient.refreshToken.and.returnValue(of(mockResponse));

    const result = await service.refreshTokenAsync(mockRefreshToken);

    expect(mockAuthenticationClient.refreshToken).toHaveBeenCalledWith(mockRefreshToken);
    expect(result).toEqual(mockResponse);
  });

  it('should call resetPasswordAsync with correct data', async () => {
    const mockUserData = { email: 'test@example.com', newPassword: 'new-password' };
    const mockResponse = { success: true };
    mockAuthenticationClient.resetPassword.and.returnValue(of(mockResponse));

    const result = await service.resetPasswordAsync(mockUserData);

    expect(mockAuthenticationClient.resetPassword).toHaveBeenCalledWith(mockUserData);
    expect(result).toEqual(mockResponse);
  });


  it('should validate if the user is authenticated', () => {
    spyOn(service, 'getCookie').and.returnValue('mock-token');

    const isAuthenticated = service.isAuthenticated();

    expect(isAuthenticated).toBeTrue();
  });


  it('should detect if the refresh token is expired', () => {
    spyOn(service, 'getCookie').and.returnValue(null);

    const isRefreshTokenExpired = service.isRefreshTokenExpired();

    expect(isRefreshTokenExpired).toBeTrue();
  });

  it('should return the user role from the decoded token', () => {
    const mockDecodedToken = {
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': 'admin',
    };
    spyOn(service, 'getCookie').and.returnValue('mock-token');
    mockJwtHelperService.decodeToken.and.returnValue(mockDecodedToken);

    const userRole = service.getUserRole();

    expect(userRole).toBe('admin');
  });

  it('should return null for the user role if no token exists', () => {
    spyOn(service, 'getCookie').and.returnValue(null);

    const userRole = service.getUserRole();

    expect(userRole).toBe('');
  });
});

import { TestBed } from '@angular/core/testing';

import { RoleService } from './role.service';

describe('RoleService', () => {
  let service: RoleService;

  beforeEach(() => {
    const roleServiceSpy = jasmine.createSpyObj('RoleService', ['getRoles']);
    TestBed.configureTestingModule({
      providers: [
        { provide: RoleService, useValue: roleServiceSpy }
      ]
    });
    service = TestBed.inject(RoleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

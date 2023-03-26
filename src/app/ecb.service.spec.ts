import { TestBed } from '@angular/core/testing';

import { EcbService } from './ecb.service';

describe('EcbService', () => {
  let service: EcbService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EcbService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

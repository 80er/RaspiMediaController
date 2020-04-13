import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaumfeldComponent } from './raumfeld.component';

describe('RaumfeldComponent', () => {
  let component: RaumfeldComponent;
  let fixture: ComponentFixture<RaumfeldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaumfeldComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaumfeldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

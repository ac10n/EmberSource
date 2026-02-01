import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentExplorer } from './content-explorer';

describe('ContentExplorer', () => {
  let component: ContentExplorer;
  let fixture: ComponentFixture<ContentExplorer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContentExplorer]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContentExplorer);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

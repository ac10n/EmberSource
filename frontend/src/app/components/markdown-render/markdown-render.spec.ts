import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MarkdownRender } from './markdown-render';

describe('MarkdownRender', () => {
  let component: MarkdownRender;
  let fixture: ComponentFixture<MarkdownRender>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MarkdownRender]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MarkdownRender);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { MarkdownitPipe } from './markdownit-pipe';

describe('MarkdownitPipe', () => {
  it('create an instance', () => {
    const pipe = new MarkdownitPipe();
    expect(pipe).toBeTruthy();
  });
});

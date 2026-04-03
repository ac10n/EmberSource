import { Pipe, PipeTransform } from '@angular/core';
import markdownit from 'markdown-it';

@Pipe({
  name: 'markdownit',
})
export class MarkdownitPipe implements PipeTransform {
  md = new markdownit();

  transform(value: unknown, ...args: unknown[]): unknown {
    return this.md.render(value as string);
  }
}

import { Component, OnInit, signal } from '@angular/core';
import { ContentService } from '../../services/content-service';
import { KnowledgeResponseModel } from '../../models/contract-models';
import { CommonModule } from '@angular/common';
import { MarkdownitPipe } from '../../services/markdownit-pipe';

@Component({
  selector: 'ember-content-explorer',
  imports: [CommonModule, MarkdownitPipe],
  templateUrl: './content-explorer.html',
  styleUrl: './content-explorer.scss',
})
export class ContentExplorer implements OnInit {
  knowledgeItems = signal<KnowledgeResponseModel | null>(null);
  constructor(private readonly contentService: ContentService) {}

  ngOnInit(): void {
    this.contentService.getKnowledge({
      maxCount: 10,
    }).subscribe((response) => {
      this.knowledgeItems.set(response);
    });
  }
}

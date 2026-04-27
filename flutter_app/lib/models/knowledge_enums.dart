enum ContentType {
  paragraph,
  section,
  article,
  image,
  video,
  audio,
  link,
  interactiveElement,
  codeSnippet,
  explorableDataset,
  comment,
}

extension ContentTypeX on ContentType {
  int get id {
    switch (this) {
      case ContentType.paragraph:
        return 1;
      case ContentType.section:
        return 2;
      case ContentType.article:
        return 3;
      case ContentType.image:
        return 4;
      case ContentType.video:
        return 5;
      case ContentType.audio:
        return 6;
      case ContentType.link:
        return 7;
      case ContentType.interactiveElement:
        return 8;
      case ContentType.codeSnippet:
        return 9;
      case ContentType.explorableDataset:
        return 10;
      case ContentType.comment:
        return 11;
    }
  }

  static ContentType fromId(int id) {
    switch (id) {
      case 1:
        return ContentType.paragraph;
      case 2:
        return ContentType.section;
      case 3:
        return ContentType.article;
      case 4:
        return ContentType.image;
      case 5:
        return ContentType.video;
      case 6:
        return ContentType.audio;
      case 7:
        return ContentType.link;
      case 8:
        return ContentType.interactiveElement;
      case 9:
        return ContentType.codeSnippet;
      case 10:
        return ContentType.explorableDataset;
      case 11:
        return ContentType.comment;
      default:
        return ContentType.article;
    }
  }
}

enum ContentFormat {
  markdown,
  richText,
  plainText,
  html,
}

extension ContentFormatX on ContentFormat {
  int get id {
    switch (this) {
      case ContentFormat.markdown:
        return 1;
      case ContentFormat.richText:
        return 2;
      case ContentFormat.plainText:
        return 3;
      case ContentFormat.html:
        return 4;
    }
  }

  static ContentFormat fromId(int id) {
    switch (id) {
      case 1:
        return ContentFormat.markdown;
      case 2:
        return ContentFormat.richText;
      case 3:
        return ContentFormat.plainText;
      case 4:
        return ContentFormat.html;
      default:
        return ContentFormat.richText;
    }
  }
}

enum ContentVisibility {
  public,
  private,
  visibleToSpecificPeople,
  visibleByCriteria,
  scheduledRollout,
}

extension ContentVisibilityX on ContentVisibility {
  int get id {
    switch (this) {
      case ContentVisibility.public:
        return 1;
      case ContentVisibility.private:
        return 2;
      case ContentVisibility.visibleToSpecificPeople:
        return 3;
      case ContentVisibility.visibleByCriteria:
        return 4;
      case ContentVisibility.scheduledRollout:
        return 5;
    }
  }

  static ContentVisibility fromId(int id) {
    switch (id) {
      case 1:
        return ContentVisibility.public;
      case 2:
        return ContentVisibility.private;
      case 3:
        return ContentVisibility.visibleToSpecificPeople;
      case 4:
        return ContentVisibility.visibleByCriteria;
      case 5:
        return ContentVisibility.scheduledRollout;
      default:
        return ContentVisibility.private;
    }
  }
}

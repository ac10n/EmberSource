enum ContentBlockType {
  heading,
  paragraph,
  image,
}

class ContentBlock {
  final String localId;
  ContentBlockType type;
  String text;
  String? imageUrl;
  final List<String> tagIds;
  final List<String> collectionIds;

  ContentBlock({
    required this.localId,
    required this.type,
    required this.text,
    this.imageUrl,
    List<String>? tagIds,
    List<String>? collectionIds,
  })  : tagIds = tagIds ?? <String>[],
        collectionIds = collectionIds ?? <String>[];

  Map<String, dynamic> toJson() {
    return {
      'type': type.name,
      'text': text,
      if (imageUrl != null && imageUrl!.isNotEmpty) 'imageUrl': imageUrl,
      if (tagIds.isNotEmpty) 'tagIds': tagIds,
      if (collectionIds.isNotEmpty) 'collectionIds': collectionIds,
    };
  }

  static ContentBlock fromJson(Map<String, dynamic> json) {
    final typeName = json['type']?.toString() ?? 'paragraph';
    final blockType = ContentBlockType.values.firstWhere(
      (value) => value.name == typeName,
      orElse: () => ContentBlockType.paragraph,
    );

    return ContentBlock(
      localId: DateTime.now().microsecondsSinceEpoch.toString(),
      type: blockType,
      text: json['text']?.toString() ?? '',
      imageUrl: json['imageUrl']?.toString(),
      tagIds: (json['tagIds'] as List<dynamic>?)
              ?.map((id) => id.toString())
              .toList() ??
          <String>[],
      collectionIds: (json['collectionIds'] as List<dynamic>?)
              ?.map((id) => id.toString())
              .toList() ??
          <String>[],
    );
  }
}

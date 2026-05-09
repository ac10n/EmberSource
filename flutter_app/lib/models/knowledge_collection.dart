import 'knowledge_content.dart';

class KnowledgeCollectionItem {
  final String id;
  final String? contentId;
  final String collectionId;
  final int orderIndex;
  final DateTime addedAt;
  final KnowledgeContent? content;

  KnowledgeCollectionItem({
    required this.id,
    required this.contentId,
    required this.collectionId,
    required this.orderIndex,
    required this.addedAt,
    required this.content,
  });

  factory KnowledgeCollectionItem.fromJson(Map<String, dynamic> json) {
    return KnowledgeCollectionItem(
      id: json['id']?.toString() ?? '',
      contentId: json['contentId']?.toString(),
      collectionId: json['collectionId']?.toString() ?? '',
      orderIndex: (json['orderIndex'] as num?)?.toInt() ?? 0,
      addedAt: DateTime.tryParse(json['addedAt']?.toString() ?? '') ??
          DateTime.fromMillisecondsSinceEpoch(0),
      content: json['content'] is Map<String, dynamic>
          ? KnowledgeContent.fromJson(json['content'] as Map<String, dynamic>)
          : null,
    );
  }
}

class KnowledgeCollection {
  final String id;
  final String name;
  final String? description;
  final String emberUserId;
  final DateTime createdAt;
  final List<KnowledgeCollectionItem> items;

  KnowledgeCollection({
    required this.id,
    required this.name,
    required this.description,
    required this.emberUserId,
    required this.createdAt,
    required this.items,
  });

  factory KnowledgeCollection.fromJson(Map<String, dynamic> json) {
    final itemsJson = json['items'] as List<dynamic>? ?? [];
    return KnowledgeCollection(
      id: json['id']?.toString() ?? '',
      name: json['name']?.toString() ?? '',
      description: json['description']?.toString(),
      emberUserId: json['emberUserId']?.toString() ?? '',
      createdAt: DateTime.tryParse(json['createdAt']?.toString() ?? '') ??
          DateTime.fromMillisecondsSinceEpoch(0),
      items: itemsJson
          .whereType<Map<String, dynamic>>()
          .map(KnowledgeCollectionItem.fromJson)
          .toList(),
    );
  }

  Map<String, dynamic> toCreateJson() {
    return {
      'name': name,
      if (description != null) 'description': description,
    };
  }

  Map<String, dynamic> toUpdateJson() {
    return {
      'name': name,
      if (description != null) 'description': description,
    };
  }
}

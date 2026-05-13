import 'knowledge_enums.dart';

class KnowledgeContent {
  final String id;
  final String identifier;
  final String? parentContentId;
  final ContentType contentType;
  final ContentFormat contentFormat;
  final ContentVisibility contentVisibility;
  final String? visibilityCriteria;
  final String? title;
  final String? data;
  final String emberUserId;
  final DateTime createdAt;

  KnowledgeContent({
    required this.id,
    required this.identifier,
    required this.parentContentId,
    required this.contentType,
    required this.contentFormat,
    required this.contentVisibility,
    required this.visibilityCriteria,
    required this.title,
    required this.data,
    required this.emberUserId,
    required this.createdAt,
  });

  factory KnowledgeContent.fromJson(Map<String, dynamic> json) {
    return KnowledgeContent(
      id: json['id']?.toString() ?? '',
      identifier:
          json['identifier']?.toString() ?? json['id']?.toString() ?? '',
      parentContentId: json['parentContentId']?.toString(),
      contentType: ContentTypeX.fromJson(json['contentTypeId']),
      contentFormat: ContentFormatX.fromJson(json['contentFormatId']),
      contentVisibility:
          ContentVisibilityX.fromJson(json['contentVisibilityId']),
      visibilityCriteria: json['visibilityCriteria']?.toString(),
      title: json['title']?.toString(),
      data: json['data']?.toString(),
      emberUserId: json['emberUserId']?.toString() ?? '',
      createdAt: DateTime.tryParse(json['createdAt']?.toString() ?? '') ??
          DateTime.fromMillisecondsSinceEpoch(0),
    );
  }
}

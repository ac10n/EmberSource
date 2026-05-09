import 'dart:convert';
import 'package:flutter/foundation.dart';
import '../config/api_config.dart';
import '../models/knowledge_collection.dart';
import '../models/knowledge_content.dart';
import '../models/knowledge_enums.dart';
import '../models/knowledge_tag.dart';
import 'api_service.dart';

class KnowledgeService {
  final ApiService _apiService;

  KnowledgeService(this._apiService);

  String _action(String action) => '${ApiConfig.knowledgeBase}/$action';

  Future<List<KnowledgeContent>> getContents({String? titleSearchTerm}) async {
    try {
      final payload = <String, dynamic>{};
      if (titleSearchTerm != null && titleSearchTerm.trim().isNotEmpty) {
        payload['titleSearchTerm'] = titleSearchTerm.trim();
      }

      final response = await _apiService.post(
        _action('GetKnowledgeItems'),
        data: payload,
      );

      final data = response.data is Map<String, dynamic>
          ? response.data as Map<String, dynamic>
          : <String, dynamic>{};
      final contents = data['contents'] as List<dynamic>? ?? <dynamic>[];
      return contents
          .whereType<Map<String, dynamic>>()
          .map(KnowledgeContent.fromJson)
          .toList();
    } catch (e) {
      debugPrint('Error fetching knowledge items: $e');
      throw Exception('Failed to load knowledge items: $e');
    }
  }

  Future<KnowledgeContent> createContent({
    String? parentContentId,
    required ContentType contentType,
    required ContentFormat contentFormat,
    required int formatVersion,
    String? title,
    required String data,
    required ContentVisibility contentVisibility,
    String? visibilityCriteria,
    List<KnowledgeTag>? tags,
    List<String>? collectionIds,
  }) async {
    final payload = <String, dynamic>{
      'parentContentId': parentContentId,
      'contentTypeId': contentType.id,
      'contentFormatId': contentFormat.id,
      'formatVersion': formatVersion,
      'title': title,
      'data': data,
      'contentVisibilityId': contentVisibility.id,
      if (visibilityCriteria != null) 'visibilityCriteria': visibilityCriteria,
      if (tags != null && tags.isNotEmpty)
        'tags': tags.map((t) => t.toContentTagJson()).toList(),
      if (collectionIds != null && collectionIds.isNotEmpty)
        'collectionIds': collectionIds,
    };

    final response = await _apiService.post(
      _action('AddContent'),
      data: payload,
    );

    return KnowledgeContent.fromJson(
      response.data as Map<String, dynamic>,
    );
  }

  Future<KnowledgeContent> updateContent({
    required String contentId,
    String? parentContentId,
    required ContentType contentType,
    required ContentFormat contentFormat,
    required int formatVersion,
    String? title,
    required String data,
    required ContentVisibility contentVisibility,
    String? visibilityCriteria,
    List<KnowledgeTag>? tags,
    List<String>? collectionIds,
  }) async {
    final payload = <String, dynamic>{
      'parentContentId': parentContentId,
      'contentTypeId': contentType.id,
      'contentFormatId': contentFormat.id,
      'formatVersion': formatVersion,
      'title': title,
      'data': data,
      'contentVisibilityId': contentVisibility.id,
      if (visibilityCriteria != null) 'visibilityCriteria': visibilityCriteria,
      if (tags != null) 'tags': tags.map((t) => t.toContentTagJson()).toList(),
      if (collectionIds != null) 'collectionIds': collectionIds,
    };

    final response = await _apiService.put(
      _action('UpdateContent/$contentId'),
      data: payload,
    );

    return KnowledgeContent.fromJson(
      response.data as Map<String, dynamic>,
    );
  }

  Future<void> deactivateContent(String contentId) async {
    await _apiService.delete(_action('DeactivateContent/$contentId'));
  }

  Future<List<KnowledgeTag>> getTags() async {
    final response = await _apiService.get(_action('GetTags'));
    final tags = response.data as List<dynamic>? ?? <dynamic>[];
    return tags
        .whereType<Map<String, dynamic>>()
        .map(KnowledgeTag.fromJson)
        .toList();
  }

  Future<KnowledgeTag> createTag(KnowledgeTag tag) async {
    final response = await _apiService.post(
      _action('CreateTag'),
      data: tag.toCreateJson(),
    );
    return KnowledgeTag.fromJson(response.data as Map<String, dynamic>);
  }

  Future<KnowledgeTag> updateTag(String tagId, KnowledgeTag tag) async {
    final response = await _apiService.put(
      _action('UpdateTag/$tagId'),
      data: tag.toUpdateJson(),
    );
    return KnowledgeTag.fromJson(response.data as Map<String, dynamic>);
  }

  Future<void> deleteTag(String tagId) async {
    await _apiService.delete(_action('DeleteTag/$tagId'));
  }

  Future<List<KnowledgeCollection>> getCollections() async {
    final response = await _apiService.get(_action('GetCollections'));
    final collections = response.data as List<dynamic>? ?? <dynamic>[];
    return collections
        .whereType<Map<String, dynamic>>()
        .map(KnowledgeCollection.fromJson)
        .toList();
  }

  Future<KnowledgeCollection> createCollection(
      KnowledgeCollection collection) async {
    final response = await _apiService.post(
      _action('CreateCollection'),
      data: collection.toCreateJson(),
    );
    return KnowledgeCollection.fromJson(response.data as Map<String, dynamic>);
  }

  Future<KnowledgeCollection> updateCollection(
    String collectionId,
    KnowledgeCollection collection,
  ) async {
    final response = await _apiService.put(
      _action('UpdateCollection/$collectionId'),
      data: collection.toUpdateJson(),
    );
    return KnowledgeCollection.fromJson(response.data as Map<String, dynamic>);
  }

  Future<void> deleteCollection(String collectionId) async {
    await _apiService.delete(_action('DeleteCollection/$collectionId'));
  }

  Future<void> addContentToCollection(
    String collectionId,
    String contentId,
    int orderIndex,
  ) async {
    final payload = {
      'contentId': contentId,
      'orderIndex': orderIndex,
    };
    await _apiService.post(
      _action('AddContentToCollection/$collectionId'),
      data: payload,
    );
  }

  Future<void> removeContentFromCollection(
    String collectionId,
    String contentId,
  ) async {
    await _apiService.delete(
      _action('RemoveContentFromCollection/$collectionId/items/$contentId'),
    );
  }

  String encodeBlocks(List<Map<String, dynamic>> blocks) {
    return jsonEncode({'blocks': blocks});
  }

  List<Map<String, dynamic>> decodeBlocks(String? data) {
    if (data == null || data.trim().isEmpty) {
      return <Map<String, dynamic>>[];
    }
    try {
      final decoded = jsonDecode(data) as Map<String, dynamic>;
      final blocks = decoded['blocks'] as List<dynamic>? ?? <dynamic>[];
      return blocks.whereType<Map<String, dynamic>>().toList();
    } catch (_) {
      return <Map<String, dynamic>>[];
    }
  }
}

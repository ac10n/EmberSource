import 'package:flutter/foundation.dart';
import '../config/api_config.dart';
import '../models/invitation.dart';
import 'api_service.dart';

class InvitationService {
  final ApiService _apiService;

  InvitationService(this._apiService);

  String _action(String action) => '${ApiConfig.invitationBase}/$action';

  Future<List<Invitation>> getMyInvitations() async {
    try {
      final response = await _apiService.get(_action('GetMyInvitations'));
      final list = response.data as List<dynamic>? ?? [];
      return list
          .whereType<Map<String, dynamic>>()
          .map(Invitation.fromJson)
          .toList();
    } catch (e) {
      debugPrint('Error fetching invitations: $e');
      rethrow;
    }
  }

  Future<Invitation> createInvitation({
    required String realName,
    required bool isInLegalAge,
    required String jurisdiction,
    String? email,
    String? phone,
    DateTime? expiresAt,
  }) async {
    final payload = <String, dynamic>{
      'realName': realName,
      'isInLegalAge': isInLegalAge,
      'jurisdiction': jurisdiction,
      if (email != null && email.isNotEmpty) 'email': email,
      if (phone != null && phone.isNotEmpty) 'phone': phone,
      if (expiresAt != null) 'expiresAt': expiresAt.toUtc().toIso8601String(),
    };

    try {
      final response = await _apiService.post(
        _action('CreateInvitation'),
        data: payload,
      );
      return Invitation.fromJson(response.data as Map<String, dynamic>);
    } catch (e) {
      debugPrint('Error creating invitation: $e');
      rethrow;
    }
  }
}

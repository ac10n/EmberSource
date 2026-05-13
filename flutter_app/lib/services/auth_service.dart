import 'dart:io';

import 'package:dio/dio.dart';

import '../config/api_config.dart';
import '../models/profile.dart';
import '../utils/jwt_utils.dart';
import 'api_service.dart';
import 'auth_token_store.dart';

class AuthService {
  final ApiService _apiService;
  final AuthTokenStore _tokenStore;

  AuthService(this._apiService, this._tokenStore);

  Future<void> login(
      {required String userName,
      required String password,
      bool rememberMe = true}) async {
    try {
      final response = await _apiService.post(
        ApiConfig.login,
        data: {
          'userName': userName,
          'password': password,
          'rememberMe': rememberMe,
        },
      );

      final accessToken = response.data['accessToken'] as String? ??
          response.data['token'] as String?;
      final refreshToken = response.data['refreshToken'] as String?;

      if (accessToken == null || accessToken.isEmpty) {
        throw Exception('Login failed: missing access token');
      }

      await _tokenStore.saveTokens(accessToken, refreshToken);
    } on DioException catch (error) {
      if (error.type == DioExceptionType.connectionTimeout ||
          error.type == DioExceptionType.connectionError ||
          error.type == DioExceptionType.sendTimeout ||
          error.type == DioExceptionType.receiveTimeout ||
          error.error is SocketException) {
        throw Exception('Unable to connect to the server. Please try again.');
      }

      rethrow;
    }
  }

  Future<void> register({
    required String inviteCode,
    required String username,
    required String email,
    required String password,
    String? profileImageUrl,
  }) async {
    await _apiService.post(
      ApiConfig.register,
      data: {
        'inviteCode': inviteCode,
        'username': username,
        'email': email,
        'password': password,
        if (profileImageUrl != null && profileImageUrl.isNotEmpty)
          'profileImageUrl': profileImageUrl,
      },
    );
  }

  Future<ProfileInfo> getProfile() async {
    final response = await _apiService.post(
      ApiConfig.profileGet,
      data: const {},
    );
    return ProfileInfo.fromJson(
        Map<String, dynamic>.from(response.data as Map));
  }

  Future<void> updateProfile({
    required String fullName,
    required int birthYear,
    required String jurisdiction,
  }) async {
    await _apiService.post(
      ApiConfig.profileUpdate,
      data: {
        'fullName': fullName,
        'birthYear': birthYear,
        'jurisdiction': jurisdiction,
      },
    );
  }

  Future<void> changePassword({
    required String oldPassword,
    required String newPassword,
  }) async {
    await _apiService.post(
      ApiConfig.profileChangePassword,
      data: {
        'oldPassword': oldPassword,
        'newPassword': newPassword,
      },
    );
  }

  Future<bool> hasValidToken() async {
    final accessToken = await _tokenStore.readAccessToken();
    if (accessToken == null || accessToken.isEmpty) {
      return false;
    }

    if (!JwtUtils.isExpired(accessToken)) {
      return true;
    }

    final refreshed = await _apiService.refreshTokens();
    if (refreshed) {
      return true;
    }

    await _tokenStore.clearToken();
    return false;
  }

  Future<void> logout() async {
    await _tokenStore.clearToken();
  }
}

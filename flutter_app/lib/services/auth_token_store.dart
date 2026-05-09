import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthTokenStore {
  static const _accessTokenKey = 'auth_access_token';
  static const _refreshTokenKey = 'auth_refresh_token';
  static const _storage = FlutterSecureStorage();

  Future<void> saveToken(String token) {
    return _storage.write(key: _accessTokenKey, value: token);
  }

  Future<void> saveTokens(String accessToken, String? refreshToken) async {
    await _storage.write(key: _accessTokenKey, value: accessToken);
    if (refreshToken != null) {
      await _storage.write(key: _refreshTokenKey, value: refreshToken);
    }
  }

  Future<String?> readToken() {
    return _storage.read(key: _accessTokenKey);
  }

  Future<String?> readAccessToken() {
    return _storage.read(key: _accessTokenKey);
  }

  Future<String?> readRefreshToken() {
    return _storage.read(key: _refreshTokenKey);
  }

  Future<void> clearToken() async {
    await _storage.delete(key: _accessTokenKey);
    await _storage.delete(key: _refreshTokenKey);
  }
}

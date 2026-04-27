import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthTokenStore {
  static const _tokenKey = 'auth_token';
  static const _storage = FlutterSecureStorage();

  Future<void> saveToken(String token) {
    return _storage.write(key: _tokenKey, value: token);
  }

  Future<String?> readToken() {
    return _storage.read(key: _tokenKey);
  }

  Future<void> clearToken() {
    return _storage.delete(key: _tokenKey);
  }
}

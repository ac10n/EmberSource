import '../config/api_config.dart';
import '../utils/jwt_utils.dart';
import 'api_service.dart';
import 'auth_token_store.dart';

class AuthService {
  final ApiService _apiService;
  final AuthTokenStore _tokenStore;

  AuthService(this._apiService, this._tokenStore);

  Future<void> login({required String email, required String password}) async {
    final response = await _apiService.post(
      ApiConfig.login,
      data: {
        'email': email,
        'password': password,
      },
    );

    final token = response.data['token'] as String?;
    if (token == null || token.isEmpty) {
      throw Exception('Login failed: missing token');
    }

    await _tokenStore.saveToken(token);
  }

  Future<void> register({
    required String username,
    required String email,
    required String password,
    String? profileImageUrl,
  }) async {
    await _apiService.post(
      ApiConfig.register,
      data: {
        'username': username,
        'email': email,
        'password': password,
        if (profileImageUrl != null && profileImageUrl.isNotEmpty)
          'profileImageUrl': profileImageUrl,
      },
    );
  }

  Future<bool> hasValidToken() async {
    final token = await _tokenStore.readToken();
    if (token == null || token.isEmpty) {
      return false;
    }

    final expired = JwtUtils.isExpired(token);
    if (expired) {
      await _tokenStore.clearToken();
      return false;
    }

    return true;
  }

  Future<void> logout() async {
    await _tokenStore.clearToken();
  }
}

import 'dart:convert';

class JwtUtils {
  static bool isExpired(String token) {
    final parts = token.split('.');
    if (parts.length != 3) {
      return true;
    }

    final payload = _decodeBase64Url(parts[1]);
    if (payload == null) {
      return true;
    }

    final Map<String, dynamic> data = jsonDecode(payload);
    final exp = data['exp'];
    if (exp is! int) {
      return true;
    }

    final nowSeconds = DateTime.now().millisecondsSinceEpoch ~/ 1000;
    return nowSeconds >= exp;
  }

  /// Returns true if the token's payload contains the given claim with value 'true'.
  static bool hasClaim(String token, String claimType) {
    final parts = token.split('.');
    if (parts.length != 3) return false;
    final payload = _decodeBase64Url(parts[1]);
    if (payload == null) return false;
    final Map<String, dynamic> data = jsonDecode(payload);
    final value = data[claimType];
    if (value == null) return false;
    if (value is bool) return value;
    return value.toString().toLowerCase() == 'true';
  }

  static String? _decodeBase64Url(String input) {
    try {
      final normalized = base64Url.normalize(input);
      final bytes = base64Url.decode(normalized);
      return utf8.decode(bytes);
    } catch (_) {
      return null;
    }
  }
}

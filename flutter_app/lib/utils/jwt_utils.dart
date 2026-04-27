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

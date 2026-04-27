import 'dart:io';
import 'package:flutter/foundation.dart';

class ApiConfig {
  // Base URL for API - platform specific
  static String get baseUrl {
    String url;

    // For web, use localhost
    if (kIsWeb) {
      url = 'http://localhost:8080/api/v01';
    }
    // For Android emulator, use 10.0.2.2 to access host machine
    else if (Platform.isAndroid) {
      url = 'http://10.0.2.2:8080/api/v01';
    }
    // For iOS simulator, localhost works
    else if (Platform.isIOS) {
      url = 'http://localhost:8080/api/v01';
    }
    // Default fallback
    else {
      url = 'http://localhost:8080/api/v01';
    }

    debugPrint('🌐 API Base URL: $url');
    return url;
  }

  // Timeout duration
  static const Duration connectTimeout = Duration(seconds: 30);
  static const Duration receiveTimeout = Duration(seconds: 30);

  // API Endpoints
  static const String login = '/auth/login';
  static const String register = '/auth/register';
  static const String users = '/users';
  static const String deviceTokens = '/notifications/register-token';

  // Knowledge endpoints
  static const String knowledgeBase = '/Knowledge';

  // Invitation endpoints
  static const String invitationBase = '/Invitation';
}

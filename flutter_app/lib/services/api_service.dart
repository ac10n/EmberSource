import 'dart:io';
import 'package:dio/dio.dart';
import 'package:dio/io.dart';
import 'package:flutter/foundation.dart';
import '../config/api_config.dart';
import 'auth_token_store.dart';

class ApiService {
  late final Dio _dio;
  final AuthTokenStore _tokenStore = AuthTokenStore();

  // Expose Dio instance for direct use when needed (e.g., full URLs)
  Dio get dio => _dio;

  ApiService() {
    final baseUrl = ApiConfig.baseUrl;
    debugPrint('🚀 Initializing ApiService');
    debugPrint('📍 Base URL: $baseUrl');

    _dio = Dio(
      BaseOptions(
        baseUrl: baseUrl,
        connectTimeout: ApiConfig.connectTimeout,
        receiveTimeout: ApiConfig.receiveTimeout,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
      ),
    );

    // Bypass SSL certificate verification for development (HTTP only)
    if (!kIsWeb) {
      (_dio.httpClientAdapter as IOHttpClientAdapter).createHttpClient = () {
        final client = HttpClient();
        client.badCertificateCallback =
            (X509Certificate cert, String host, int port) => true;
        return client;
      };
    }

    // Add interceptors
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) async {
          final fullUrl = '${options.baseUrl}${options.path}';
          debugPrint('🔄 Making ${options.method} request to: $fullUrl');
          // Add auth token if available
          final token = await _tokenStore.readToken();
          if (token != null && token.isNotEmpty) {
            options.headers['Authorization'] = 'Bearer $token';
          }
          return handler.next(options);
        },
        onError: (error, handler) {
          debugPrint('Request error: ${error.type} - ${error.message}');
          debugPrint('Error details: ${error.error}');
          // Handle errors globally
          _handleError(error);
          return handler.next(error);
        },
      ),
    );
  }

  // GET request
  Future<Response> get(String endpoint,
      {Map<String, dynamic>? queryParameters}) async {
    try {
      final response =
          await _dio.get(endpoint, queryParameters: queryParameters);
      return response;
    } catch (e) {
      rethrow;
    }
  }

  // POST request
  Future<Response> post(String endpoint, {dynamic data}) async {
    try {
      final response = await _dio.post(endpoint, data: data);
      return response;
    } catch (e) {
      rethrow;
    }
  }

  // PUT request
  Future<Response> put(String endpoint, {dynamic data}) async {
    try {
      final response = await _dio.put(endpoint, data: data);
      return response;
    } catch (e) {
      rethrow;
    }
  }

  // DELETE request
  Future<Response> delete(String endpoint) async {
    try {
      final response = await _dio.delete(endpoint);
      return response;
    } catch (e) {
      rethrow;
    }
  }

  void _handleError(DioException error) {
    debugPrint('Handling error type: ${error.type}');
    debugPrint('Error message: ${error.message}');
    debugPrint('Error: ${error.error}');

    switch (error.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.sendTimeout:
      case DioExceptionType.receiveTimeout:
        throw Exception('Connection timeout');
      case DioExceptionType.badResponse:
        throw Exception('Server error: ${error.response?.statusCode}');
      case DioExceptionType.cancel:
        throw Exception('Request cancelled');
      default:
        throw Exception('Network error: ${error.message ?? error.error}');
    }
  }
}

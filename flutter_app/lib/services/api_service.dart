import 'dart:async';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:dio/io.dart';
import 'package:flutter/foundation.dart';

import '../config/api_config.dart';
import 'auth_token_store.dart';

class ApiService {
  late final Dio _dio;
  final AuthTokenStore _tokenStore = AuthTokenStore();
  Future<String?>? _refreshTokenInFlight;

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

    if (!kIsWeb) {
      (_dio.httpClientAdapter as IOHttpClientAdapter).createHttpClient = () {
        final client = HttpClient();
        client.badCertificateCallback =
            (X509Certificate cert, String host, int port) => true;
        return client;
      };
    }

    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) async {
          final fullUrl = '${options.baseUrl}${options.path}';
          debugPrint('🔄 Making ${options.method} request to: $fullUrl');

          if (options.extra['skipAuth'] != true) {
            final token = await _tokenStore.readAccessToken();
            if (token != null && token.isNotEmpty) {
              options.headers['Authorization'] = 'Bearer $token';
            }
          }

          return handler.next(options);
        },
        onError: (error, handler) async {
          final shouldSkipRefresh =
              error.requestOptions.extra['skipRefresh'] == true;
          final alreadyRetried =
              error.requestOptions.extra['retriedAfterRefresh'] == true;

          if (error.response?.statusCode == 401 &&
              !shouldSkipRefresh &&
              !alreadyRetried) {
            final refreshedToken = await _refreshAccessToken();
            if (refreshedToken != null && refreshedToken.isNotEmpty) {
              final requestOptions = error.requestOptions;
              requestOptions.extra['retriedAfterRefresh'] = true;
              requestOptions.headers['Authorization'] =
                  'Bearer $refreshedToken';

              try {
                final response = await _dio.fetch(requestOptions);
                return handler.resolve(response);
              } on DioException catch (retryError) {
                if (retryError.response?.statusCode == 401) {
                  await _tokenStore.clearToken();
                }

                return handler.next(retryError);
              }
            }

            return handler.next(error);
          }

          if (error.response?.statusCode == 401 && !shouldSkipRefresh) {
            await _tokenStore.clearToken();
            return handler.next(error);
          }

          debugPrint('Request error: ${error.type} - ${error.message}');
          debugPrint(
            'Request: ${error.requestOptions.method} ${error.requestOptions.uri}',
          );
          debugPrint('Status: ${error.response?.statusCode}');
          debugPrint('Response: ${error.response?.data}');
          debugPrint('Error details: ${error.error}');
          return handler.next(error);
        },
      ),
    );
  }

  Future<bool> refreshTokens() async {
    final token = await _refreshAccessToken();
    return token != null && token.isNotEmpty;
  }

  Future<String?> _refreshAccessToken() {
    _refreshTokenInFlight ??= _performTokenRefresh().whenComplete(() {
      _refreshTokenInFlight = null;
    });

    return _refreshTokenInFlight!;
  }

  Future<String?> _performTokenRefresh() async {
    final refreshToken = await _tokenStore.readRefreshToken();
    if (refreshToken == null || refreshToken.isEmpty) {
      return null;
    }

    try {
      final response = await _dio.post(
        ApiConfig.refresh,
        data: {'refreshToken': refreshToken},
        options: Options(
          extra: {
            'skipAuth': true,
            'skipRefresh': true,
          },
        ),
      );

      final responseData = Map<String, dynamic>.from(response.data as Map);
      final accessToken = responseData['accessToken'] as String? ??
          responseData['token'] as String?;
      final newRefreshToken =
          responseData['refreshToken'] as String? ?? refreshToken;

      if (accessToken == null || accessToken.isEmpty) {
        return null;
      }

      await _tokenStore.saveTokens(accessToken, newRefreshToken);
      return accessToken;
    } on DioException catch (error) {
      if (error.response?.statusCode == 401 ||
          error.response?.statusCode == 403) {
        await _tokenStore.clearToken();
      }

      return null;
    }
  }

  Future<Response> get(String endpoint,
      {Map<String, dynamic>? queryParameters}) async {
    try {
      return await _dio.get(endpoint, queryParameters: queryParameters);
    } on DioException catch (error) {
      throw _mapDioException(error);
    }
  }

  Future<Response> post(String endpoint, {dynamic data}) async {
    try {
      return await _dio.post(endpoint, data: data);
    } on DioException catch (error) {
      throw _mapDioException(error);
    }
  }

  Future<Response> put(String endpoint, {dynamic data}) async {
    try {
      return await _dio.put(endpoint, data: data);
    } on DioException catch (error) {
      throw _mapDioException(error);
    }
  }

  Future<Response> delete(String endpoint) async {
    try {
      return await _dio.delete(endpoint);
    } on DioException catch (error) {
      throw _mapDioException(error);
    }
  }

  Exception _mapDioException(DioException error) {
    debugPrint('Handling error type: ${error.type}');
    debugPrint('Error message: ${error.message}');
    debugPrint('Error: ${error.error}');

    switch (error.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.sendTimeout:
      case DioExceptionType.receiveTimeout:
        return Exception('Connection timeout');
      case DioExceptionType.connectionError:
        return Exception('Unable to connect to the server. Please try again.');
      case DioExceptionType.badResponse:
        if (error.response?.statusCode == 401) {
          return Exception('Unauthorized');
        }
        return Exception('Server error: ${error.response?.statusCode}');
      case DioExceptionType.cancel:
        return Exception('Request cancelled');
      default:
        return Exception('Network error: ${error.message ?? error.error}');
    }
  }
}

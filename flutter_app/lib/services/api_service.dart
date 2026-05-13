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

          debugPrint('Interceptor onError fired');
          debugPrint(
              'Request: ${error.requestOptions.method} ${error.requestOptions.uri}');
          debugPrint('Extras: ${error.requestOptions.extra}');
          debugPrint('Status code: ${error.response?.statusCode}');
          debugPrint('Error type: ${error.type}');

          // Attempt refresh on common auth failure codes (401) and optionally 403
          final status = error.response?.statusCode;
          if ((status == 401 || status == 403) &&
              !shouldSkipRefresh &&
              !alreadyRetried) {
            debugPrint('Attempting to refresh access token (interceptor)');
            final refreshedToken = await _refreshAccessToken();
            debugPrint(
                'Refreshed token: ${refreshedToken != null ? 'present' : 'null'}');
            if (refreshedToken != null && refreshedToken.isNotEmpty) {
              final requestOptions = error.requestOptions;
              requestOptions.extra['retriedAfterRefresh'] = true;
              requestOptions.headers['Authorization'] =
                  'Bearer $refreshedToken';

              try {
                final response = await _dio.fetch(requestOptions);
                debugPrint('Retry after refresh succeeded');
                return handler.resolve(response);
              } on DioException catch (retryError) {
                debugPrint(
                    'Retry after refresh failed, status: ${retryError.response?.statusCode}');
                if (retryError.response?.statusCode == 401 ||
                    retryError.response?.statusCode == 403) {
                  await _tokenStore.clearToken();
                }

                return handler.next(retryError);
              }
            }

            debugPrint('Refresh did not yield a token or failed');
            return handler.next(error);
          }

          if (status == 401 && !shouldSkipRefresh) {
            debugPrint(
                '401 received and not skipping refresh -> clearing tokens');
            await _tokenStore.clearToken();
            return handler.next(error);
          }

          debugPrint('Request error: ${error.type} - ${error.message}');
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
    // Try several common payload formats to be tolerant of backend expectations
    final payloadCandidates = [
      {'refreshToken': refreshToken},
      {'refresh_token': refreshToken},
      {'token': refreshToken},
      {'refresh': refreshToken},
    ];

    for (final payload in payloadCandidates) {
      try {
        debugPrint(
            'Attempting token refresh with payload keys: ${payload.keys}');
        final response = await _dio.post(
          ApiConfig.refresh,
          data: payload,
          options: Options(
            extra: {
              'skipAuth': true,
              'skipRefresh': true,
            },
          ),
        );

        debugPrint('Refresh response status: ${response.statusCode}');
        debugPrint('Refresh response data: ${response.data}');

        final responseData = Map<String, dynamic>.from(response.data as Map);
        final accessToken = responseData['accessToken'] as String? ??
            responseData['token'] as String?;
        final newRefreshToken = responseData['refreshToken'] as String? ??
            responseData['refresh_token'] as String? ??
            refreshToken;

        if (accessToken == null || accessToken.isEmpty) {
          // try next payload
          continue;
        }

        await _tokenStore.saveTokens(accessToken, newRefreshToken);
        return accessToken;
      } on DioException catch (error) {
        debugPrint(
            'Token refresh attempt failed with payload keys: ${payload.keys}');
        debugPrint('Error status: ${error.response?.statusCode}');
        debugPrint('Error data: ${error.response?.data}');

        if (error.response?.statusCode == 401 ||
            error.response?.statusCode == 403) {
          await _tokenStore.clearToken();
          return null;
        }

        // otherwise try the next payload candidate
        continue;
      }
    }

    return null;
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

import 'package:firebase_messaging/firebase_messaging.dart';
import 'package:flutter/foundation.dart';

// Top-level background message handler
Future<void> _firebaseMessagingBackgroundHandler(RemoteMessage message) async {
  debugPrint('Handling a background message: ${message.messageId}');
}

class NotificationService {
  final FirebaseMessaging _firebaseMessaging = FirebaseMessaging.instance;
  String? _fcmToken;

  String? get fcmToken => _fcmToken;

  Future<void> initialize() async {
    // Request permission for iOS
    NotificationSettings settings = await _firebaseMessaging.requestPermission(
      alert: true,
      badge: true,
      sound: true,
      provisional: false,
    );

    if (settings.authorizationStatus == AuthorizationStatus.authorized) {
      debugPrint('User granted notification permission');
    } else {
      debugPrint('User declined or has not accepted notification permission');
    }

    // Get FCM token
    _fcmToken = await _firebaseMessaging.getToken();
    debugPrint('FCM Token: $_fcmToken');

    // TODO: Send token to backend
    // await _sendTokenToBackend(_fcmToken);

    // Listen to token refresh
    _firebaseMessaging.onTokenRefresh.listen((newToken) {
      _fcmToken = newToken;
      debugPrint('FCM Token refreshed: $newToken');
      // TODO: Send updated token to backend
    });

    // Configure message handlers
    FirebaseMessaging.onBackgroundMessage(_firebaseMessagingBackgroundHandler);

    // Handle foreground messages
    FirebaseMessaging.onMessage.listen((RemoteMessage message) {
      debugPrint('Got a message whilst in the foreground!');
      debugPrint('Message data: ${message.data}');

      if (message.notification != null) {
        debugPrint(
            'Message also contained a notification: ${message.notification}');
        // TODO: Show local notification when app is in foreground
      }
    });

    // Handle notification tap when app is in background
    FirebaseMessaging.onMessageOpenedApp.listen((RemoteMessage message) {
      debugPrint('Notification tapped!');
      // TODO: Navigate to specific screen based on message data
    });

    // Check if app was opened from terminated state via notification
    RemoteMessage? initialMessage =
        await _firebaseMessaging.getInitialMessage();
    if (initialMessage != null) {
      debugPrint('App opened from terminated state via notification');
      // TODO: Handle initial message
    }
  }

  // TODO: Implement method to send token to backend
  // Future<void> _sendTokenToBackend(String? token) async {
  //   if (token == null) return;
  //   // Use ApiService to send token to backend
  // }
}

# Ember App - Flutter Frontend

A mobile-first cross-platform ember application built with Flutter.

## Features

- 📱 Mobile-first design (iOS & Android)
- 🌐 Web support
- 🔔 Push notifications via Firebase
- 🎨 Material Design 3
- 🌓 Dark mode support
- 🔄 State management with Provider
- 🌐 RESTful API integration

## Getting Started

### Prerequisites

- Flutter SDK (>=3.0.0)
- Dart SDK (>=3.0.0)
- Firebase project setup
- Android Studio / Xcode (for mobile development)

### Installation

1. Install dependencies:
```bash
flutter pub get
```

2. Generate model files:
```bash
flutter pub run build_runner build --delete-conflicting-outputs
```

3. Configure Firebase (see SETUP.md in root directory)

4. Run the app:
```bash
# Mobile
flutter run

# Web
flutter run -d chrome
```

## Project Structure

```
lib/
├── config/          # Configuration files
├── models/          # Data models
├── screens/         # UI screens
├── services/        # API & Firebase services
├── widgets/         # Reusable widgets
└── main.dart        # App entry point
```

## Future Enhancements

- Responsive design for tablets
- Improved web UI
- Offline playback
- Custom audio player
- User authentication

For detailed setup instructions, see the main [SETUP.md](../SETUP.md) file.

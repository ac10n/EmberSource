# Fedora Web Build Instructions

These steps build, verify, and locally serve the Flutter app for web on Fedora Linux.

## Prerequisites

- Fedora Workstation or another Fedora-based Linux distribution
- `git`, `unzip`, `xz`, `zip`, and `curl`
- Flutter SDK installed and available on `PATH`
- Google Chrome or Chromium installed for local web runs
- A terminal with access to the repository root at `/home/EmberSource/flutter_app`

## Install dependencies on Fedora

```bash
sudo dnf install -y git unzip xz zip curl
```

## Install Flutter

If Flutter is not already installed, clone the SDK somewhere on your machine and add it to your shell profile:

```bash
git clone https://github.com/flutter/flutter.git -b stable ~/flutter
echo 'export PATH="$HOME/flutter/bin:$PATH"' >> ~/.bashrc
source ~/.bashrc
```

Verify the installation:

```bash
flutter --version
flutter doctor
```

If `flutter doctor` reports missing web tooling, install Chrome or Chromium and run it again.

## Preflight Check

Before building, make sure you are in the Flutter project root and the package graph is up to date:

```bash
cd /home/EmberSource/flutter_app
flutter clean
flutter pub get
```

If the project uses generated models or JSON serialization, regenerate them before the build:

```bash
flutter pub run build_runner build --delete-conflicting-outputs
```

## Build the web app

From the Flutter app directory, run the release build:

```bash
flutter build web --release
```

The compiled web output is written to `build/web`.

## Confirm The Build

After the build completes, verify the output exists:

```bash
ls build/web
```

You should see files such as `index.html`, `main.dart.js`, and `flutter_bootstrap.js`.

## Run locally

To preview the app in Chrome during development:

```bash
flutter run -d chrome
```

If Chrome is not detected, install Chromium or Chrome and rerun `flutter doctor`.

To serve the compiled web output locally after the release build:

```bash
cd build/web
python3 -m http.server 8080
```

Then open `http://localhost:8080` in a browser.

## Clean Rebuild

If the build fails because of stale artifacts, repeat the full sequence:

```bash
cd /home/EmberSource/flutter_app
flutter clean
flutter pub get
flutter pub run build_runner build --delete-conflicting-outputs
flutter build web --release
```

## Notes

- This repository already supports web in `pubspec.yaml` and `web/`.
- Firebase and notification initialization may need platform-specific configuration before production deployment.
- On this machine, Flutter is not currently available on `PATH`, so the commands above could not be executed here until the SDK is installed or added to the shell environment.
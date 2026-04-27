import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../services/auth_service.dart';
import 'auth_landing_screen.dart';
import 'main_screen.dart';

class AuthGate extends StatefulWidget {
  const AuthGate({super.key});

  @override
  State<AuthGate> createState() => _AuthGateState();
}

class _AuthGateState extends State<AuthGate> {
  bool? _isAuthenticated;

  @override
  void initState() {
    super.initState();
    _checkAuth();
  }

  Future<void> _checkAuth() async {
    final authService = context.read<AuthService>();
    final valid = await authService.hasValidToken();
    if (mounted) {
      setState(() {
        _isAuthenticated = valid;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isAuthenticated == null) {
      return const Scaffold(
        body: Center(child: CircularProgressIndicator()),
      );
    }

    if (_isAuthenticated == true) {
      return const MainScreen();
    }

    return const AuthLandingScreen();
  }
}

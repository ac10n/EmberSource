import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import 'login_screen.dart';
import 'register_screen.dart';

class AuthLandingScreen extends StatelessWidget {
  const AuthLandingScreen({super.key});

  static final Uri _websiteUri = Uri.parse('https://mehdipodcast.ca');

  void _openLogin(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => const LoginScreen()),
    );
  }

  void _openRegister(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => const RegisterScreen()),
    );
  }

  Future<void> _openWebsite() async {
    await launchUrl(
      _websiteUri,
      mode: LaunchMode.externalApplication,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [Color(0xFF0F172A), Color(0xFF020617)],
            ),
          ),
          child: Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 420),
              child: Padding(
                padding: const EdgeInsets.all(24),
                child: Container(
                  padding: const EdgeInsets.all(24),
                  decoration: BoxDecoration(
                    color: const Color(0xFF111827),
                    borderRadius: BorderRadius.circular(16),
                    border: Border.all(color: const Color(0xFF1F2937)),
                  ),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      const Text(
                        'Welcome to Podcast App',
                        style: TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.w600,
                          color: Color(0xFFE5E7EB),
                        ),
                      ),
                      const SizedBox(height: 8),
                      const Text(
                        'Sign in to continue, or create a new account.',
                        style: TextStyle(
                          color: Color(0xFF9CA3AF),
                          height: 1.5,
                        ),
                      ),
                      const SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: () => _openLogin(context),
                        child: const Text('Login'),
                      ),
                      const SizedBox(height: 12),
                      OutlinedButton(
                        onPressed: () => _openRegister(context),
                        child: const Text('Create account'),
                      ),
                      const SizedBox(height: 16),
                      TextButton(
                        onPressed: _openWebsite,
                        child: const Text('Visit mehdipodcast.ca'),
                      ),
                      const SizedBox(height: 8),
                      const Text(
                        'Email confirmation is required after registration.',
                        style: TextStyle(
                          color: Color(0xFF9CA3AF),
                          fontSize: 12,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

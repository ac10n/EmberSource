import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/profile.dart';
import '../services/auth_service.dart';
import '../widgets/app_drawer.dart';
import 'auth_landing_screen.dart';
import 'content_list_screen.dart';
import 'invitations_screen.dart';

class ProfileScreen extends StatefulWidget {
  const ProfileScreen({super.key});

  @override
  State<ProfileScreen> createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  final _infoFormKey = GlobalKey<FormState>();
  final _passwordFormKey = GlobalKey<FormState>();
  final _fullNameController = TextEditingController();
  final _birthYearController = TextEditingController();
  final _jurisdictionController = TextEditingController();
  final _oldPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  AuthService? _authService;
  ProfileInfo? _profile;
  bool _loading = true;
  bool _savingInfo = false;
  bool _changingPassword = false;
  String? _error;

  @override
  void dispose() {
    _fullNameController.dispose();
    _birthYearController.dispose();
    _jurisdictionController.dispose();
    _oldPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    _authService ??= context.read<AuthService>();
    if (_loading && _profile == null) {
      _loadProfile();
    }
  }

  Future<void> _loadProfile() async {
    if (_authService == null) return;

    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      final profile = await _authService!.getProfile();
      if (!mounted) return;
      setState(() {
        _profile = profile;
        _fullNameController.text = profile.fullName;
        _birthYearController.text =
            profile.birthYear == 0 ? '' : profile.birthYear.toString();
        _jurisdictionController.text = profile.jurisdiction;
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _error = e.toString();
        _loading = false;
      });
    }
  }

  Future<void> _saveInfo() async {
    if (_authService == null) return;
    if (!(_infoFormKey.currentState?.validate() ?? false)) return;

    setState(() => _savingInfo = true);

    try {
      await _authService!.updateProfile(
        fullName: _fullNameController.text.trim(),
        birthYear: int.parse(_birthYearController.text.trim()),
        jurisdiction: _jurisdictionController.text.trim(),
      );
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Profile updated')),
      );
      await _loadProfile();
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error: $e')),
      );
    } finally {
      if (mounted) setState(() => _savingInfo = false);
    }
  }

  Future<void> _changePassword() async {
    if (_authService == null) return;
    if (!(_passwordFormKey.currentState?.validate() ?? false)) return;

    setState(() => _changingPassword = true);

    try {
      await _authService!.changePassword(
        oldPassword: _oldPasswordController.text,
        newPassword: _newPasswordController.text,
      );
      if (!mounted) return;
      _oldPasswordController.clear();
      _newPasswordController.clear();
      _confirmPasswordController.clear();
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Password changed')),
      );
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error: $e')),
      );
    } finally {
      if (mounted) setState(() => _changingPassword = false);
    }
  }

  Future<void> _logout() async {
    if (!mounted) return;

    Navigator.of(context).pop();
    await context.read<AuthService>().logout();
    if (!mounted) return;

    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const AuthLandingScreen()),
      (route) => false,
    );
  }

  void _openSection(AppSection section) {
    Widget? destination;
    switch (section) {
      case AppSection.explore:
      case AppSection.contents:
        destination = const ContentListScreen();
        break;
      case AppSection.invite:
        destination = const InvitationsScreen(openCreateDialogOnStart: true);
        break;
      case AppSection.invitationList:
        destination = const InvitationsScreen();
        break;
      case AppSection.profile:
        Navigator.of(context).pop();
        return;
    }

    Navigator.of(context).pushReplacement(
      MaterialPageRoute(builder: (_) => destination!),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: AppDrawer(
        selectedSection: AppSection.profile,
        onSectionSelected: _openSection,
        onLogout: _logout,
      ),
      appBar: AppBar(
        title: const Text('Profile'),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: _loadProfile,
          ),
        ],
      ),
      body: _loading
          ? const Center(child: CircularProgressIndicator())
          : _error != null
              ? Center(
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text(_error!, style: const TextStyle(color: Colors.red)),
                      const SizedBox(height: 12),
                      ElevatedButton(
                        onPressed: _loadProfile,
                        child: const Text('Retry'),
                      ),
                    ],
                  ),
                )
              : RefreshIndicator(
                  onRefresh: _loadProfile,
                  child: ListView(
                    padding: const EdgeInsets.all(16),
                    children: [
                      Card(
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Information',
                                style: Theme.of(context).textTheme.titleLarge,
                              ),
                              const SizedBox(height: 16),
                              Text('Username: ${_profile?.username ?? ''}'),
                              const SizedBox(height: 16),
                              Form(
                                key: _infoFormKey,
                                child: Column(
                                  children: [
                                    TextFormField(
                                      controller: _fullNameController,
                                      decoration: const InputDecoration(
                                        labelText: 'Full name',
                                      ),
                                      validator: (value) =>
                                          value == null || value.trim().isEmpty
                                              ? 'Full name is required'
                                              : null,
                                    ),
                                    const SizedBox(height: 12),
                                    TextFormField(
                                      controller: _birthYearController,
                                      decoration: const InputDecoration(
                                        labelText: 'Birth year',
                                      ),
                                      keyboardType:
                                          const TextInputType.numberWithOptions(
                                        signed: false,
                                        decimal: false,
                                      ),
                                      validator: (value) {
                                        final parsed =
                                            int.tryParse(value?.trim() ?? '');
                                        if (parsed == null || parsed < 1900) {
                                          return 'Enter a valid birth year';
                                        }
                                        return null;
                                      },
                                    ),
                                    const SizedBox(height: 12),
                                    TextFormField(
                                      controller: _jurisdictionController,
                                      decoration: const InputDecoration(
                                        labelText: 'Jurisdiction',
                                      ),
                                      validator: (value) =>
                                          value == null || value.trim().isEmpty
                                              ? 'Jurisdiction is required'
                                              : null,
                                    ),
                                    const SizedBox(height: 16),
                                    Align(
                                      alignment: Alignment.centerRight,
                                      child: FilledButton(
                                        onPressed:
                                            _savingInfo ? null : _saveInfo,
                                        child: _savingInfo
                                            ? const SizedBox(
                                                width: 18,
                                                height: 18,
                                                child:
                                                    CircularProgressIndicator(
                                                  strokeWidth: 2,
                                                ),
                                              )
                                            : const Text('Save information'),
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      const SizedBox(height: 16),
                      Card(
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Change Password',
                                style: Theme.of(context).textTheme.titleLarge,
                              ),
                              const SizedBox(height: 16),
                              Form(
                                key: _passwordFormKey,
                                child: Column(
                                  children: [
                                    TextFormField(
                                      controller: _oldPasswordController,
                                      decoration: const InputDecoration(
                                        labelText: 'Current password',
                                      ),
                                      obscureText: true,
                                      validator: (value) =>
                                          value == null || value.isEmpty
                                              ? 'Current password is required'
                                              : null,
                                    ),
                                    const SizedBox(height: 12),
                                    TextFormField(
                                      controller: _newPasswordController,
                                      decoration: const InputDecoration(
                                        labelText: 'New password',
                                      ),
                                      obscureText: true,
                                      validator: (value) {
                                        if (value == null || value.isEmpty) {
                                          return 'New password is required';
                                        }
                                        if (value.length < 6) {
                                          return 'Use at least 6 characters';
                                        }
                                        return null;
                                      },
                                    ),
                                    const SizedBox(height: 12),
                                    TextFormField(
                                      controller: _confirmPasswordController,
                                      decoration: const InputDecoration(
                                        labelText: 'Confirm new password',
                                      ),
                                      obscureText: true,
                                      validator: (value) {
                                        if (value == null || value.isEmpty) {
                                          return 'Please confirm the password';
                                        }
                                        if (value !=
                                            _newPasswordController.text) {
                                          return 'Passwords do not match';
                                        }
                                        return null;
                                      },
                                    ),
                                    const SizedBox(height: 16),
                                    Align(
                                      alignment: Alignment.centerRight,
                                      child: FilledButton(
                                        onPressed: _changingPassword
                                            ? null
                                            : _changePassword,
                                        child: _changingPassword
                                            ? const SizedBox(
                                                width: 18,
                                                height: 18,
                                                child:
                                                    CircularProgressIndicator(
                                                  strokeWidth: 2,
                                                ),
                                              )
                                            : const Text('Change password'),
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
    );
  }
}

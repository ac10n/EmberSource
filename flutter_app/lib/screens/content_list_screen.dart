import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../widgets/app_drawer.dart';
import '../models/knowledge_content.dart';
import '../models/knowledge_collection.dart';
import '../models/knowledge_tag.dart';
import '../services/api_service.dart';
import '../services/auth_service.dart';
import '../services/auth_token_store.dart';
import '../services/knowledge_service.dart';
import '../utils/jwt_utils.dart';
import '../widgets/knowledge_pickers.dart';
import 'auth_landing_screen.dart';
import 'content_editor_screen.dart';
import 'invitations_screen.dart';
import 'register_screen.dart';
import 'profile_screen.dart';

class ContentListScreen extends StatefulWidget {
  const ContentListScreen({super.key});

  @override
  State<ContentListScreen> createState() => _ContentListScreenState();
}

class _ContentListScreenState extends State<ContentListScreen> {
  KnowledgeService? _knowledgeService;
  List<KnowledgeContent> _contents = [];
  bool _isLoading = true;
  String? _error;
  bool _canRegisterUser = false;

  final Map<String, Set<String>> _contentTagSelections = {};
  final Map<String, Set<String>> _contentCollectionSelections = {};

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_knowledgeService == null) {
      _knowledgeService = KnowledgeService(context.read<ApiService>());
      _checkAdminPermission();
      _loadContents();
    }
  }

  Future<void> _checkAdminPermission() async {
    final valid = await context.read<AuthService>().hasValidToken();
    if (!valid) return;

    final token = await AuthTokenStore().readAccessToken();
    if (token != null) {
      final can = JwtUtils.hasClaim(token, 'AllowToRegisterUser');
      if (mounted) setState(() => _canRegisterUser = can);
    }
  }

  Future<void> _loadContents() async {
    if (_knowledgeService == null) return;

    setState(() {
      _isLoading = true;
      _error = null;
    });

    final valid = await context.read<AuthService>().hasValidToken();
    if (!valid) {
      await _handleSessionExpired();
      return;
    }

    try {
      final items = await _knowledgeService!.getContents();
      setState(() {
        _contents = items;
        _isLoading = false;
      });
    } catch (e) {
      final errorText = e.toString();
      if (errorText.contains('401') || errorText.contains('Unauthorized')) {
        await _handleSessionExpired();
        return;
      }

      setState(() {
        _error = errorText;
        _isLoading = false;
      });
    }
  }

  Future<void> _openEditor({KnowledgeContent? content}) async {
    final navigator = Navigator.of(context);
    await navigator.push(
      MaterialPageRoute(
        builder: (_) => ContentEditorScreen(initialContent: content),
      ),
    );

    if (!mounted) return;
    await _loadContents();
  }

  Future<void> _editTagsForContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    List<KnowledgeTag> tags;
    try {
      tags = await _knowledgeService!.getTags();
    } catch (e) {
      if (_isUnauthorizedError(e)) {
        await _handleSessionExpired();
        return;
      }
      rethrow;
    }
    if (!mounted) return;
    final selected = _contentTagSelections[content.id] ?? <String>{};

    final updated = await showTagPickerDialog(
      context: context,
      tags: tags,
      selected: selected,
    );

    if (updated == null) return;

    final selectedTags = tags.where((tag) => updated.contains(tag.id)).toList();
    try {
      await _knowledgeService!.updateContent(
        contentId: content.id,
        parentContentId: content.parentContentId,
        contentType: content.contentType,
        contentFormat: content.contentFormat,
        formatVersion: 1,
        title: content.title,
        data: content.data ?? '',
        contentVisibility: content.contentVisibility,
        visibilityCriteria: content.visibilityCriteria,
        tags: selectedTags,
        collectionIds: null,
      );
    } catch (e) {
      if (_isUnauthorizedError(e)) {
        await _handleSessionExpired();
        return;
      }
      rethrow;
    }

    setState(() {
      _contentTagSelections[content.id] = updated;
    });
  }

  Future<void> _editCollectionsForContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    List<KnowledgeCollection> collections;
    try {
      collections = await _knowledgeService!.getCollections();
    } catch (e) {
      if (_isUnauthorizedError(e)) {
        await _handleSessionExpired();
        return;
      }
      rethrow;
    }
    if (!mounted) return;
    final selected = _contentCollectionSelections[content.id] ?? <String>{};

    final updated = await showCollectionPickerDialog(
      context: context,
      collections: collections,
      selected: selected,
    );

    if (updated == null) return;

    try {
      await _knowledgeService!.updateContent(
        contentId: content.id,
        parentContentId: content.parentContentId,
        contentType: content.contentType,
        contentFormat: content.contentFormat,
        formatVersion: 1,
        title: content.title,
        data: content.data ?? '',
        contentVisibility: content.contentVisibility,
        visibilityCriteria: content.visibilityCriteria,
        tags: null,
        collectionIds: updated.toList(),
      );
    } catch (e) {
      if (_isUnauthorizedError(e)) {
        await _handleSessionExpired();
        return;
      }
      rethrow;
    }

    setState(() {
      _contentCollectionSelections[content.id] = updated;
    });
  }

  Future<void> _deactivateContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    try {
      await _knowledgeService!.deactivateContent(content.identifier);
    } catch (e) {
      if (_isUnauthorizedError(e)) {
        await _handleSessionExpired();
        return;
      }
      rethrow;
    }
    await _loadContents();
  }

  bool _isUnauthorizedError(Object error) {
    final text = error.toString();
    return text.contains('401') || text.contains('Unauthorized');
  }

  Future<void> _handleSessionExpired() async {
    if (!mounted) return;

    await context.read<AuthService>().logout();
    if (!mounted) return;

    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const AuthLandingScreen()),
      (route) => false,
    );
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
    if (section == AppSection.explore || section == AppSection.contents) {
      Navigator.of(context).pop();
      return;
    }

    Widget? destination;
    switch (section) {
      case AppSection.invite:
        destination = const InvitationsScreen(openCreateDialogOnStart: true);
        break;
      case AppSection.invitationList:
        destination = const InvitationsScreen();
        break;
      case AppSection.profile:
        destination = const ProfileScreen();
        break;
      case AppSection.explore:
      case AppSection.contents:
        destination = const ContentListScreen();
        break;
    }

    Navigator.of(context).pushReplacement(
      MaterialPageRoute(builder: (_) => destination!),
    );
  }

  Future<void> _showContextMenu(
    Offset position,
    KnowledgeContent content,
  ) async {
    final overlay = Overlay.of(context).context.findRenderObject() as RenderBox;
    final selected = await showMenu<String>(
      context: context,
      position: RelativeRect.fromRect(
        position & const Size(40, 40),
        Offset.zero & overlay.size,
      ),
      items: const [
        PopupMenuItem(value: 'edit', child: Text('Edit content')),
        PopupMenuItem(value: 'tags', child: Text('Edit tags')),
        PopupMenuItem(value: 'collections', child: Text('Edit collections')),
        PopupMenuItem(value: 'deactivate', child: Text('Deactivate')),
      ],
    );

    switch (selected) {
      case 'edit':
        await _openEditor(content: content);
        break;
      case 'tags':
        await _editTagsForContent(content);
        break;
      case 'collections':
        await _editCollectionsForContent(content);
        break;
      case 'deactivate':
        await _deactivateContent(content);
        break;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: AppDrawer(
        selectedSection: AppSection.contents,
        onSectionSelected: _openSection,
        onLogout: _logout,
      ),
      appBar: AppBar(
        title: const Text('My Knowledge'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        actions: [
          if (_canRegisterUser)
            PopupMenuButton<String>(
              onSelected: (value) {
                if (value == 'register') {
                  Navigator.of(context).push(
                    MaterialPageRoute(builder: (_) => const RegisterScreen()),
                  );
                }
              },
              itemBuilder: (_) => const [
                PopupMenuItem(
                  value: 'register',
                  child: ListTile(
                    leading: Icon(Icons.person_add_outlined),
                    title: Text('Register User'),
                    contentPadding: EdgeInsets.zero,
                  ),
                ),
              ],
            ),
        ],
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => _openEditor(),
        icon: const Icon(Icons.add),
        label: const Text('New Knowledge'),
      ),
      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_error != null) {
      return Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Icon(Icons.error_outline, size: 48, color: Colors.red),
            const SizedBox(height: 16),
            Text('Error: $_error'),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: _loadContents,
              child: const Text('Retry'),
            ),
          ],
        ),
      );
    }

    if (_contents.isEmpty) {
      return const Center(
        child: Text('No knowledge items yet'),
      );
    }

    return RefreshIndicator(
      onRefresh: _loadContents,
      child: ListView.builder(
        padding: const EdgeInsets.all(16),
        itemCount: _contents.length,
        itemBuilder: (context, index) {
          final content = _contents[index];
          return GestureDetector(
            onLongPressStart: (details) =>
                _showContextMenu(details.globalPosition, content),
            onSecondaryTapDown: (details) =>
                _showContextMenu(details.globalPosition, content),
            child: Card(
              margin: const EdgeInsets.only(bottom: 16),
              elevation: 2,
              child: ListTile(
                title: Text(content.title?.isNotEmpty == true
                    ? content.title!
                    : 'Untitled content'),
                subtitle: Text('Type: ${content.contentType.name}'),
                trailing: const Icon(Icons.more_vert),
                onTap: () => _openEditor(content: content),
              ),
            ),
          );
        },
      ),
    );
  }
}

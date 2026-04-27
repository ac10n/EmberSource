import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/knowledge_content.dart';
import '../models/knowledge_tag.dart';
import '../services/api_service.dart';
import '../services/knowledge_service.dart';
import '../widgets/knowledge_pickers.dart';
import 'content_editor_screen.dart';

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

  final Map<String, Set<String>> _contentTagSelections = {};
  final Map<String, Set<String>> _contentCollectionSelections = {};

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_knowledgeService == null) {
      _knowledgeService = KnowledgeService(context.read<ApiService>());
      _loadContents();
    }
  }

  Future<void> _loadContents() async {
    if (_knowledgeService == null) return;

    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final items = await _knowledgeService!.getContents();
      setState(() {
        _contents = items;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _openEditor({KnowledgeContent? content}) async {
    await Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => ContentEditorScreen(initialContent: content),
      ),
    );

    await _loadContents();
  }

  Future<void> _editTagsForContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    final tags = await _knowledgeService!.getTags();
    final selected = _contentTagSelections[content.id] ?? <String>{};

    final updated = await showTagPickerDialog(
      context: context,
      tags: tags,
      selected: selected,
    );

    if (updated == null) return;

    final selectedTags = tags.where((tag) => updated.contains(tag.id)).toList();
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

    setState(() {
      _contentTagSelections[content.id] = updated;
    });
  }

  Future<void> _editCollectionsForContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    final collections = await _knowledgeService!.getCollections();
    final selected = _contentCollectionSelections[content.id] ?? <String>{};

    final updated = await showCollectionPickerDialog(
      context: context,
      collections: collections,
      selected: selected,
    );

    if (updated == null) return;

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

    setState(() {
      _contentCollectionSelections[content.id] = updated;
    });
  }

  Future<void> _deactivateContent(KnowledgeContent content) async {
    if (_knowledgeService == null) return;
    await _knowledgeService!.deactivateContent(content.id);
    await _loadContents();
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
      appBar: AppBar(
        title: const Text('Knowledge Content'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => _openEditor(),
        icon: const Icon(Icons.add),
        label: const Text('New Content'),
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

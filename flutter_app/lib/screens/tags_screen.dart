import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/knowledge_tag.dart';
import '../services/api_service.dart';
import '../services/knowledge_service.dart';

class TagsScreen extends StatefulWidget {
  const TagsScreen({super.key});

  @override
  State<TagsScreen> createState() => _TagsScreenState();
}

class _TagsScreenState extends State<TagsScreen> {
  KnowledgeService? _knowledgeService;
  List<KnowledgeTag> _tags = [];
  bool _isLoading = true;
  String? _error;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_knowledgeService == null) {
      _knowledgeService = KnowledgeService(context.read<ApiService>());
      _loadTags();
    }
  }

  Future<void> _loadTags() async {
    if (_knowledgeService == null) return;

    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final tags = await _knowledgeService!.getTags();
      setState(() {
        _tags = tags;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _openTagDialog({KnowledgeTag? tag}) async {
    final controller = TextEditingController(text: tag?.name ?? '');
    bool isPrivate = tag?.isPrivate ?? false;
    bool hasConfidenceRate = tag?.hasConfidenceRate ?? false;

    final result = await showDialog<KnowledgeTag>(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          title: Text(tag == null ? 'New Tag' : 'Edit Tag'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: controller,
                decoration: const InputDecoration(labelText: 'Name'),
              ),
              const SizedBox(height: 12),
              SwitchListTile(
                title: const Text('Private'),
                value: isPrivate,
                onChanged: (value) {
                  isPrivate = value;
                  (dialogContext as Element).markNeedsBuild();
                },
              ),
              SwitchListTile(
                title: const Text('Has confidence rate'),
                value: hasConfidenceRate,
                onChanged: (value) {
                  hasConfidenceRate = value;
                  (dialogContext as Element).markNeedsBuild();
                },
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(),
              child: const Text('Cancel'),
            ),
            ElevatedButton(
              onPressed: () {
                final name = controller.text.trim();
                if (name.isEmpty) return;
                Navigator.of(dialogContext).pop(
                  KnowledgeTag(
                    id: tag?.id ?? '',
                    name: name,
                    isPrivate: isPrivate,
                    hasConfidenceRate: hasConfidenceRate,
                  ),
                );
              },
              child: const Text('Save'),
            ),
          ],
        );
      },
    );

    if (result == null || _knowledgeService == null) return;

    try {
      if (tag == null) {
        await _knowledgeService!.createTag(result);
      } else {
        await _knowledgeService!.updateTag(tag.id, result);
      }
      await _loadTags();
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to save tag: $e')),
      );
    }
  }

  Future<void> _deleteTag(KnowledgeTag tag) async {
    if (_knowledgeService == null) return;
    await _knowledgeService!.deleteTag(tag.id);
    await _loadTags();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Tags'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => _openTagDialog(),
        child: const Icon(Icons.add),
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
              onPressed: _loadTags,
              child: const Text('Retry'),
            ),
          ],
        ),
      );
    }

    if (_tags.isEmpty) {
      return const Center(child: Text('No tags yet'));
    }

    return ListView.builder(
      padding: const EdgeInsets.all(16),
      itemCount: _tags.length,
      itemBuilder: (context, index) {
        final tag = _tags[index];
        return Card(
          margin: const EdgeInsets.only(bottom: 12),
          child: ListTile(
            title: Text(tag.name),
            subtitle: Text(tag.isPrivate ? 'Private' : 'Public'),
            trailing: PopupMenuButton<String>(
              onSelected: (value) {
                if (value == 'edit') {
                  _openTagDialog(tag: tag);
                } else if (value == 'delete') {
                  _deleteTag(tag);
                }
              },
              itemBuilder: (context) => const [
                PopupMenuItem(value: 'edit', child: Text('Edit')),
                PopupMenuItem(value: 'delete', child: Text('Delete')),
              ],
            ),
          ),
        );
      },
    );
  }
}

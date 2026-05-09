import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/knowledge_collection.dart';
import '../services/api_service.dart';
import '../services/knowledge_service.dart';

class CollectionsScreen extends StatefulWidget {
  const CollectionsScreen({super.key});

  @override
  State<CollectionsScreen> createState() => _CollectionsScreenState();
}

class _CollectionsScreenState extends State<CollectionsScreen> {
  KnowledgeService? _knowledgeService;
  List<KnowledgeCollection> _collections = [];
  bool _isLoading = true;
  String? _error;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_knowledgeService == null) {
      _knowledgeService = KnowledgeService(context.read<ApiService>());
      _loadCollections();
    }
  }

  Future<void> _loadCollections() async {
    if (_knowledgeService == null) return;

    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final collections = await _knowledgeService!.getCollections();
      setState(() {
        _collections = collections;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _openCollectionDialog({KnowledgeCollection? collection}) async {
    final nameController = TextEditingController(text: collection?.name ?? '');
    final descriptionController =
        TextEditingController(text: collection?.description ?? '');

    final result = await showDialog<KnowledgeCollection>(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          title:
              Text(collection == null ? 'New Collection' : 'Edit Collection'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: nameController,
                decoration: const InputDecoration(labelText: 'Name'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: descriptionController,
                decoration: const InputDecoration(labelText: 'Description'),
                maxLines: 3,
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
                final name = nameController.text.trim();
                if (name.isEmpty) return;
                Navigator.of(dialogContext).pop(
                  KnowledgeCollection(
                    id: collection?.id ?? '',
                    name: name,
                    description: descriptionController.text.trim().isEmpty
                        ? null
                        : descriptionController.text.trim(),
                    emberUserId: collection?.emberUserId ?? '',
                    createdAt: collection?.createdAt ?? DateTime.now().toUtc(),
                    items: collection?.items ?? [],
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
      if (collection == null) {
        await _knowledgeService!.createCollection(result);
      } else {
        await _knowledgeService!.updateCollection(collection.id, result);
      }
      await _loadCollections();
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to save collection: $e')),
      );
    }
  }

  Future<void> _deleteCollection(KnowledgeCollection collection) async {
    if (_knowledgeService == null) return;
    await _knowledgeService!.deleteCollection(collection.id);
    await _loadCollections();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Collections'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => _openCollectionDialog(),
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
              onPressed: _loadCollections,
              child: const Text('Retry'),
            ),
          ],
        ),
      );
    }

    if (_collections.isEmpty) {
      return const Center(child: Text('No collections yet'));
    }

    return ListView.builder(
      padding: const EdgeInsets.all(16),
      itemCount: _collections.length,
      itemBuilder: (context, index) {
        final collection = _collections[index];
        return Card(
          margin: const EdgeInsets.only(bottom: 12),
          child: ListTile(
            title: Text(collection.name),
            subtitle: Text(collection.description ?? 'No description'),
            trailing: PopupMenuButton<String>(
              onSelected: (value) {
                if (value == 'edit') {
                  _openCollectionDialog(collection: collection);
                } else if (value == 'delete') {
                  _deleteCollection(collection);
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

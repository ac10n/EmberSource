import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/knowledge_block.dart';
import '../models/knowledge_collection.dart';
import '../models/knowledge_content.dart';
import '../models/knowledge_enums.dart';
import '../models/knowledge_tag.dart';
import '../services/api_service.dart';
import '../services/knowledge_service.dart';
import '../widgets/knowledge_pickers.dart';

class ContentEditorScreen extends StatefulWidget {
  final KnowledgeContent? initialContent;

  const ContentEditorScreen({super.key, this.initialContent});

  @override
  State<ContentEditorScreen> createState() => _ContentEditorScreenState();
}

class _ContentEditorScreenState extends State<ContentEditorScreen> {
  KnowledgeService? _knowledgeService;
  final TextEditingController _titleController = TextEditingController();
  final List<ContentBlock> _blocks = [];
  bool _isSaving = false;

  List<KnowledgeTag> _tags = [];
  List<KnowledgeCollection> _collections = [];
  Set<String> _selectedTagIds = {};
  Set<String> _selectedCollectionIds = {};

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_knowledgeService == null) {
      _knowledgeService = KnowledgeService(context.read<ApiService>());
      _hydrateInitialContent();
      _loadPickers();
    }
  }

  @override
  void dispose() {
    _titleController.dispose();
    super.dispose();
  }

  void _hydrateInitialContent() {
    final initial = widget.initialContent;
    if (initial == null) {
      _blocks.add(_newParagraphBlock());
      return;
    }

    _titleController.text = initial.title ?? '';

    final blocks = _decodeBlocks(initial.data);
    if (blocks.isEmpty) {
      _blocks.add(_newParagraphBlock());
    } else {
      _blocks.addAll(blocks);
    }
  }

  Future<void> _loadPickers() async {
    if (_knowledgeService == null) return;
    try {
      final tags = await _knowledgeService!.getTags();
      final collections = await _knowledgeService!.getCollections();
      if (mounted) {
        setState(() {
          _tags = tags;
          _collections = collections;
        });
      }
    } catch (_) {
      if (!mounted) return;
      setState(() {
        _tags = [];
        _collections = [];
      });
    }
  }

  ContentBlock _newParagraphBlock() {
    return ContentBlock(
      localId: DateTime.now().microsecondsSinceEpoch.toString(),
      type: ContentBlockType.paragraph,
      text: '',
    );
  }

  ContentBlock _newHeadingBlock() {
    return ContentBlock(
      localId: DateTime.now().microsecondsSinceEpoch.toString(),
      type: ContentBlockType.heading,
      text: 'Heading',
    );
  }

  ContentBlock _newImageBlock() {
    return ContentBlock(
      localId: DateTime.now().microsecondsSinceEpoch.toString(),
      type: ContentBlockType.image,
      text: '',
      imageUrl: '',
    );
  }

  List<ContentBlock> _decodeBlocks(String? data) {
    if (data == null || data.trim().isEmpty) {
      return [];
    }
    try {
      final decoded = jsonDecode(data) as Map<String, dynamic>;
      final blocks = decoded['blocks'] as List<dynamic>? ?? <dynamic>[];
      return blocks
          .whereType<Map<String, dynamic>>()
          .map(ContentBlock.fromJson)
          .toList();
    } catch (_) {
      return [];
    }
  }

  String _encodeBlocks() {
    final serialized = _blocks.map((block) => block.toJson()).toList();
    return jsonEncode({'blocks': serialized});
  }

  Future<void> _saveContent() async {
    if (_knowledgeService == null) return;
    setState(() => _isSaving = true);

    final title = _titleController.text.trim();
    final data = _encodeBlocks();

    try {
      if (widget.initialContent == null) {
        await _knowledgeService!.createContent(
          contentType: ContentType.article,
          contentFormat: ContentFormat.richText,
          formatVersion: 1,
          title: title.isEmpty ? null : title,
          data: data,
          contentVisibility: ContentVisibility.private,
          tags: _selectedTags,
          collectionIds: _selectedCollectionIds.toList(),
        );
      } else {
        final initial = widget.initialContent!;
        await _knowledgeService!.updateContent(
          contentId: initial.id,
          parentContentId: initial.parentContentId,
          contentType: initial.contentType,
          contentFormat: initial.contentFormat,
          formatVersion: 1,
          title: title.isEmpty ? null : title,
          data: data,
          contentVisibility: initial.contentVisibility,
          visibilityCriteria: initial.visibilityCriteria,
          tags: _selectedTags,
          collectionIds: _selectedCollectionIds.toList(),
        );
      }

      if (mounted) {
        Navigator.of(context).pop();
      }
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to save: $e')),
      );
    } finally {
      if (mounted) {
        setState(() => _isSaving = false);
      }
    }
  }

  List<KnowledgeTag> get _selectedTags {
    if (_tags.isEmpty) return [];
    return _tags.where((tag) => _selectedTagIds.contains(tag.id)).toList();
  }

  Future<void> _editContentTags() async {
    final updated = await showTagPickerDialog(
      context: context,
      tags: _tags,
      selected: _selectedTagIds,
    );

    if (updated == null) return;
    setState(() => _selectedTagIds = updated);
  }

  Future<void> _editContentCollections() async {
    final updated = await showCollectionPickerDialog(
      context: context,
      collections: _collections,
      selected: _selectedCollectionIds,
    );

    if (updated == null) return;
    setState(() => _selectedCollectionIds = updated);
  }

  Future<void> _showBlockMenu(
    Offset position,
    ContentBlock block,
  ) async {
    final overlay = Overlay.of(context).context.findRenderObject() as RenderBox;
    final selected = await showMenu<String>(
      context: context,
      position: RelativeRect.fromRect(
        position & const Size(40, 40),
        Offset.zero & overlay.size,
      ),
      items: const [
        PopupMenuItem(value: 'tags', child: Text('Edit tags')),
        PopupMenuItem(value: 'collections', child: Text('Edit collections')),
        PopupMenuItem(value: 'remove', child: Text('Remove block')),
      ],
    );

    switch (selected) {
      case 'tags':
        final updated = await showTagPickerDialog(
          context: context,
          tags: _tags,
          selected: block.tagIds.toSet(),
        );
        if (updated != null) {
          setState(() {
            block.tagIds
              ..clear()
              ..addAll(updated);
          });
        }
        break;
      case 'collections':
        final updated = await showCollectionPickerDialog(
          context: context,
          collections: _collections,
          selected: block.collectionIds.toSet(),
        );
        if (updated != null) {
          setState(() {
            block.collectionIds
              ..clear()
              ..addAll(updated);
          });
        }
        break;
      case 'remove':
        setState(() => _blocks.remove(block));
        break;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
            widget.initialContent == null ? 'New Content' : 'Edit Content'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        actions: [
          TextButton(
            onPressed: _isSaving ? null : _saveContent,
            child: _isSaving
                ? const SizedBox(
                    width: 16,
                    height: 16,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  )
                : const Text('Save'),
          ),
        ],
      ),
      body: SafeArea(
        child: Column(
          children: [
            _buildHeaderControls(),
            const Divider(height: 1),
            Expanded(child: _buildBlocksList()),
            _buildBlockToolbar(),
          ],
        ),
      ),
    );
  }

  Widget _buildHeaderControls() {
    return Padding(
      padding: const EdgeInsets.all(16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          TextField(
            controller: _titleController,
            decoration: const InputDecoration(
              labelText: 'Title',
              border: OutlineInputBorder(),
            ),
          ),
          const SizedBox(height: 12),
          Wrap(
            spacing: 8,
            runSpacing: 8,
            children: [
              ElevatedButton.icon(
                onPressed: _tags.isEmpty ? null : _editContentTags,
                icon: const Icon(Icons.label_outline),
                label: const Text('Tags'),
              ),
              ElevatedButton.icon(
                onPressed:
                    _collections.isEmpty ? null : _editContentCollections,
                icon: const Icon(Icons.collections_bookmark_outlined),
                label: const Text('Collections'),
              ),
              if (_selectedTagIds.isNotEmpty)
                _buildSelectionChip('Tags', _selectedTagIds.length),
              if (_selectedCollectionIds.isNotEmpty)
                _buildSelectionChip(
                  'Collections',
                  _selectedCollectionIds.length,
                ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildSelectionChip(String label, int count) {
    return Chip(
      label: Text('$label: $count'),
      backgroundColor: Theme.of(context).colorScheme.surfaceContainerHighest,
    );
  }

  Widget _buildBlocksList() {
    return ListView.builder(
      padding: const EdgeInsets.all(16),
      itemCount: _blocks.length,
      itemBuilder: (context, index) {
        final block = _blocks[index];
        return GestureDetector(
          onLongPressStart: (details) =>
              _showBlockMenu(details.globalPosition, block),
          onSecondaryTapDown: (details) =>
              _showBlockMenu(details.globalPosition, block),
          child: Card(
            margin: const EdgeInsets.only(bottom: 16),
            child: Padding(
              padding: const EdgeInsets.all(12),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Text(
                        block.type.name.toUpperCase(),
                        style: Theme.of(context).textTheme.labelLarge,
                      ),
                      const Spacer(),
                      if (block.tagIds.isNotEmpty)
                        const Icon(Icons.label_outline, size: 18),
                      if (block.collectionIds.isNotEmpty)
                        const SizedBox(width: 8),
                      if (block.collectionIds.isNotEmpty)
                        const Icon(Icons.collections_bookmark_outlined,
                            size: 18),
                    ],
                  ),
                  const SizedBox(height: 12),
                  if (block.type == ContentBlockType.image)
                    TextFormField(
                      initialValue: block.imageUrl ?? '',
                      decoration: const InputDecoration(
                        labelText: 'Image URL',
                        border: OutlineInputBorder(),
                      ),
                      onChanged: (value) => block.imageUrl = value,
                    )
                  else
                    TextFormField(
                      initialValue: block.text,
                      decoration: InputDecoration(
                        labelText: block.type == ContentBlockType.heading
                            ? 'Heading'
                            : 'Paragraph',
                        border: const OutlineInputBorder(),
                      ),
                      minLines:
                          block.type == ContentBlockType.paragraph ? 3 : 1,
                      maxLines:
                          block.type == ContentBlockType.paragraph ? 6 : 2,
                      onChanged: (value) => block.text = value,
                    ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildBlockToolbar() {
    return SafeArea(
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
        color: Theme.of(context).colorScheme.surfaceContainerHighest,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            ElevatedButton.icon(
              onPressed: () => setState(() => _blocks.add(_newHeadingBlock())),
              icon: const Icon(Icons.title),
              label: const Text('Heading'),
            ),
            ElevatedButton.icon(
              onPressed: () =>
                  setState(() => _blocks.add(_newParagraphBlock())),
              icon: const Icon(Icons.notes),
              label: const Text('Paragraph'),
            ),
            ElevatedButton.icon(
              onPressed: () => setState(() => _blocks.add(_newImageBlock())),
              icon: const Icon(Icons.image_outlined),
              label: const Text('Image'),
            ),
          ],
        ),
      ),
    );
  }
}

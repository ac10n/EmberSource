import 'package:flutter/material.dart';
import '../models/knowledge_collection.dart';
import '../models/knowledge_tag.dart';

Future<Set<String>?> showTagPickerDialog({
  required BuildContext context,
  required List<KnowledgeTag> tags,
  required Set<String> selected,
}) {
  return showDialog<Set<String>>(
    context: context,
    builder: (dialogContext) {
      final current = Set<String>.from(selected);
      return AlertDialog(
        title: const Text('Select tags'),
        content: SizedBox(
          width: double.maxFinite,
          child: ListView.builder(
            shrinkWrap: true,
            itemCount: tags.length,
            itemBuilder: (context, index) {
              final tag = tags[index];
              final isSelected = current.contains(tag.id);
              return CheckboxListTile(
                value: isSelected,
                title: Text(tag.name),
                onChanged: (value) {
                  if (value == true) {
                    current.add(tag.id);
                  } else {
                    current.remove(tag.id);
                  }
                  (dialogContext as Element).markNeedsBuild();
                },
              );
            },
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(dialogContext).pop(),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.of(dialogContext).pop(current),
            child: const Text('Save'),
          ),
        ],
      );
    },
  );
}

Future<Set<String>?> showCollectionPickerDialog({
  required BuildContext context,
  required List<KnowledgeCollection> collections,
  required Set<String> selected,
}) {
  return showDialog<Set<String>>(
    context: context,
    builder: (dialogContext) {
      final current = Set<String>.from(selected);
      return AlertDialog(
        title: const Text('Select collections'),
        content: SizedBox(
          width: double.maxFinite,
          child: ListView.builder(
            shrinkWrap: true,
            itemCount: collections.length,
            itemBuilder: (context, index) {
              final collection = collections[index];
              final isSelected = current.contains(collection.id);
              return CheckboxListTile(
                value: isSelected,
                title: Text(collection.name),
                onChanged: (value) {
                  if (value == true) {
                    current.add(collection.id);
                  } else {
                    current.remove(collection.id);
                  }
                  (dialogContext as Element).markNeedsBuild();
                },
              );
            },
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(dialogContext).pop(),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.of(dialogContext).pop(current),
            child: const Text('Save'),
          ),
        ],
      );
    },
  );
}

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:provider/provider.dart';
import '../models/invitation.dart';
import '../services/api_service.dart';
import '../services/invitation_service.dart';

class InvitationsScreen extends StatefulWidget {
  const InvitationsScreen({super.key});

  @override
  State<InvitationsScreen> createState() => _InvitationsScreenState();
}

class _InvitationsScreenState extends State<InvitationsScreen> {
  InvitationService? _service;
  List<Invitation> _invitations = [];
  bool _isLoading = true;
  String? _error;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (_service == null) {
      _service = InvitationService(context.read<ApiService>());
      _load();
    }
  }

  Future<void> _load() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });
    try {
      final invitations = await _service!.getMyInvitations();
      setState(() {
        _invitations = invitations;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  Future<void> _openCreateDialog() async {
    final realNameCtrl = TextEditingController();
    final jurisdictionCtrl = TextEditingController();
    final emailCtrl = TextEditingController();
    final phoneCtrl = TextEditingController();
    bool isInLegalAge = true;
    DateTime? expiresAt;

    final created = await showDialog<Invitation>(
      context: context,
      builder: (ctx) {
        return StatefulBuilder(
          builder: (ctx, setDialogState) {
            return AlertDialog(
              title: const Text('New Invitation'),
              content: SingleChildScrollView(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    TextField(
                      controller: realNameCtrl,
                      decoration: const InputDecoration(
                        labelText: 'Real Name *',
                        hintText: 'Full legal name of the person',
                      ),
                    ),
                    const SizedBox(height: 12),
                    TextField(
                      controller: jurisdictionCtrl,
                      decoration: const InputDecoration(
                        labelText: 'Jurisdiction *',
                        hintText: 'Country or region',
                      ),
                    ),
                    const SizedBox(height: 12),
                    TextField(
                      controller: emailCtrl,
                      decoration: const InputDecoration(
                        labelText: 'Email',
                      ),
                      keyboardType: TextInputType.emailAddress,
                    ),
                    const SizedBox(height: 12),
                    TextField(
                      controller: phoneCtrl,
                      decoration: const InputDecoration(
                        labelText: 'Phone',
                      ),
                      keyboardType: TextInputType.phone,
                    ),
                    const SizedBox(height: 8),
                    CheckboxListTile(
                      contentPadding: EdgeInsets.zero,
                      title: const Text('Is of legal age'),
                      value: isInLegalAge,
                      onChanged: (v) =>
                          setDialogState(() => isInLegalAge = v ?? true),
                    ),
                    ListTile(
                      contentPadding: EdgeInsets.zero,
                      title: const Text('Expires at'),
                      subtitle: Text(
                        expiresAt == null
                            ? 'Never'
                            : '${expiresAt!.year}-${expiresAt!.month.toString().padLeft(2, '0')}-${expiresAt!.day.toString().padLeft(2, '0')}',
                      ),
                      trailing: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          IconButton(
                            icon: const Icon(Icons.calendar_today),
                            onPressed: () async {
                              final picked = await showDatePicker(
                                context: ctx,
                                initialDate: DateTime.now()
                                    .add(const Duration(days: 30)),
                                firstDate: DateTime.now(),
                                lastDate: DateTime.now()
                                    .add(const Duration(days: 365)),
                              );
                              if (picked != null) {
                                setDialogState(() => expiresAt = picked);
                              }
                            },
                          ),
                          if (expiresAt != null)
                            IconButton(
                              icon: const Icon(Icons.clear),
                              onPressed: () =>
                                  setDialogState(() => expiresAt = null),
                            ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(ctx),
                  child: const Text('Cancel'),
                ),
                FilledButton(
                  onPressed: () async {
                    if (realNameCtrl.text.trim().isEmpty ||
                        jurisdictionCtrl.text.trim().isEmpty) {
                      ScaffoldMessenger.of(ctx).showSnackBar(
                        const SnackBar(
                            content: Text(
                                'Real Name and Jurisdiction are required')),
                      );
                      return;
                    }
                    try {
                      final inv = await _service!.createInvitation(
                        realName: realNameCtrl.text.trim(),
                        isInLegalAge: isInLegalAge,
                        jurisdiction: jurisdictionCtrl.text.trim(),
                        email: emailCtrl.text.trim().isEmpty
                            ? null
                            : emailCtrl.text.trim(),
                        phone: phoneCtrl.text.trim().isEmpty
                            ? null
                            : phoneCtrl.text.trim(),
                        expiresAt: expiresAt,
                      );
                      if (ctx.mounted) Navigator.pop(ctx, inv);
                    } catch (e) {
                      if (ctx.mounted) {
                        ScaffoldMessenger.of(ctx).showSnackBar(
                          SnackBar(content: Text('Error: $e')),
                        );
                      }
                    }
                  },
                  child: const Text('Create'),
                ),
              ],
            );
          },
        );
      },
    );

    if (created != null) {
      setState(() => _invitations.insert(0, created));
      if (mounted) {
        _showInviteCodeDialog(created);
      }
    }
  }

  void _showInviteCodeDialog(Invitation inv) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Invitation Created'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('Share this code with ${inv.realName}:'),
            const SizedBox(height: 12),
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
              decoration: BoxDecoration(
                color: Theme.of(ctx).colorScheme.surfaceContainerHighest,
                borderRadius: BorderRadius.circular(8),
              ),
              child: Row(
                children: [
                  Expanded(
                    child: Text(
                      inv.inviteCode,
                      style: Theme.of(ctx).textTheme.titleLarge?.copyWith(
                            fontFamily: 'monospace',
                            letterSpacing: 2,
                          ),
                    ),
                  ),
                  IconButton(
                    icon: const Icon(Icons.copy),
                    tooltip: 'Copy code',
                    onPressed: () {
                      Clipboard.setData(ClipboardData(text: inv.inviteCode));
                      ScaffoldMessenger.of(ctx).showSnackBar(
                        const SnackBar(content: Text('Code copied')),
                      );
                    },
                  ),
                ],
              ),
            ),
          ],
        ),
        actions: [
          FilledButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('Done'),
          ),
        ],
      ),
    );
  }

  void _copyCode(String code) {
    Clipboard.setData(ClipboardData(text: code));
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(content: Text('Invite code copied')),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Invitations'),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: _load,
          ),
        ],
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _openCreateDialog,
        tooltip: 'New Invitation',
        child: const Icon(Icons.person_add_outlined),
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
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(_error!, style: const TextStyle(color: Colors.red)),
            const SizedBox(height: 8),
            ElevatedButton(onPressed: _load, child: const Text('Retry')),
          ],
        ),
      );
    }
    if (_invitations.isEmpty) {
      return const Center(
        child: Text('No invitations yet. Tap + to invite someone.'),
      );
    }

    return ListView.builder(
      itemCount: _invitations.length,
      itemBuilder: (context, index) {
        final inv = _invitations[index];
        return _InvitationTile(
          invitation: inv,
          onCopyCode: () => _copyCode(inv.inviteCode),
        );
      },
    );
  }
}

class _InvitationTile extends StatelessWidget {
  final Invitation invitation;
  final VoidCallback onCopyCode;

  const _InvitationTile({
    required this.invitation,
    required this.onCopyCode,
  });

  @override
  Widget build(BuildContext context) {
    final colorScheme = Theme.of(context).colorScheme;

    Color statusColor;
    String statusLabel;
    IconData statusIcon;

    if (invitation.isAccepted) {
      statusColor = Colors.green;
      statusLabel = 'Accepted';
      statusIcon = Icons.check_circle_outline;
    } else if (invitation.isExpired) {
      statusColor = colorScheme.error;
      statusLabel = 'Expired';
      statusIcon = Icons.cancel_outlined;
    } else {
      statusColor = colorScheme.primary;
      statusLabel = 'Pending';
      statusIcon = Icons.hourglass_empty_outlined;
    }

    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
      child: ListTile(
        leading: Icon(statusIcon, color: statusColor),
        title: Text(invitation.realName),
        subtitle: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(invitation.jurisdiction),
            if (invitation.email != null) Text(invitation.email!),
            const SizedBox(height: 4),
            Row(
              children: [
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                  decoration: BoxDecoration(
                    color: statusColor.withValues(alpha: 0.12),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Text(
                    statusLabel,
                    style: TextStyle(color: statusColor, fontSize: 12),
                  ),
                ),
                if (invitation.expiresAt != null) ...[
                  const SizedBox(width: 8),
                  Text(
                    'Expires ${_formatDate(invitation.expiresAt!)}',
                    style: Theme.of(context).textTheme.bodySmall,
                  ),
                ],
              ],
            ),
          ],
        ),
        isThreeLine: true,
        trailing: invitation.isPending
            ? IconButton(
                icon: const Icon(Icons.copy_outlined),
                tooltip: 'Copy invite code',
                onPressed: onCopyCode,
              )
            : null,
      ),
    );
  }

  String _formatDate(DateTime dt) {
    return '${dt.year}-${dt.month.toString().padLeft(2, '0')}-${dt.day.toString().padLeft(2, '0')}';
  }
}

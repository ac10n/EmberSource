import 'package:flutter/material.dart';

enum AppSection {
  explore,
  contents,
  invite,
  invitationList,
  profile,
}

class AppDrawer extends StatelessWidget {
  final AppSection selectedSection;
  final ValueChanged<AppSection> onSectionSelected;
  final VoidCallback onLogout;

  const AppDrawer({
    super.key,
    required this.selectedSection,
    required this.onSectionSelected,
    required this.onLogout,
  });

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: SafeArea(
        child: Column(
          children: [
            DrawerHeader(
              margin: EdgeInsets.zero,
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    Theme.of(context).colorScheme.primary,
                    Theme.of(context).colorScheme.primaryContainer,
                  ],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
              ),
              child: SizedBox(
                width: double.infinity,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    Text(
                      'Ember Knowledge',
                      style:
                          Theme.of(context).textTheme.headlineSmall?.copyWith(
                                color: Theme.of(context).colorScheme.onPrimary,
                                fontWeight: FontWeight.w700,
                              ),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      'Navigate your knowledge and profile',
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                            color: Theme.of(context).colorScheme.onPrimary,
                          ),
                    ),
                  ],
                ),
              ),
            ),
            Expanded(
              child: ListView(
                children: [
                  _buildTile(
                    context,
                    section: AppSection.explore,
                    icon: Icons.explore_outlined,
                    title: 'Explore',
                    subtitle: 'Show contents',
                  ),
                  _buildTile(
                    context,
                    section: AppSection.contents,
                    icon: Icons.menu_book_outlined,
                    title: 'My Knowledge',
                    subtitle: 'Your knowledge list',
                  ),
                  _buildTile(
                    context,
                    section: AppSection.invite,
                    icon: Icons.person_add_outlined,
                    title: 'Invite',
                    subtitle: 'Enter email and invite someone',
                  ),
                  _buildTile(
                    context,
                    section: AppSection.invitationList,
                    icon: Icons.receipt_long_outlined,
                    title: 'Invitation List',
                    subtitle: 'View pending and accepted invites',
                  ),
                  _buildTile(
                    context,
                    section: AppSection.profile,
                    icon: Icons.account_circle_outlined,
                    title: 'Profile',
                    subtitle: 'Information and password',
                  ),
                  const Divider(height: 32),
                  ListTile(
                    leading: const Icon(Icons.logout),
                    title: const Text('Logout'),
                    onTap: onLogout,
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTile(
    BuildContext context, {
    required AppSection section,
    required IconData icon,
    required String title,
    required String subtitle,
  }) {
    final isSelected = selectedSection == section;
    final colorScheme = Theme.of(context).colorScheme;

    return ListTile(
      selected: isSelected,
      selectedTileColor: colorScheme.primaryContainer.withOpacity(0.5),
      leading: Icon(icon),
      title: Text(title),
      subtitle: Text(subtitle),
      onTap: () => onSectionSelected(section),
    );
  }
}

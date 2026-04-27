import 'package:flutter/material.dart';
import 'collections_screen.dart';
import 'content_list_screen.dart';
import 'tags_screen.dart';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  State<MainScreen> createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  int _currentIndex = 0;

  final List<Widget> _screens = const [
    ContentListScreen(),
    TagsScreen(),
    CollectionsScreen(),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _screens[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.article_outlined),
            label: 'Content',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.label_outline),
            label: 'Tags',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.collections_bookmark_outlined),
            label: 'Collections',
          ),
        ],
      ),
    );
  }
}

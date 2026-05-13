class ProfileInfo {
  final String username;
  final String fullName;
  final int birthYear;
  final String jurisdiction;

  const ProfileInfo({
    required this.username,
    required this.fullName,
    required this.birthYear,
    required this.jurisdiction,
  });

  factory ProfileInfo.fromJson(Map<String, dynamic> json) {
    return ProfileInfo(
      username: json['username']?.toString() ?? '',
      fullName: json['fullName']?.toString() ?? '',
      birthYear: int.tryParse(json['birthYear']?.toString() ?? '') ?? 0,
      jurisdiction: json['jurisdiction']?.toString() ?? '',
    );
  }
}

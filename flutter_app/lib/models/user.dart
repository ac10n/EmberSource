import 'package:json_annotation/json_annotation.dart';

part 'user.g.dart';

@JsonSerializable()
class User {
  final int id;
  final String email;
  final String username;
  final String? profileImageUrl;
  final DateTime createdAt;

  User({
    required this.id,
    required this.email,
    required this.username,
    this.profileImageUrl,
    required this.createdAt,
  });

  factory User.fromJson(Map<String, dynamic> json) => _$UserFromJson(json);
  Map<String, dynamic> toJson() => _$UserToJson(this);
}

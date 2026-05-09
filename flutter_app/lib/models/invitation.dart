class Invitation {
  final String id;
  final String inviteCode;
  final String realName;
  final bool isInLegalAge;
  final String jurisdiction;
  final String? email;
  final String? phone;
  final DateTime createdAt;
  final DateTime? expiresAt;
  final DateTime? acceptedAt;
  final String? acceptedByUserId;

  const Invitation({
    required this.id,
    required this.inviteCode,
    required this.realName,
    required this.isInLegalAge,
    required this.jurisdiction,
    this.email,
    this.phone,
    required this.createdAt,
    this.expiresAt,
    this.acceptedAt,
    this.acceptedByUserId,
  });

  bool get isAccepted => acceptedAt != null;
  bool get isExpired =>
      expiresAt != null && expiresAt!.isBefore(DateTime.now().toUtc());
  bool get isPending => !isAccepted && !isExpired;

  factory Invitation.fromJson(Map<String, dynamic> json) {
    return Invitation(
      id: json['id'] as String,
      inviteCode: json['inviteCode'] as String,
      realName: json['realName'] as String,
      isInLegalAge: json['isInLegalAge'] as bool,
      jurisdiction: json['jurisdiction'] as String,
      email: json['email'] as String?,
      phone: json['phone'] as String?,
      createdAt: DateTime.parse(json['createdAt'] as String),
      expiresAt: json['expiresAt'] != null
          ? DateTime.parse(json['expiresAt'] as String)
          : null,
      acceptedAt: json['acceptedAt'] != null
          ? DateTime.parse(json['acceptedAt'] as String)
          : null,
      acceptedByUserId: json['acceptedByUserId'] as String?,
    );
  }
}

class KnowledgeTag {
  final String id;
  final String name;
  final bool isPrivate;
  final bool hasConfidenceRate;
  final int? confidenceRate;

  KnowledgeTag({
    required this.id,
    required this.name,
    required this.isPrivate,
    required this.hasConfidenceRate,
    this.confidenceRate,
  });

  factory KnowledgeTag.fromJson(Map<String, dynamic> json) {
    return KnowledgeTag(
      id: json['id']?.toString() ?? '',
      name: json['name']?.toString() ?? '',
      isPrivate: json['isPrivate'] == true,
      hasConfidenceRate: json['hasConfidenceRate'] == true,
      confidenceRate: json['confidenceRate'] is int
          ? json['confidenceRate'] as int
          : json['confidenceRate'] is num
              ? (json['confidenceRate'] as num).toInt()
              : null,
    );
  }

  Map<String, dynamic> toCreateJson() {
    return {
      'name': name,
      'isPrivate': isPrivate,
      'hasConfidenceRate': hasConfidenceRate,
    };
  }

  Map<String, dynamic> toUpdateJson() {
    return {
      'name': name,
      'isPrivate': isPrivate,
      'hasConfidenceRate': hasConfidenceRate,
    };
  }

  Map<String, dynamic> toContentTagJson() {
    return {
      'id': id,
      'name': name,
      'isPrivate': isPrivate,
      'hasConfidenceRate': hasConfidenceRate,
      if (confidenceRate != null) 'confidenceRate': confidenceRate,
    };
  }
}

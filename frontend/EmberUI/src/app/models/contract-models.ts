export type ListRequestBase =  {
  maxCount: number;
  continuationToken?: string;
  sorting?: string[];
}

export type ListResponseBase =  {
  continuationToken?: string;
}

export type TokenResponse =  {
  accessToken: string;
  accessTokenExpiresAt: string;
  refreshToken: string;
  refreshTokenExpiresAt: string;
}

export type LoginRequest =  {
  userName: string;
  password: string;
  rememberMe: boolean;
}

export type RefreshRequest =  {
  refreshToken: string;
}

export type ProfileResponse =  {
  username: string;
  fullName: string;
  birthYear: number;
  jurisdiction: string;
}

export type ProfileRequest =  {
  profileId?: string;
}

export type UpdateProfileRequest =  {
  fullName: string;
  birthYear: number;
  jurisdiction: string;
  oldPassword?: string;
  newPassword?: string;
}

export type KnowledgeRequestModel =  ListRequestBase & {
  contentIds?: string[];
  parentContentIds?: (string | null | undefined)[];
  contentTypes?: ContentTypes[];
  titleSearchTerm?: string;
  dataSearchTerm?: string;
  createdByUserIds?: string[];
  createdAfter?: string | null | undefined;
  unreadByMe?: boolean | null | undefined;
  relationshipFilters?: RelationshipFilters[];
  collections?: string[];
  maxCount: number;
  continuationToken?: string;
  sorting?: string[];
}

export enum ContentTypes {
  Paragraph = 1,
  Section = 2,
  Article = 3,
  Image = 4,
  Video = 5,
  Audio = 6,
  Link = 7,
  InteractiveElement = 8,
  CodeSnippet = 9,
  ExplorableDataset = 10,
  Comment = 11,
}

export type RelationshipFilters =  {
  relatedContentTypeId: RelatedContentTypes;
  relatedContentId: string;
}

export enum RelatedContentTypes {
  Reference = 1,
  SupplementaryMaterial = 2,
  Citation = 3,
  FurtherReading = 4,
  RelatedTopic = 5,
  Alternative = 6,
  Contradicting = 7,
}

export type KnowledgeResponseModel =  ListResponseBase & {
  contents?: ContentModel[];
  continuationToken?: string;
}

export type ContentModel =  {
  id: string;
  parentContentId?: string | null | undefined;
  contentTypeId: ContentTypes;
  title?: string;
  data?: string;
  createdByUserId: string;
  createdAt: string;
}


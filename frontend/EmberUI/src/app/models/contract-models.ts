export type ListRequestBase =  {
  maxCount: number;
  continuationToken?: string;
  sorting?: string[];
}

export type ListResponseBase =  {
  continuationToken?: string;
}

export type UpdateResultKind = 'failure' | 'success';

export type FailureReason = 'logical' | 'permission' | 'transient' | 'unknown';

export type UpdateResult<TModel> =  {
  result: UpdateResultKind;
  data?: TModel;
  reason?: FailureReason | null | undefined;
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
}

export type ChangePasswordRequest =  {
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

export type ContentTypes = 'article' | 'audio' | 'codeSnippet' | 'comment' | 'explorableDataset' | 'image' | 'interactiveElement' | 'link' | 'paragraph' | 'section' | 'video';

export type RelationshipFilters =  {
  relatedContentTypeId: RelatedContentTypes;
  relatedContentId: string;
}

export type RelatedContentTypes = 'alternative' | 'citation' | 'contradicting' | 'furtherReading' | 'reference' | 'relatedTopic' | 'supplementaryMaterial';

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


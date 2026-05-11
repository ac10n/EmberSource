using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ember.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LOG");

            migrationBuilder.EnsureSchema(
                name: "SSO");

            migrationBuilder.EnsureSchema(
                name: "USR");

            migrationBuilder.EnsureSchema(
                name: "DOC");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "SSO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "SSO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BirthYear = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Jurisdiction = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BadgeDefinitions",
                schema: "USR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsNumeric = table.Column<bool>(type: "boolean", nullable: false),
                    IsFractional = table.Column<bool>(type: "boolean", nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: false),
                    DefaultValue = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentFormats",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypes",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentVisibilities",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentVisibilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataOwnerships",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataOwnerships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialModels",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "SSO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReplacedByTokenId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatedContentTypes",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "SSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SSO",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "SSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "SSO",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "SSO",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_AspNetUsers_EmberUserId",
                        column: x => x.EmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteractionLogs",
                schema: "LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelativeUrl = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InteractionType = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    Owner = table.Column<int>(type: "integer", nullable: false),
                    PurgingTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                schema: "USR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RealName = table.Column<string>(type: "text", nullable: false),
                    IsInLegalAge = table.Column<bool>(type: "boolean", nullable: false),
                    Jurisdiction = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    InviteCode = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AcceptedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_AcceptedByUserId",
                        column: x => x.AcceptedByUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                schema: "LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    HttpMethod = table.Column<string>(type: "text", nullable: false),
                    RelativeUrl = table.Column<string>(type: "text", nullable: false),
                    QueryString = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TotalRequestSize = table.Column<long>(type: "bigint", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: true),
                    BaseUri = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmberUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    HasConfidenceRate = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_EmberUserId",
                        column: x => x.EmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ByEmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForEmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovesBooleanBadge = table.Column<bool>(type: "boolean", nullable: true),
                    NumericBadgeValue = table.Column<decimal>(type: "numeric", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    FromTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ToTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_AspNetUsers_ByEmberUserId",
                        column: x => x.ByEmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testimonials_AspNetUsers_ForEmberUserId",
                        column: x => x.ForEmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testimonials_BadgeDefinitions_BadgeDefinitionId",
                        column: x => x.BadgeDefinitionId,
                        principalSchema: "USR",
                        principalTable: "BadgeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBadgeValues",
                schema: "USR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: true),
                    FromTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ToTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadgeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBadgeValues_AspNetUsers_EmberUserId",
                        column: x => x.EmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBadgeValues_BadgeDefinitions_BadgeDefinitionId",
                        column: x => x.BadgeDefinitionId,
                        principalSchema: "USR",
                        principalTable: "BadgeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    ParentContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContentTypeId = table.Column<int>(type: "integer", nullable: false),
                    ContentFormatId = table.Column<int>(type: "integer", nullable: false),
                    FormatVersion = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "text", nullable: false),
                    EmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RemovedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ContentVisibilityId = table.Column<int>(type: "integer", nullable: false),
                    VisibilityCriteria = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_AspNetUsers_EmberUserId",
                        column: x => x.EmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentFormats_ContentFormatId",
                        column: x => x.ContentFormatId,
                        principalSchema: "DOC",
                        principalTable: "ContentFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalSchema: "DOC",
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentVisibilities_ContentVisibilityId",
                        column: x => x.ContentVisibilityId,
                        principalSchema: "DOC",
                        principalTable: "ContentVisibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_Contents_ParentContentId",
                        column: x => x.ParentContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformSections",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentSectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InheritRoles = table.Column<bool>(type: "boolean", nullable: false),
                    FinancialModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformSections_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformSections_FinancialModels_FinancialModelId",
                        column: x => x.FinancialModelId,
                        principalSchema: "DOC",
                        principalTable: "FinancialModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformSections_PlatformSections_ParentSectionId",
                        column: x => x.ParentSectionId,
                        principalSchema: "DOC",
                        principalTable: "PlatformSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionLogs",
                schema: "LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    OldValues = table.Column<string>(type: "text", nullable: true),
                    NewValues = table.Column<string>(type: "text", nullable: true),
                    PurgingTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Owner = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionLogs_RequestLogs_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "LOG",
                        principalTable: "RequestLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProcessLogs",
                schema: "LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    CodePath = table.Column<string>(type: "text", nullable: false),
                    Parameters = table.Column<string>(type: "text", nullable: true),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    PurgingTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessLogs_RequestLogs_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "LOG",
                        principalTable: "RequestLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResponseLogs",
                schema: "LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ProcessingTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TotalResponseSize = table.Column<long>(type: "bigint", nullable: false),
                    PurgingTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseLogs_RequestLogs_RequestLogId",
                        column: x => x.RequestLogId,
                        principalSchema: "LOG",
                        principalTable: "RequestLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionItems",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionItems_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalSchema: "DOC",
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionItems_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentInteractions",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsLiked = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisliked = table.Column<bool>(type: "boolean", nullable: false),
                    Recommend = table.Column<bool>(type: "boolean", nullable: false),
                    RemindLaterList = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentInteractions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentInteractions_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmberUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfidenceRate = table.Column<byte>(type: "smallint", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTags_AspNetUsers_EmberUserId",
                        column: x => x.EmberUserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTags_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalSchema: "DOC",
                        principalTable: "Collections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "DOC",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedContents",
                schema: "DOC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedContentTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedContents_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedContents_Contents_RelatedContentId",
                        column: x => x.RelatedContentId,
                        principalSchema: "DOC",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedContents_RelatedContentTypes_RelatedContentTypeId",
                        column: x => x.RelatedContentTypeId,
                        principalSchema: "DOC",
                        principalTable: "RelatedContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "SSO",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformSectionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SSO",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SSO",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_PlatformSections_PlatformSectionId",
                        column: x => x.PlatformSectionId,
                        principalSchema: "DOC",
                        principalTable: "PlatformSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_RequestId",
                schema: "LOG",
                table: "ActionLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_UserId",
                schema: "LOG",
                table: "ActionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "SSO",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "SSO",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "SSO",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "SSO",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_PlatformSectionId",
                schema: "SSO",
                table: "AspNetUserRoles",
                column: "PlatformSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "SSO",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "SSO",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "SSO",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_CollectionId",
                schema: "DOC",
                table: "CollectionItems",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_ContentId",
                schema: "DOC",
                table: "CollectionItems",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_EmberUserId",
                schema: "DOC",
                table: "Collections",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentInteractions_ContentId",
                schema: "DOC",
                table: "ContentInteractions",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentInteractions_UserId",
                schema: "DOC",
                table: "ContentInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentFormatId",
                schema: "DOC",
                table: "Contents",
                column: "ContentFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentTypeId",
                schema: "DOC",
                table: "Contents",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentVisibilityId",
                schema: "DOC",
                table: "Contents",
                column: "ContentVisibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_EmberUserId",
                schema: "DOC",
                table: "Contents",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ParentContentId",
                schema: "DOC",
                table: "Contents",
                column: "ParentContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_CollectionId",
                schema: "DOC",
                table: "ContentTags",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId",
                schema: "DOC",
                table: "ContentTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_EmberUserId",
                schema: "DOC",
                table: "ContentTags",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_TagId",
                schema: "DOC",
                table: "ContentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionLogs_UserId",
                schema: "LOG",
                table: "InteractionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_AcceptedByUserId",
                schema: "USR",
                table: "Invitations",
                column: "AcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InviteCode",
                schema: "USR",
                table: "Invitations",
                column: "InviteCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedByUserId",
                schema: "USR",
                table: "Invitations",
                column: "InvitedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_CreatorUserId",
                schema: "DOC",
                table: "PlatformSections",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_FinancialModelId",
                schema: "DOC",
                table: "PlatformSections",
                column: "FinancialModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_ParentSectionId",
                schema: "DOC",
                table: "PlatformSections",
                column: "ParentSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessLogs_RequestId",
                schema: "LOG",
                table: "ProcessLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_ContentId",
                schema: "DOC",
                table: "RelatedContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_RelatedContentId",
                schema: "DOC",
                table: "RelatedContents",
                column: "RelatedContentId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_RelatedContentTypeId",
                schema: "DOC",
                table: "RelatedContents",
                column: "RelatedContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_UserId",
                schema: "LOG",
                table: "RequestLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseLogs_RequestLogId",
                schema: "LOG",
                table: "ResponseLogs",
                column: "RequestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_EmberUserId",
                schema: "DOC",
                table: "Tags",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_BadgeDefinitionId",
                schema: "DOC",
                table: "Testimonials",
                column: "BadgeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ByEmberUserId",
                schema: "DOC",
                table: "Testimonials",
                column: "ByEmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ForEmberUserId",
                schema: "DOC",
                table: "Testimonials",
                column: "ForEmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadgeValues_BadgeDefinitionId",
                schema: "USR",
                table: "UserBadgeValues",
                column: "BadgeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadgeValues_EmberUserId",
                schema: "USR",
                table: "UserBadgeValues",
                column: "EmberUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs",
                schema: "LOG");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "CollectionItems",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ContentInteractions",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ContentTags",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "DataOwnerships",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "InteractionLogs",
                schema: "LOG");

            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "USR");

            migrationBuilder.DropTable(
                name: "ProcessLogs",
                schema: "LOG");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "RelatedContents",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ResponseLogs",
                schema: "LOG");

            migrationBuilder.DropTable(
                name: "Testimonials",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "UserBadgeValues",
                schema: "USR");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "SSO");

            migrationBuilder.DropTable(
                name: "PlatformSections",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "Collections",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "Contents",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "RelatedContentTypes",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "RequestLogs",
                schema: "LOG");

            migrationBuilder.DropTable(
                name: "BadgeDefinitions",
                schema: "USR");

            migrationBuilder.DropTable(
                name: "FinancialModels",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ContentFormats",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ContentTypes",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "ContentVisibilities",
                schema: "DOC");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "SSO");
        }
    }
}

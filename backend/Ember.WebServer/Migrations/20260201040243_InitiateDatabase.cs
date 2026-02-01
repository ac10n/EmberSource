using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ember.WebServer.Migrations
{
    /// <inheritdoc />
    public partial class InitiateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
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
                name: "ContentFormat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFormat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypes",
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
                name: "FinancialModels",
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
                name: "LogOwnerships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogOwnerships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatedContentTypes",
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
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteractionLogs",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForUserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_Testimonials_AspNetUsers_ByUserId",
                        column: x => x.ByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testimonials_AspNetUsers_ForUserId",
                        column: x => x.ForUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Testimonials_BadgeDefinitions_BadgeDefinitionId",
                        column: x => x.BadgeDefinitionId,
                        principalTable: "BadgeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBadgeValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_UserBadgeValues_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBadgeValues_BadgeDefinitions_BadgeDefinitionId",
                        column: x => x.BadgeDefinitionId,
                        principalTable: "BadgeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
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
                    Data = table.Column<string>(type: "text", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RemovedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ContentVisibilityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentFormat_ContentFormatId",
                        column: x => x.ContentFormatId,
                        principalTable: "ContentFormat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_ContentVisibilities_ContentVisibilityId",
                        column: x => x.ContentVisibilityId,
                        principalTable: "ContentVisibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_Contents_ParentContentId",
                        column: x => x.ParentContentId,
                        principalTable: "Contents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlatformSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentSectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentPlatformSectionId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformSections_FinancialModels_FinancialModelId",
                        column: x => x.FinancialModelId,
                        principalTable: "FinancialModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatformSections_PlatformSections_ParentPlatformSectionId",
                        column: x => x.ParentPlatformSectionId,
                        principalTable: "PlatformSections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionLogs",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionLogs_RequestLogs_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProcessLogs",
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
                        principalTable: "RequestLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResponseLogs",
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
                        principalTable: "RequestLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionItems_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionItems_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentInteractions",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentInteractions_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedContents",
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
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedContents_Contents_RelatedContentId",
                        column: x => x.RelatedContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedContents_RelatedContentTypes_RelatedContentTypeId",
                        column: x => x.RelatedContentTypeId,
                        principalTable: "RelatedContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_PlatformSections_PlatformSectionId",
                        column: x => x.PlatformSectionId,
                        principalTable: "PlatformSections",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5e4b79a1-afe1-4e79-a4e1-ee1b86e4ce33"), null, "Owner", "OWNER" },
                    { new Guid("5e848956-8a01-4816-be8d-d6a6b7091cf3"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("a03c7114-423e-47e1-a280-6ba824c25828"), null, "User", "USER" },
                    { new Guid("b373f8c6-b1b9-4097-98e1-46a881e5484d"), null, "Creator", "CREATOR" },
                    { new Guid("f883569f-2494-4328-95a9-e76a2b6ff143"), null, "Root", "Root" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthYear", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Jurisdiction", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("2f673ad9-6913-46d0-9b44-d517c5611c8c"), 0, 0, "2aaf7da4-ce97-4efb-a069-55ff3c131465", null, true, "Alireza Haghshenas", "Canada", false, null, null, "ALI", null, null, true, null, false, "ali" },
                    { new Guid("6c0a35b2-23c4-44a3-8e13-161285b0f9db"), 0, 0, "a5405995-3314-4821-be2f-17ddabd54fbf", null, true, "System User", "Canada", false, null, null, "SYSTEM", null, null, true, null, false, "System" }
                });

            migrationBuilder.InsertData(
                table: "ContentFormat",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Markdown" });

            migrationBuilder.InsertData(
                table: "ContentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Paragraph" },
                    { 2, "Section" },
                    { 3, "Article" },
                    { 4, "Image" },
                    { 5, "Video" },
                    { 6, "Audio" },
                    { 7, "Link" },
                    { 8, "InteractiveElement" },
                    { 9, "CodeSnippet" },
                    { 10, "ExplorableDataset" },
                    { 11, "Comment" }
                });

            migrationBuilder.InsertData(
                table: "ContentVisibilities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Public" },
                    { 2, "Private" },
                    { 3, "VisibleToSpecificPeople" },
                    { 4, "VisibleByCriteria" },
                    { 5, "ScheduledRollout" }
                });

            migrationBuilder.InsertData(
                table: "FinancialModels",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "A financial model where the organization does not distribute profits to owners or shareholders.", "Non-Profit" },
                    { 2, "A financial model where the organization aims to generate profit for its owners or shareholders.", "For-Profit" }
                });

            migrationBuilder.InsertData(
                table: "LogOwnerships",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "System" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId", "Id", "PlatformSectionId" },
                values: new object[,]
                {
                    { new Guid("5e4b79a1-afe1-4e79-a4e1-ee1b86e4ce33"), new Guid("2f673ad9-6913-46d0-9b44-d517c5611c8c"), new Guid("072c2bb6-f216-4c07-8e4c-f6b3abb05770"), null },
                    { new Guid("f883569f-2494-4328-95a9-e76a2b6ff143"), new Guid("6c0a35b2-23c4-44a3-8e13-161285b0f9db"), new Guid("43510e33-41e3-45bb-96c3-be52af6b9c3b"), null }
                });

            migrationBuilder.InsertData(
                table: "Contents",
                columns: new[] { "Id", "ContentFormatId", "ContentTypeId", "ContentVisibilityId", "CreatedAt", "CreatedByUserId", "Data", "FormatVersion", "Identifier", "IsActive", "ParentContentId", "RemovedTime", "Title", "Version" },
                values: new object[] { new Guid("3173c43b-b415-4d45-a002-7c7d7253bf1b"), 1, 1, 1, new DateTimeOffset(new DateTime(2026, 1, 31, 14, 44, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), new Guid("2f673ad9-6913-46d0-9b44-d517c5611c8c"), "# Welcome to Ember!\n\nEmber is an attempt to give users ownership of their online activities.\nAn online service that does not seek profit, would give users tools they need to find the data they want,\ninteract with the platform in ways that make sense to their use cases,\nkeep the user in control, not at the mercy of intentionally limited UX and engagement-maximizing algorithms.\n\nTransparency builds dependable trust, amongst users, and between users and the platform.\nWith time, we can create tools that provide users with a measure of trustworthiness of content and its creators.\nThese measures should reflect each user's values and priorities, not those imposed by a centralized authority.\n\nThe premise is that a confused user is more likely to make decisions that are not in their best interest.\n\nWe'll start with the simplest tools: a place you can write and share pieces of content.\nWith that, conversations around Ember development can be hosted on the platform itself.\n\nEmber is an attempt to bring back how trust shaped human interactions in small communities,\nbut we lost that when we moved to the global village, and platforms that seek profit at the expense societal good.\n\nMuch more details to be shared as we build more features.\n", 1, new Guid("3fee5a19-69e4-4c65-81fe-8d9de9c11539"), true, null, null, "Welcome to Ember", 1 });

            migrationBuilder.InsertData(
                table: "PlatformSections",
                columns: new[] { "Id", "CreatorUserId", "Description", "FinancialModelId", "InheritRoles", "Name", "ParentPlatformSectionId", "ParentSectionId", "Url" },
                values: new object[] { new Guid("d04c1e89-a4c8-450b-8608-84f35415fd39"), new Guid("2f673ad9-6913-46d0-9b44-d517c5611c8c"), "The Ember Foundation, the non-profit organization that drives all other Ember projects.", 1, false, "Ember Foundation", null, null, "/" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_RequestId",
                table: "ActionLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_UserId",
                table: "ActionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_PlatformSectionId",
                table: "AspNetUserRoles",
                column: "PlatformSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_CollectionId",
                table: "CollectionItems",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_ContentId",
                table: "CollectionItems",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_CreatedByUserId",
                table: "Collections",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentInteractions_ContentId",
                table: "ContentInteractions",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentInteractions_UserId",
                table: "ContentInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentFormatId",
                table: "Contents",
                column: "ContentFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentTypeId",
                table: "Contents",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentVisibilityId",
                table: "Contents",
                column: "ContentVisibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CreatedByUserId",
                table: "Contents",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ParentContentId",
                table: "Contents",
                column: "ParentContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId",
                table: "ContentTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_TagId",
                table: "ContentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionLogs_UserId",
                table: "InteractionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_CreatorUserId",
                table: "PlatformSections",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_FinancialModelId",
                table: "PlatformSections",
                column: "FinancialModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSections_ParentPlatformSectionId",
                table: "PlatformSections",
                column: "ParentPlatformSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessLogs_RequestId",
                table: "ProcessLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_ContentId",
                table: "RelatedContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_RelatedContentId",
                table: "RelatedContents",
                column: "RelatedContentId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedContents_RelatedContentTypeId",
                table: "RelatedContents",
                column: "RelatedContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_UserId",
                table: "RequestLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseLogs_RequestLogId",
                table: "ResponseLogs",
                column: "RequestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_BadgeDefinitionId",
                table: "Testimonials",
                column: "BadgeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ByUserId",
                table: "Testimonials",
                column: "ByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ForUserId",
                table: "Testimonials",
                column: "ForUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadgeValues_BadgeDefinitionId",
                table: "UserBadgeValues",
                column: "BadgeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadgeValues_UserId",
                table: "UserBadgeValues",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CollectionItems");

            migrationBuilder.DropTable(
                name: "ContentInteractions");

            migrationBuilder.DropTable(
                name: "ContentTags");

            migrationBuilder.DropTable(
                name: "InteractionLogs");

            migrationBuilder.DropTable(
                name: "LogOwnerships");

            migrationBuilder.DropTable(
                name: "ProcessLogs");

            migrationBuilder.DropTable(
                name: "RelatedContents");

            migrationBuilder.DropTable(
                name: "ResponseLogs");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "UserBadgeValues");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PlatformSections");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "RelatedContentTypes");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "BadgeDefinitions");

            migrationBuilder.DropTable(
                name: "FinancialModels");

            migrationBuilder.DropTable(
                name: "ContentFormat");

            migrationBuilder.DropTable(
                name: "ContentTypes");

            migrationBuilder.DropTable(
                name: "ContentVisibilities");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

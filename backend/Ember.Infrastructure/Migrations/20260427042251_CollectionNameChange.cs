using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ember.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CollectionNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionItems_Contents_ContentId",
                table: "CollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Collections_AspNetUsers_CreatedByUserId",
                table: "Collections");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_AspNetUsers_CreatedByUserId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_Contents_ContentId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_ByUserId",
                table: "Testimonials");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_ForUserId",
                table: "Testimonials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadgeValues_AspNetUsers_UserId",
                table: "UserBadgeValues");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBadgeValues",
                newName: "EmberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBadgeValues_UserId",
                table: "UserBadgeValues",
                newName: "IX_UserBadgeValues_EmberUserId");

            migrationBuilder.RenameColumn(
                name: "ForUserId",
                table: "Testimonials",
                newName: "ForEmberUserId");

            migrationBuilder.RenameColumn(
                name: "ByUserId",
                table: "Testimonials",
                newName: "ByEmberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Testimonials_ForUserId",
                table: "Testimonials",
                newName: "IX_Testimonials_ForEmberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Testimonials_ByUserId",
                table: "Testimonials",
                newName: "IX_Testimonials_ByEmberUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Contents",
                newName: "EmberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_CreatedByUserId",
                table: "Contents",
                newName: "IX_Contents_EmberUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Collections",
                newName: "EmberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_CreatedByUserId",
                table: "Collections",
                newName: "IX_Collections_EmberUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "EmberUserId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasConfidenceRate",
                table: "Tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "ContentId",
                table: "ContentTags",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CollectionId",
                table: "ContentTags",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ConfidenceRate",
                table: "ContentTags",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmberUserId",
                table: "ContentTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "ContentTags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Contents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisibilityCriteria",
                table: "Contents",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ContentId",
                table: "CollectionItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Invitations",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_EmberUserId",
                table: "Tags",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_CollectionId",
                table: "ContentTags",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_EmberUserId",
                table: "ContentTags",
                column: "EmberUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_AcceptedByUserId",
                table: "Invitations",
                column: "AcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InviteCode",
                table: "Invitations",
                column: "InviteCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedByUserId",
                table: "Invitations",
                column: "InvitedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionItems_Contents_ContentId",
                table: "CollectionItems",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_AspNetUsers_EmberUserId",
                table: "Collections",
                column: "EmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_AspNetUsers_EmberUserId",
                table: "Contents",
                column: "EmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_AspNetUsers_EmberUserId",
                table: "ContentTags",
                column: "EmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_Collections_CollectionId",
                table: "ContentTags",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_Contents_ContentId",
                table: "ContentTags",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_EmberUserId",
                table: "Tags",
                column: "EmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_ByEmberUserId",
                table: "Testimonials",
                column: "ByEmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_ForEmberUserId",
                table: "Testimonials",
                column: "ForEmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadgeValues_AspNetUsers_EmberUserId",
                table: "UserBadgeValues",
                column: "EmberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionItems_Contents_ContentId",
                table: "CollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Collections_AspNetUsers_EmberUserId",
                table: "Collections");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_AspNetUsers_EmberUserId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_AspNetUsers_EmberUserId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_Collections_CollectionId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTags_Contents_ContentId",
                table: "ContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_EmberUserId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_ByEmberUserId",
                table: "Testimonials");

            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_AspNetUsers_ForEmberUserId",
                table: "Testimonials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadgeValues_AspNetUsers_EmberUserId",
                table: "UserBadgeValues");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Tags_EmberUserId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_ContentTags_CollectionId",
                table: "ContentTags");

            migrationBuilder.DropIndex(
                name: "IX_ContentTags_EmberUserId",
                table: "ContentTags");

            migrationBuilder.DropColumn(
                name: "EmberUserId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "HasConfidenceRate",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "ContentTags");

            migrationBuilder.DropColumn(
                name: "ConfidenceRate",
                table: "ContentTags");

            migrationBuilder.DropColumn(
                name: "EmberUserId",
                table: "ContentTags");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "ContentTags");

            migrationBuilder.DropColumn(
                name: "VisibilityCriteria",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "EmberUserId",
                table: "UserBadgeValues",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBadgeValues_EmberUserId",
                table: "UserBadgeValues",
                newName: "IX_UserBadgeValues_UserId");

            migrationBuilder.RenameColumn(
                name: "ForEmberUserId",
                table: "Testimonials",
                newName: "ForUserId");

            migrationBuilder.RenameColumn(
                name: "ByEmberUserId",
                table: "Testimonials",
                newName: "ByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Testimonials_ForEmberUserId",
                table: "Testimonials",
                newName: "IX_Testimonials_ForUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Testimonials_ByEmberUserId",
                table: "Testimonials",
                newName: "IX_Testimonials_ByUserId");

            migrationBuilder.RenameColumn(
                name: "EmberUserId",
                table: "Contents",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_EmberUserId",
                table: "Contents",
                newName: "IX_Contents_CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "EmberUserId",
                table: "Collections",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_EmberUserId",
                table: "Collections",
                newName: "IX_Collections_CreatedByUserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContentId",
                table: "ContentTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Contents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContentId",
                table: "CollectionItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionItems_Contents_ContentId",
                table: "CollectionItems",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_AspNetUsers_CreatedByUserId",
                table: "Collections",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_AspNetUsers_CreatedByUserId",
                table: "Contents",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTags_Contents_ContentId",
                table: "ContentTags",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_ByUserId",
                table: "Testimonials",
                column: "ByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_AspNetUsers_ForUserId",
                table: "Testimonials",
                column: "ForUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadgeValues_AspNetUsers_UserId",
                table: "UserBadgeValues",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

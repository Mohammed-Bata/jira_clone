using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addNotificationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMember_AspNetUsers_UserId",
                table: "ProjectMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMember_Projects_ProjectId",
                table: "ProjectMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember");

            migrationBuilder.RenameTable(
                name: "ProjectMember",
                newName: "ProjectMembers");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMember_UserId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_UserId");

            migrationBuilder.AddColumn<string>(
                name: "ActorId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActorId",
                table: "Notifications",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ActorId",
                table: "Notifications",
                column: "ActorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ActorId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ActorId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                newName: "ProjectMember");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMember",
                newName: "IX_ProjectMember_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember",
                columns: new[] { "ProjectId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMember_AspNetUsers_UserId",
                table: "ProjectMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMember_Projects_ProjectId",
                table: "ProjectMember",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class handleCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_BoardColumns_BoardColumnId",
                table: "WorkItems");

            migrationBuilder.DropTable(
                name: "BoardColumns");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "BoardColumnId",
                table: "WorkItems",
                newName: "ProjectColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_BoardColumnId",
                table: "WorkItems",
                newName: "IX_WorkItems_ProjectColumnId");

            migrationBuilder.CreateTable(
                name: "ProjectColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectColumns_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectColumns_ProjectId",
                table: "ProjectColumns",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_ProjectColumns_ProjectColumnId",
                table: "WorkItems",
                column: "ProjectColumnId",
                principalTable: "ProjectColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_ProjectColumns_ProjectColumnId",
                table: "WorkItems");

            migrationBuilder.DropTable(
                name: "ProjectColumns");

            migrationBuilder.RenameColumn(
                name: "ProjectColumnId",
                table: "WorkItems",
                newName: "BoardColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ProjectColumnId",
                table: "WorkItems",
                newName: "IX_WorkItems_BoardColumnId");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardColumns", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_BoardColumns_BoardColumnId",
                table: "WorkItems",
                column: "BoardColumnId",
                principalTable: "BoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

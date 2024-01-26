using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Dal.Migrations.CntContextsMigrations
{
    /// <inheritdoc />
    public partial class commentAdd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "cnt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WebSite = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    UnicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                schema: "cnt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    UnicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UnicId",
                schema: "cnt",
                table: "Comments",
                column: "UnicId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UnicId",
                schema: "cnt",
                table: "Scores",
                column: "UnicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments",
                schema: "cnt");

            migrationBuilder.DropTable(
                name: "Scores",
                schema: "cnt");
        }
    }
}

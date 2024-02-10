using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Dal.Migrations.CntContextsMigrations
{
    /// <inheritdoc />
    public partial class postoptionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostOptions",
                schema: "cnt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoFlow = table.Column<bool>(type: "bit", nullable: true),
                    NoIndex = table.Column<bool>(type: "bit", nullable: true),
                    IsComment = table.Column<bool>(type: "bit", nullable: true),
                    IsScore = table.Column<bool>(type: "bit", nullable: true),
                    Redirect = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    UnicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostOptions_UnicId",
                schema: "cnt",
                table: "PostOptions",
                column: "UnicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostOptions",
                schema: "cnt");
        }
    }
}

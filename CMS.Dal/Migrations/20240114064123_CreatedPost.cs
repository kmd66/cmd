using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Dal.Migrations
{
    /// <inheritdoc />
    public partial class CreatedPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "pbl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    Content = table.Column<bool>(type: "bit", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Special = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Published = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PublishDown = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Access = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    Hit = table.Column<int>(type: "int", nullable: false),
                    UnicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UnicId",
                schema: "pbl",
                table: "Posts",
                column: "UnicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts",
                schema: "pbl");
        }
    }
}

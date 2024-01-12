using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Dal.Migrations
{
    /// <inheritdoc />
    public partial class MenuUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                schema: "pbl",
                table: "Menus",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                schema: "pbl",
                table: "Menus");
        }
    }
}

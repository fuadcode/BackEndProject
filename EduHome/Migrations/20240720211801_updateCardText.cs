using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.Migrations
{
    /// <inheritdoc />
    public partial class updateCardText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CardTexts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "CardTexts");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.Migrations
{
    /// <inheritdoc />
    public partial class statusadding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplied",
                table: "MessageForms");

            migrationBuilder.RenameColumn(
                name: "Reply",
                table: "MessageForms",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MessageForms",
                newName: "Reply");

            migrationBuilder.AddColumn<bool>(
                name: "IsReplied",
                table: "MessageForms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

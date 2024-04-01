using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProElectionV2.Migrations
{
    /// <inheritdoc />
    public partial class AddElectionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectionType",
                table: "Elections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectionType",
                table: "Elections");
        }
    }
}

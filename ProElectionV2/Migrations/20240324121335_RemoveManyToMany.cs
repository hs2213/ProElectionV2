using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProElectionV2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Candidates",
                table: "Elections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Candidates",
                table: "Elections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProElectionV2.Migrations
{
    /// <inheritdoc />
    public partial class AddListOfElectionsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipatingElections",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipatingElections",
                table: "Users");
        }
    }
}

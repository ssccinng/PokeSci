using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonIsshoni.Net.Server.Migrations
{
    public partial class AddBOGId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BO",
                table: "PCLBattles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "PCLBattles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwissRoundIdx",
                table: "PCLBattles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BO",
                table: "PCLBattles");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "PCLBattles");

            migrationBuilder.DropColumn(
                name: "SwissRoundIdx",
                table: "PCLBattles");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonIsshoni.Net.Server.Migrations
{
    public partial class RemoveRpBId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCLBattles_PCLRoundPlayers_PCLRoundPlayerId",
                table: "PCLBattles");

            migrationBuilder.DropIndex(
                name: "IX_PCLBattles_PCLRoundPlayerId",
                table: "PCLBattles");

            migrationBuilder.DropColumn(
                name: "PCLRoundPlayerId",
                table: "PCLBattles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PCLRoundPlayerId",
                table: "PCLBattles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_PCLRoundPlayerId",
                table: "PCLBattles",
                column: "PCLRoundPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCLBattles_PCLRoundPlayers_PCLRoundPlayerId",
                table: "PCLBattles",
                column: "PCLRoundPlayerId",
                principalTable: "PCLRoundPlayers",
                principalColumn: "Id");
        }
    }
}

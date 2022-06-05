using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonIsshoni.Net.Server.Migrations
{
    public partial class upteamio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCLBattles_PCLMatchs_PCLMatchId",
                table: "PCLBattles");

            migrationBuilder.DropIndex(
                name: "IX_PCLBattles_PCLMatchId",
                table: "PCLBattles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_PCLMatchId",
                table: "PCLBattles",
                column: "PCLMatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCLBattles_PCLMatchs_PCLMatchId",
                table: "PCLBattles",
                column: "PCLMatchId",
                principalTable: "PCLMatchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

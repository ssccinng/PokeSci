using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonDataAccess.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PSPokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PSName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PSImgName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PSChsName = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    AllValue = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSPokemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PSPokemons_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PSPokemons_PokemonId",
                table: "PSPokemons",
                column: "PokemonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PSPokemons");
        }
    }
}

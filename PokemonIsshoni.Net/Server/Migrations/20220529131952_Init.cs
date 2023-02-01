using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonIsshoni.Net.Server.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    AbilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(40)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "日文名"),
                    description_Chs = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "中文描述"),
                    description_Eng = table.Column<string>(type: "varchar(200)", nullable: true, comment: "英文描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description_Jpn = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "日文描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.AbilityId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(2)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(10)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(10)", nullable: true, comment: "日文名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Data = table.Column<string>(type: "longtext", maxLength: 50000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Egg_Groups",
                columns: table => new
                {
                    EggGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(40)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "日文名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egg_Groups", x => x.EggGroupId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(40)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "日文名"),
                    description_Chs = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "中文描述"),
                    description_Eng = table.Column<string>(type: "varchar(200)", nullable: true, comment: "英文描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description_Jpn = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "日文描述"),
                    Item_Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Use = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Algorithm = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsX509Certificate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataProtected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLMatchs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(270)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    MatchStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MatchEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AllowGuest = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTeamCompeition = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanCancelSign = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MatchType = table.Column<int>(type: "int", nullable: false),
                    MatchOnline = table.Column<int>(type: "int", nullable: false),
                    MatchState = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logo = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoundIdx = table.Column<int>(type: "int", nullable: false),
                    NeedCheck = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LimitPlayer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLMatchs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLMatchTeamGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CaptainId = table.Column<string>(type: "varchar(270)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLMatchTeamGroup", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLPokeTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PSText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamData = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLPokeTeams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Performances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(2)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(10)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(6)", nullable: true, comment: "日文名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performances", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Data = table.Column<string>(type: "longtext", maxLength: 50000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PokeDex",
                columns: table => new
                {
                    PokeDexId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    National_Dex_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokeDex", x => x.PokeDexId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PokeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(5)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(20)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(10)", nullable: true, comment: "日文名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokeTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLMatchRounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PCLMatchId = table.Column<int>(type: "int", nullable: false),
                    PCLRoundType = table.Column<int>(type: "int", nullable: false),
                    IsGroup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GroupCnt = table.Column<int>(type: "int", nullable: false),
                    PCLRoundState = table.Column<int>(type: "int", nullable: false),
                    LockTeam = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AcceptTeamSubmit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSeeOppTeam = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BO = table.Column<int>(type: "int", nullable: false),
                    RoundPromotion = table.Column<int>(type: "int", nullable: false),
                    WinScore = table.Column<int>(type: "int", nullable: false),
                    DrawScore = table.Column<int>(type: "int", nullable: false),
                    LoseScore = table.Column<int>(type: "int", nullable: false),
                    SwissCount = table.Column<int>(type: "int", nullable: false),
                    Swissidx = table.Column<int>(type: "int", nullable: false),
                    SwissPromotionType = table.Column<int>(type: "int", nullable: false),
                    EliminationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLMatchRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCLMatchRounds_PCLMatchs_PCLMatchId",
                        column: x => x.PCLMatchId,
                        principalTable: "PCLMatchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLReferees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(270)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PCLMatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLReferees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCLReferees_PCLMatchs_PCLMatchId",
                        column: x => x.PCLMatchId,
                        principalTable: "PCLMatchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLMatchPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(270)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PCLMatchId = table.Column<int>(type: "int", nullable: false),
                    Declaration = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ShadowId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    PCLMatchTeamGroupId = table.Column<int>(type: "int", nullable: true),
                    QQ = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsChecked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreTeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLMatchPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCLMatchPlayers_PCLMatchs_PCLMatchId",
                        column: x => x.PCLMatchId,
                        principalTable: "PCLMatchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLMatchPlayers_PCLMatchTeamGroup_PCLMatchTeamGroupId",
                        column: x => x.PCLMatchTeamGroupId,
                        principalTable: "PCLMatchTeamGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PCLMatchPlayers_PCLPokeTeams_PreTeamId",
                        column: x => x.PreTeamId,
                        principalTable: "PCLPokeTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Flavors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(1)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(10)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(5)", nullable: true, comment: "日文名"),
                    Condition_UpId = table.Column<int>(type: "int", nullable: true),
                    Performance_UpId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flavors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flavors_Conditions_Condition_UpId",
                        column: x => x.Condition_UpId,
                        principalTable: "Conditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flavors_Performances_Performance_UpId",
                        column: x => x.Performance_UpId,
                        principalTable: "Performances",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    MoveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(40)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "日文名"),
                    description_Chs = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "中文描述"),
                    description_Eng = table.Column<string>(type: "varchar(200)", nullable: true, comment: "英文描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description_Jpn = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "日文描述"),
                    Pow = table.Column<int>(type: "int", nullable: true),
                    Acc = table.Column<int>(type: "int", nullable: true),
                    PP = table.Column<int>(type: "int", nullable: false),
                    MoveTypeId = table.Column<int>(type: "int", nullable: true),
                    Damage_Type = table.Column<string>(type: "nvarchar(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                    table.ForeignKey(
                        name: "FK_Moves_PokeTypes_MoveTypeId",
                        column: x => x.MoveTypeId,
                        principalTable: "PokeTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DexId = table.Column<int>(type: "int", nullable: false, comment: "全国图鉴编号"),
                    NameChs = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "中文名"),
                    NameEng = table.Column<string>(type: "varchar(40)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NameJpn = table.Column<string>(type: "nvarchar(20)", nullable: true, comment: "日文名"),
                    PokeFormId = table.Column<int>(type: "int", nullable: false, comment: "形态编号"),
                    FormNameChs = table.Column<string>(type: "nvarchar(30)", nullable: true, comment: "形态名"),
                    FormNameEng = table.Column<string>(type: "varchar(40)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormNameJpn = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    FullNameChs = table.Column<string>(type: "nvarchar(30)", nullable: true, comment: "全名"),
                    FullNameEng = table.Column<string>(type: "varchar(60)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullNameJpn = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Stage = table.Column<int>(type: "int", nullable: false, comment: "意味無し"),
                    DMax = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "能否极巨化"),
                    BaseHP = table.Column<int>(type: "int", nullable: false),
                    BaseAtk = table.Column<int>(type: "int", nullable: false),
                    BaseDef = table.Column<int>(type: "int", nullable: false),
                    BaseSpa = table.Column<int>(type: "int", nullable: false),
                    BaseSpd = table.Column<int>(type: "int", nullable: false),
                    BaseSpe = table.Column<int>(type: "int", nullable: false),
                    EVHP = table.Column<int>(type: "int", nullable: false),
                    EVAtk = table.Column<int>(type: "int", nullable: false),
                    EVDef = table.Column<int>(type: "int", nullable: false),
                    EVSpa = table.Column<int>(type: "int", nullable: false),
                    EVSpd = table.Column<int>(type: "int", nullable: false),
                    EVSpe = table.Column<int>(type: "int", nullable: false),
                    GenderRatio = table.Column<int>(type: "int", nullable: false, comment: "♀/(♂+♀)=(GenderRatio+1)/(255+1),254雌性,255无性别"),
                    CatchRate = table.Column<int>(type: "int", nullable: false),
                    Ability1AbilityId = table.Column<int>(type: "int", nullable: true),
                    Ability2AbilityId = table.Column<int>(type: "int", nullable: true),
                    AbilityHAbilityId = table.Column<int>(type: "int", nullable: true),
                    Type1Id = table.Column<int>(type: "int", nullable: true),
                    Type2Id = table.Column<int>(type: "int", nullable: true),
                    EXPGroup = table.Column<int>(type: "int", nullable: false, comment: "1:Erratic,2:Fast,3:Medium Fast,4:Medium Slow,5:Slow"),
                    EggGroup1EggGroupId = table.Column<int>(type: "int", nullable: true),
                    EggGroup2EggGroupId = table.Column<int>(type: "int", nullable: true),
                    HatchCycles = table.Column<int>(type: "int", nullable: false, comment: "孵化周期"),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(4,1)", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false, comment: "0-9:红色,蓝色,绿色,黄色,紫色,粉红色,褐色,黑色,灰色,白色"),
                    PokeDexId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pokemons_Abilities_Ability1AbilityId",
                        column: x => x.Ability1AbilityId,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId");
                    table.ForeignKey(
                        name: "FK_Pokemons_Abilities_Ability2AbilityId",
                        column: x => x.Ability2AbilityId,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId");
                    table.ForeignKey(
                        name: "FK_Pokemons_Abilities_AbilityHAbilityId",
                        column: x => x.AbilityHAbilityId,
                        principalTable: "Abilities",
                        principalColumn: "AbilityId");
                    table.ForeignKey(
                        name: "FK_Pokemons_Egg_Groups_EggGroup1EggGroupId",
                        column: x => x.EggGroup1EggGroupId,
                        principalTable: "Egg_Groups",
                        principalColumn: "EggGroupId");
                    table.ForeignKey(
                        name: "FK_Pokemons_Egg_Groups_EggGroup2EggGroupId",
                        column: x => x.EggGroup2EggGroupId,
                        principalTable: "Egg_Groups",
                        principalColumn: "EggGroupId");
                    table.ForeignKey(
                        name: "FK_Pokemons_PokeDex_PokeDexId",
                        column: x => x.PokeDexId,
                        principalTable: "PokeDex",
                        principalColumn: "PokeDexId");
                    table.ForeignKey(
                        name: "FK_Pokemons_PokeTypes_Type1Id",
                        column: x => x.Type1Id,
                        principalTable: "PokeTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pokemons_PokeTypes_Type2Id",
                        column: x => x.Type2Id,
                        principalTable: "PokeTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TypeEffect",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type1Id = table.Column<int>(type: "int", nullable: true),
                    Type2Id = table.Column<int>(type: "int", nullable: true),
                    Effect = table.Column<decimal>(type: "decimal(2,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEffect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeEffect_PokeTypes_Type1Id",
                        column: x => x.Type1Id,
                        principalTable: "PokeTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypeEffect_PokeTypes_Type2Id",
                        column: x => x.Type2Id,
                        principalTable: "PokeTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLRoundPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(270)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PCLMatchRoundId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Win = table.Column<int>(type: "int", nullable: false),
                    Draw = table.Column<int>(type: "int", nullable: false),
                    Lose = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    IsDrop = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tag = table.Column<int>(type: "int", nullable: false),
                    HasBye = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OppRatio = table.Column<decimal>(type: "decimal(8,6)", nullable: false),
                    OppOppRatio = table.Column<decimal>(type: "decimal(8,6)", nullable: false),
                    MiniScore = table.Column<int>(type: "int", nullable: false),
                    BattleTeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLRoundPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCLRoundPlayers_PCLMatchRounds_PCLMatchRoundId",
                        column: x => x.PCLMatchRoundId,
                        principalTable: "PCLMatchRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLRoundPlayers_PCLPokeTeams_BattleTeamId",
                        column: x => x.BattleTeamId,
                        principalTable: "PCLPokeTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(3)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(20)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(10)", nullable: true, comment: "日文名"),
                    FlavorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_Flavors_FlavorId",
                        column: x => x.FlavorId,
                        principalTable: "Flavors",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PSPokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PSName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PSImgName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PSChsName = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    AllValue = table.Column<int>(type: "int", nullable: false),
                    PokemonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PSPokemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PSPokemons_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PCLBattles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PCLMatchRoundId = table.Column<int>(type: "int", nullable: false),
                    PCLMatchId = table.Column<int>(type: "int", nullable: false),
                    Player1Id = table.Column<string>(type: "varchar(270)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Player2Id = table.Column<string>(type: "varchar(270)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PCLBattleState = table.Column<int>(type: "int", nullable: false),
                    Player1TeamId = table.Column<int>(type: "int", nullable: false),
                    Player2TeamId = table.Column<int>(type: "int", nullable: false),
                    Player1Score = table.Column<int>(type: "int", nullable: false),
                    Player2Score = table.Column<int>(type: "int", nullable: false),
                    Player1MiniScore = table.Column<int>(type: "int", nullable: false),
                    Player2MiniScore = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Submitted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tag = table.Column<int>(type: "int", nullable: false),
                    PCLRoundPlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCLBattles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCLBattles_PCLMatchRounds_PCLMatchRoundId",
                        column: x => x.PCLMatchRoundId,
                        principalTable: "PCLMatchRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLBattles_PCLMatchs_PCLMatchId",
                        column: x => x.PCLMatchId,
                        principalTable: "PCLMatchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLBattles_PCLPokeTeams_Player1TeamId",
                        column: x => x.Player1TeamId,
                        principalTable: "PCLPokeTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLBattles_PCLPokeTeams_Player2TeamId",
                        column: x => x.Player2TeamId,
                        principalTable: "PCLPokeTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCLBattles_PCLRoundPlayers_PCLRoundPlayerId",
                        column: x => x.PCLRoundPlayerId,
                        principalTable: "PCLRoundPlayers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Natures",
                columns: table => new
                {
                    NatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Chs = table.Column<string>(type: "nvarchar(5)", nullable: true, comment: "中文名"),
                    Name_Eng = table.Column<string>(type: "varchar(15)", nullable: true, comment: "英文名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_Jpn = table.Column<string>(type: "nvarchar(10)", nullable: true, comment: "日文名"),
                    Stat_UpId = table.Column<int>(type: "int", nullable: true),
                    Stat_DownId = table.Column<int>(type: "int", nullable: true),
                    Perf_UpId = table.Column<int>(type: "int", nullable: true),
                    Perf_DownId = table.Column<int>(type: "int", nullable: true),
                    Flavor_UpId = table.Column<int>(type: "int", nullable: true),
                    Flavor_DownId = table.Column<int>(type: "int", nullable: true),
                    Performance_value = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natures", x => x.NatureId);
                    table.ForeignKey(
                        name: "FK_Natures_Flavors_Flavor_DownId",
                        column: x => x.Flavor_DownId,
                        principalTable: "Flavors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natures_Flavors_Flavor_UpId",
                        column: x => x.Flavor_UpId,
                        principalTable: "Flavors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natures_Performances_Perf_DownId",
                        column: x => x.Perf_DownId,
                        principalTable: "Performances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natures_Performances_Perf_UpId",
                        column: x => x.Perf_UpId,
                        principalTable: "Performances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natures_Statistics_Stat_DownId",
                        column: x => x.Stat_DownId,
                        principalTable: "Statistics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natures_Statistics_Stat_UpId",
                        column: x => x.Stat_UpId,
                        principalTable: "Statistics",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Flavors_Condition_UpId",
                table: "Flavors",
                column: "Condition_UpId");

            migrationBuilder.CreateIndex(
                name: "IX_Flavors_Performance_UpId",
                table: "Flavors",
                column: "Performance_UpId");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Use",
                table: "Keys",
                column: "Use");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_MoveTypeId",
                table: "Moves",
                column: "MoveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Flavor_DownId",
                table: "Natures",
                column: "Flavor_DownId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Flavor_UpId",
                table: "Natures",
                column: "Flavor_UpId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Perf_DownId",
                table: "Natures",
                column: "Perf_DownId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Perf_UpId",
                table: "Natures",
                column: "Perf_UpId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Stat_DownId",
                table: "Natures",
                column: "Stat_DownId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Stat_UpId",
                table: "Natures",
                column: "Stat_UpId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_PCLMatchId",
                table: "PCLBattles",
                column: "PCLMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_PCLMatchRoundId",
                table: "PCLBattles",
                column: "PCLMatchRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_PCLRoundPlayerId",
                table: "PCLBattles",
                column: "PCLRoundPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_Player1TeamId",
                table: "PCLBattles",
                column: "Player1TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLBattles_Player2TeamId",
                table: "PCLBattles",
                column: "Player2TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLMatchPlayers_PCLMatchId",
                table: "PCLMatchPlayers",
                column: "PCLMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLMatchPlayers_PCLMatchTeamGroupId",
                table: "PCLMatchPlayers",
                column: "PCLMatchTeamGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLMatchPlayers_PreTeamId",
                table: "PCLMatchPlayers",
                column: "PreTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLMatchRounds_PCLMatchId",
                table: "PCLMatchRounds",
                column: "PCLMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLReferees_PCLMatchId",
                table: "PCLReferees",
                column: "PCLMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLRoundPlayers_BattleTeamId",
                table: "PCLRoundPlayers",
                column: "BattleTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PCLRoundPlayers_PCLMatchRoundId",
                table: "PCLRoundPlayers",
                column: "PCLMatchRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants",
                column: "ConsumedTime");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_SessionId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_Ability1AbilityId",
                table: "Pokemons",
                column: "Ability1AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_Ability2AbilityId",
                table: "Pokemons",
                column: "Ability2AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_AbilityHAbilityId",
                table: "Pokemons",
                column: "AbilityHAbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_EggGroup1EggGroupId",
                table: "Pokemons",
                column: "EggGroup1EggGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_EggGroup2EggGroupId",
                table: "Pokemons",
                column: "EggGroup2EggGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_PokeDexId",
                table: "Pokemons",
                column: "PokeDexId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_Type1Id",
                table: "Pokemons",
                column: "Type1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_Type2Id",
                table: "Pokemons",
                column: "Type2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PSPokemons_PokemonId",
                table: "PSPokemons",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_FlavorId",
                table: "Statistics",
                column: "FlavorId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeEffect_Type1Id",
                table: "TypeEffect",
                column: "Type1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TypeEffect_Type2Id",
                table: "TypeEffect",
                column: "Type2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropTable(
                name: "PCLBattles");

            migrationBuilder.DropTable(
                name: "PCLMatchPlayers");

            migrationBuilder.DropTable(
                name: "PCLReferees");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "PSPokemons");

            migrationBuilder.DropTable(
                name: "TypeEffect");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "PCLRoundPlayers");

            migrationBuilder.DropTable(
                name: "PCLMatchTeamGroup");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Flavors");

            migrationBuilder.DropTable(
                name: "PCLMatchRounds");

            migrationBuilder.DropTable(
                name: "PCLPokeTeams");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Egg_Groups");

            migrationBuilder.DropTable(
                name: "PokeDex");

            migrationBuilder.DropTable(
                name: "PokeTypes");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "Performances");

            migrationBuilder.DropTable(
                name: "PCLMatchs");
        }
    }
}

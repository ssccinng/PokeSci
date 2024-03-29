﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonDataAccess;

#nullable disable

namespace PokemonDataAccess.Migrations
{
    [DbContext(typeof(PokemonContext))]
    [Migration("20230402063640_Init2")]
    partial class Init2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("PokemonDataAccess.Models.Ability", b =>
                {
                    b.Property<int>("AbilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(40)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("日文名");

                    b.Property<string>("description_Chs")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("中文描述");

                    b.Property<string>("description_Eng")
                        .HasColumnType("varchar(200)")
                        .HasComment("英文描述");

                    b.Property<string>("description_Jpn")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("日文描述");

                    b.HasKey("AbilityId");

                    b.ToTable("Abilities");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Condition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(2)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(10)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(10)")
                        .HasComment("日文名");

                    b.HasKey("Id");

                    b.ToTable("Conditions");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.EggGroup", b =>
                {
                    b.Property<int>("EggGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(40)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("日文名");

                    b.HasKey("EggGroupId");

                    b.ToTable("Egg_Groups");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Flavor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Condition_UpId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(1)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(10)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(5)")
                        .HasComment("日文名");

                    b.Property<int?>("Performance_UpId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Condition_UpId");

                    b.HasIndex("Performance_UpId");

                    b.ToTable("Flavors");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Item_Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(40)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("日文名");

                    b.Property<string>("description_Chs")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("中文描述");

                    b.Property<string>("description_Eng")
                        .HasColumnType("varchar(200)")
                        .HasComment("英文描述");

                    b.Property<string>("description_Jpn")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("日文描述");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Move", b =>
                {
                    b.Property<int>("MoveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Acc")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Damage_Type")
                        .HasColumnType("nvarchar(6)");

                    b.Property<int?>("MoveTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(40)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("日文名");

                    b.Property<int>("PP")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Pow")
                        .HasColumnType("INTEGER");

                    b.Property<string>("description_Chs")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("中文描述");

                    b.Property<string>("description_Eng")
                        .HasColumnType("varchar(200)")
                        .HasComment("英文描述");

                    b.Property<string>("description_Jpn")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("日文描述");

                    b.HasKey("MoveId");

                    b.HasIndex("MoveTypeId");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Nature", b =>
                {
                    b.Property<int>("NatureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Flavor_DownId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Flavor_UpId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(5)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(15)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(10)")
                        .HasComment("日文名");

                    b.Property<int?>("Perf_DownId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Perf_UpId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Performance_value")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Stat_DownId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Stat_UpId")
                        .HasColumnType("INTEGER");

                    b.HasKey("NatureId");

                    b.HasIndex("Flavor_DownId");

                    b.HasIndex("Flavor_UpId");

                    b.HasIndex("Perf_DownId");

                    b.HasIndex("Perf_UpId");

                    b.HasIndex("Stat_DownId");

                    b.HasIndex("Stat_UpId");

                    b.ToTable("Natures");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Performance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(2)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(10)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(6)")
                        .HasComment("日文名");

                    b.HasKey("Id");

                    b.ToTable("Performances");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.PokeDex", b =>
                {
                    b.Property<int>("PokeDexId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("National_Dex_Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokeDexId");

                    b.ToTable("PokeDex");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ability1AbilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ability2AbilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AbilityHAbilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseAtk")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseDef")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseHP")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseSpa")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseSpd")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseSpe")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CatchRate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Color")
                        .HasColumnType("INTEGER")
                        .HasComment("0-9:红色,蓝色,绿色,黄色,紫色,粉红色,褐色,黑色,灰色,白色");

                    b.Property<bool>("DMax")
                        .HasColumnType("INTEGER")
                        .HasComment("能否极巨化");

                    b.Property<int>("DexId")
                        .HasColumnType("INTEGER")
                        .HasComment("全国图鉴编号");

                    b.Property<int>("EVAtk")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EVDef")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EVHP")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EVSpa")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EVSpd")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EVSpe")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EXPGroup")
                        .HasColumnType("INTEGER")
                        .HasComment("1:Erratic,2:Fast,3:Medium Fast,4:Medium Slow,5:Slow");

                    b.Property<int?>("EggGroup1EggGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EggGroup2EggGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FormNameChs")
                        .HasColumnType("nvarchar(30)")
                        .HasComment("形态名");

                    b.Property<string>("FormNameEng")
                        .HasColumnType("varchar(40)");

                    b.Property<string>("FormNameJpn")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("FullNameChs")
                        .HasColumnType("nvarchar(30)")
                        .HasComment("全名");

                    b.Property<string>("FullNameEng")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("FullNameJpn")
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("GenderRatio")
                        .HasColumnType("INTEGER")
                        .HasComment("♀/(♂+♀)=(GenderRatio+1)/(255+1),254雌性,255无性别");

                    b.Property<int>("HatchCycles")
                        .HasColumnType("INTEGER")
                        .HasComment("孵化周期");

                    b.Property<decimal>("Height")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("NameChs")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("中文名");

                    b.Property<string>("NameEng")
                        .HasColumnType("varchar(40)")
                        .HasComment("英文名");

                    b.Property<string>("NameJpn")
                        .HasColumnType("nvarchar(20)")
                        .HasComment("日文名");

                    b.Property<int?>("PokeDexId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokeFormId")
                        .HasColumnType("INTEGER")
                        .HasComment("形态编号");

                    b.Property<int>("Stage")
                        .HasColumnType("INTEGER")
                        .HasComment("意味無し");

                    b.Property<int?>("Type1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Type2Id")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(4, 1)");

                    b.HasKey("Id");

                    b.HasIndex("Ability1AbilityId");

                    b.HasIndex("Ability2AbilityId");

                    b.HasIndex("AbilityHAbilityId");

                    b.HasIndex("EggGroup1EggGroupId");

                    b.HasIndex("EggGroup2EggGroupId");

                    b.HasIndex("PokeDexId");

                    b.HasIndex("Type1Id");

                    b.HasIndex("Type2Id");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.PokeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(5)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(20)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(10)")
                        .HasComment("日文名");

                    b.HasKey("Id");

                    b.ToTable("PokeTypes");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.PSPokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AllValue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PSChsName")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PSImgName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PSName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.ToTable("PSPokemons");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Statistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FlavorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name_Chs")
                        .HasColumnType("nvarchar(3)")
                        .HasComment("中文名");

                    b.Property<string>("Name_Eng")
                        .HasColumnType("varchar(20)")
                        .HasComment("英文名");

                    b.Property<string>("Name_Jpn")
                        .HasColumnType("nvarchar(10)")
                        .HasComment("日文名");

                    b.HasKey("Id");

                    b.HasIndex("FlavorId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.TypeEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Effect")
                        .HasColumnType("decimal(2, 1)");

                    b.Property<int?>("Type1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Type2Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Type1Id");

                    b.HasIndex("Type2Id");

                    b.ToTable("TypeEffect");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Flavor", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.Condition", "Condition_Up")
                        .WithMany()
                        .HasForeignKey("Condition_UpId");

                    b.HasOne("PokemonDataAccess.Models.Performance", "Performance_Up")
                        .WithMany()
                        .HasForeignKey("Performance_UpId");

                    b.Navigation("Condition_Up");

                    b.Navigation("Performance_Up");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Move", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.PokeType", "MoveType")
                        .WithMany()
                        .HasForeignKey("MoveTypeId");

                    b.Navigation("MoveType");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Nature", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.Flavor", "Flavor_Down")
                        .WithMany()
                        .HasForeignKey("Flavor_DownId");

                    b.HasOne("PokemonDataAccess.Models.Flavor", "Flavor_Up")
                        .WithMany()
                        .HasForeignKey("Flavor_UpId");

                    b.HasOne("PokemonDataAccess.Models.Performance", "Perf_Down")
                        .WithMany()
                        .HasForeignKey("Perf_DownId");

                    b.HasOne("PokemonDataAccess.Models.Performance", "Perf_Up")
                        .WithMany()
                        .HasForeignKey("Perf_UpId");

                    b.HasOne("PokemonDataAccess.Models.Statistic", "Stat_Down")
                        .WithMany()
                        .HasForeignKey("Stat_DownId");

                    b.HasOne("PokemonDataAccess.Models.Statistic", "Stat_Up")
                        .WithMany()
                        .HasForeignKey("Stat_UpId");

                    b.Navigation("Flavor_Down");

                    b.Navigation("Flavor_Up");

                    b.Navigation("Perf_Down");

                    b.Navigation("Perf_Up");

                    b.Navigation("Stat_Down");

                    b.Navigation("Stat_Up");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Pokemon", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.Ability", "Ability1")
                        .WithMany()
                        .HasForeignKey("Ability1AbilityId");

                    b.HasOne("PokemonDataAccess.Models.Ability", "Ability2")
                        .WithMany()
                        .HasForeignKey("Ability2AbilityId");

                    b.HasOne("PokemonDataAccess.Models.Ability", "AbilityH")
                        .WithMany()
                        .HasForeignKey("AbilityHAbilityId");

                    b.HasOne("PokemonDataAccess.Models.EggGroup", "EggGroup1")
                        .WithMany()
                        .HasForeignKey("EggGroup1EggGroupId");

                    b.HasOne("PokemonDataAccess.Models.EggGroup", "EggGroup2")
                        .WithMany()
                        .HasForeignKey("EggGroup2EggGroupId");

                    b.HasOne("PokemonDataAccess.Models.PokeDex", null)
                        .WithMany("Pokemon_Form_List")
                        .HasForeignKey("PokeDexId");

                    b.HasOne("PokemonDataAccess.Models.PokeType", "Type1")
                        .WithMany()
                        .HasForeignKey("Type1Id");

                    b.HasOne("PokemonDataAccess.Models.PokeType", "Type2")
                        .WithMany()
                        .HasForeignKey("Type2Id");

                    b.Navigation("Ability1");

                    b.Navigation("Ability2");

                    b.Navigation("AbilityH");

                    b.Navigation("EggGroup1");

                    b.Navigation("EggGroup2");

                    b.Navigation("Type1");

                    b.Navigation("Type2");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.PSPokemon", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.Pokemon", "Pokemon")
                        .WithMany()
                        .HasForeignKey("PokemonId");

                    b.Navigation("Pokemon");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.Statistic", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.Flavor", "Flavor")
                        .WithMany()
                        .HasForeignKey("FlavorId");

                    b.Navigation("Flavor");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.TypeEffect", b =>
                {
                    b.HasOne("PokemonDataAccess.Models.PokeType", "Type1")
                        .WithMany()
                        .HasForeignKey("Type1Id");

                    b.HasOne("PokemonDataAccess.Models.PokeType", "Type2")
                        .WithMany()
                        .HasForeignKey("Type2Id");

                    b.Navigation("Type1");

                    b.Navigation("Type2");
                });

            modelBuilder.Entity("PokemonDataAccess.Models.PokeDex", b =>
                {
                    b.Navigation("Pokemon_Form_List");
                });
#pragma warning restore 612, 618
        }
    }
}

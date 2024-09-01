﻿// See https://aka.ms/new-console-template for more information
using Poke.UPDL;
using PokeCommon.Models;
using PokeCommon.PokemonShowdownTools;
using System.Collections;
using System.Text;
using System.Text.Json;

string priteamps = @"Raging Bolt @ Assault Vest  
Ability: Protosynthesis  
Level: 50  
Tera Type: Electric  
EVs: 148 HP / 76 Def / 252 SpA / 20 SpD / 12 Spe  
Modest Nature  
IVs: 20 Atk  
- Thunderbolt  
- Draco Meteor  
- Thunderclap  
- Volt Switch  

Calyrex-Ice @ Clear Amulet  
Ability: As One (Glastrier)  
Level: 50  
Tera Type: Fire  
EVs: 252 HP / 252 Atk / 4 SpD  
Adamant Nature  
IVs: 29 Spe  
- Glacial Lance  
- Close Combat  
- Trick Room  
- Protect  

Farigiraf @ Safety Goggles  
Ability: Armor Tail  
Level: 50  
Tera Type: Dragon  
EVs: 220 HP / 52 Def / 236 SpD  
Modest Nature  
IVs: 0 Atk / 7 Spe  
- Psychic  
- Uproar  
- Helping Hand  
- Trick Room  

Incineroar @ Sitrus Berry  
Ability: Intimidate  
Level: 50  
Tera Type: Grass  
EVs: 156 HP / 100 Def / 252 Spe  
Jolly Nature  
- Flare Blitz  
- Knock Off  
- Fake Out  
- Parting Shot  

Amoonguss @ Focus Sash  
Ability: Regenerator  
Level: 50  
Tera Type: Water  
EVs: 204 HP / 156 Def / 148 SpD  
Relaxed Nature  
IVs: 0 Atk / 2 Spe  
- Rage Powder  
- Spore  
- Pollen Puff  
- Clear Smog  

Urshifu-Rapid-Strike @ Choice Scarf  
Ability: Unseen Fist  
Level: 50  
Tera Type: Water  
EVs: 4 HP / 252 Atk / 252 Spe  
Adamant Nature  
- Surging Strikes  
- Close Combat  
- U-turn  
- Coaching  
";

var teampri = await PSConverterWithoutDB.ConvertToPokemonsAsync(priteamps);
var cc = JsonSerializer.Serialize(teampri.ToSimpleGamePokemonTeam());

var ccc = UPDLParser.GamePokeToBytes(JsonSerializer.Deserialize<SimpleGamePokemonTeam>(cc));

Console.WriteLine(Convert.ToBase64String(ccc));

var cccc = UPDLParser.DeserializeAndDecompress([1, 2]);
//var cccc = UPDLParser.DeserializeAndDecompress(Convert.ToBase64String(ccc));
return;
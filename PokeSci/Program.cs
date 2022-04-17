//Double CPUtprt = 0;
//System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(@"root\WMI", "Select * From MSAcpi_ThermalZoneTemperature");
//foreach (System.Management.ManagementObject mo in mos.Get())
//{
//    CPUtprt = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
//    Console.WriteLine("CPU temp : " + CPUtprt.ToString() + " °C");
//}
using PokeCommon.PokemonShowdownTools;
using PokeCommon.PokeOCR;
using System.Diagnostics;

//var a = OCRTools.SplitSWSHTeamPage("test9.jpg");
//List<string> res = new List<string>();
//foreach (var item in a)
//{
//    //res.Add(OCRTools.GetText(item.PokeNameImgPath));
//    res.Add(OCRTools.GetText(item.MoveImgPath));
//}
//res.ForEach(item => Console.Write(item));
Console.WriteLine(Environment.CurrentDirectory);
 var aa = await PSConverter.ConvertToPokemonAsync(@"sdfsdfsdf (Barraskewda) @ Life Orb
Ability: Dark Aura
Level: 50
EVs: 4 HP / 4 Def / 252 SpA / 4 SpD / 244 Spe
Modest Nature
IVs: 0 Atk
- Dark Pulse
- Heat Wave
- Hurricane
- Protect");
Console.WriteLine(aa.Gmax);
Console.WriteLine(aa.MetaPokemon.NameChs);
Console.WriteLine(aa.LV);
Console.WriteLine(aa.NickName);
Console.WriteLine(aa.Ability.Name_Chs);
Console.WriteLine(aa.Nature.Name_Chs);
Console.WriteLine(aa.Item.Name_Chs);
Console.WriteLine(string.Join(", ", aa.IVs.ToSixArray()));
Console.WriteLine(string.Join(", ", aa.EVs.ToSixArray()));
foreach (var item in aa.Moves)
{
    Console.WriteLine(item.NameChs);
}


var team = await PSConverter.ConvertToPokemonsAsync(@"Dialga @ Life Orb
Ability: Pressure
Level: 50
EVs: 20 HP / 252 SpA / 236 Spe
Modest Nature
IVs: 0 Atk
- Roar of Time
- Flash Cannon
- Earth Power
- Trick Room


Kyogre @ Mystic Water
Ability: Drizzle
Level: 50
EVs: 4 HP / 12 Def / 236 SpA / 4 SpD / 252 Spe
Timid Nature
IVs: 0 Atk
- Water Spout
- Scald
- Protect
- Thunder

Whimsicott @ Focus Sash
Ability: Prankster
Level: 50
EVs: 60 HP / 4 Def / 188 SpA / 4 SpD / 252 Spe
Timid Nature
IVs: 0 Atk
- Tailwind
- Fake Tears
- Helping Hand
- Energy Ball

Indeedee-F (F) @ Psychic Seed
Ability: Psychic Surge
Level: 50
EVs: 252 HP / 252 Def / 4 SpD
Bold Nature
IVs: 0 Atk / 29 Spe
- Expanding Force
- Follow Me
- Helping Hand
- Protect

Amoonguss @ Mental Herb
Ability: Regenerator
Level: 50
EVs: 236 HP / 116 Def / 156 SpD
Sassy Nature
IVs: 0 Atk / 0 Spe
- Pollen Puff
- Protect
- Rage Powder
- Spore

Urshifu-Gmax @ Choice Band
Ability: Unseen Fist
Level: 50
EVs: 4 HP / 236 Atk / 12 Def / 4 SpD / 252 Spe
Jolly Nature
- Close Combat
- Wicked Blow
- Sucker Punch
- U-turn");
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
for (int i = 0; i < 10000; i++)
{
    var a = await PSConverter.ConvertToPokemonsAsync(@"Dialga @ Life Orb
Ability: Pressure
Level: 50
EVs: 20 HP / 252 SpA / 236 Spe
Modest Nature
IVs: 0 Atk
- Roar of Time
- Flash Cannon
- Earth Power
- Trick Room


Kyogre @ Mystic Water
Ability: Drizzle
Level: 50
EVs: 4 HP / 12 Def / 236 SpA / 4 SpD / 252 Spe
Timid Nature
IVs: 0 Atk
- Water Spout
- Scald
- Protect
- Thunder

Whimsicott @ Focus Sash
Ability: Prankster
Level: 50
EVs: 60 HP / 4 Def / 188 SpA / 4 SpD / 252 Spe
Timid Nature
IVs: 0 Atk
- Tailwind
- Fake Tears
- Helping Hand
- Energy Ball

Indeedee-F (F) @ Psychic Seed
Ability: Psychic Surge
Level: 50
EVs: 252 HP / 252 Def / 4 SpD
Bold Nature
IVs: 0 Atk / 29 Spe
- Expanding Force
- Follow Me
- Helping Hand
- Protect

Amoonguss @ Mental Herb
Ability: Regenerator
Level: 50
EVs: 236 HP / 116 Def / 156 SpD
Sassy Nature
IVs: 0 Atk / 0 Spe
- Pollen Puff
- Protect
- Rage Powder
- Spore

Urshifu-Gmax @ Choice Band
Ability: Unseen Fist
Level: 50
EVs: 4 HP / 236 Atk / 12 Def / 4 SpD / 252 Spe
Jolly Nature
- Close Combat
- Wicked Blow
- Sucker Punch
- U-turn");
}
stopWatch.Stop();
Console.WriteLine("转换10000次用时 = " + stopWatch.ElapsedMilliseconds + "ms");
foreach (var item in team.GamePokemons)
{
    Console.WriteLine(item.Gmax);
    Console.WriteLine(item.MetaPokemon.NameChs);
    Console.WriteLine(item.LV);
    Console.WriteLine("NickName = " + item.NickName);
    Console.WriteLine(item.Ability.Name_Chs);
    Console.WriteLine(item.Nature.Name_Chs);
    Console.WriteLine(item.Item.Name_Chs);
    Console.WriteLine(string.Join(", ", item.IVs.ToSixArray()));
    Console.WriteLine(string.Join(", ", item.EVs.ToSixArray()));
    Console.WriteLine();
    foreach (var aa1 in item.Moves)
    {
        Console.WriteLine(aa1.NameChs);
    }

}
//OCRTools.SplitSWSHTeamPage();

//BattleEngine.CreateBattleEngine(BattleVersion.DPPt);

//SWSHTools SWSHTools = new SWSHTools();

//int hp = SWSHTools.GetHP(95, 31, 0);
//int bhb = SWSHTools.GetPureBaseHP(hp, 31, 0);
//int spe = SWSHTools.GetOtherStat(60, 31, 0);
//int bspe = SWSHTools.GetPureBaseOtherStat(spe, 31, 0);
//Console.WriteLine(hp);
//Console.WriteLine(bhb);
//Console.WriteLine(spe);
//Console.WriteLine(bspe);
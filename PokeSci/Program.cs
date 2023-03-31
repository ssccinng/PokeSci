//Double CPUtprt = 0;
//System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(@"root\WMI", "Select * From MSAcpi_ThermalZoneTemperature");
//foreach (System.Management.ManagementObject mo in mos.Get())
//{
//    CPUtprt = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
//    Console.WriteLine("CPU temp : " + CPUtprt.ToString() + " °C");
//}
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using PokeCommon.PokemonShowdownTools;
using PokePSCore;
using PSReplayAnalysis;


//var pc21 = new PSClient("kirbyrbp", "11998whs").LogTo(Console.WriteLine);
//await pc21.ConnectAsync();
//await Task.Delay(1000);

//Console.WriteLine(await pc21.LoginAsync());

//return;
var files = Directory.GetFiles("D:\\PS数据_old\\PSreplay_6Yjyd6");
//var files = Directory.GetFiles("F:\\PSReplay\\PSreplay").Take(90000).ToArray();
List<BattleData> batches = new List<BattleData>();
int idx = 0;
var len = files.Length / 10;
Parallel.For(0, 10, i =>
{
    for (int j = 0; j < len; j++)
    {
        var test = PSReplayAnalysis.PSReplayAnalysis.ConvFile(files[i * len + j]);
        var cc = Regex.Replace(test, @"\S*?$", "");
        if (cc.Contains("Zoroark"))
            continue;
        //File.WriteAllText($"newReplay/{idx++}.sci", cc);
        BattleData a = PSReplayAnalysis.PSReplayAnalysis.Thonk(cc);
        if (a != null)
            batches.Add(a);

        if (j % 100 == 99) Console.WriteLine($"完成 {i * len} 的{j} / {len}场");
    }
});
//Console.WriteLine(batches.Count);
//for (int i = 0; i < files.Length; i += 10000)
//{
//    List<Task<BattleData?>> datas = new();
//    for (int j = 0; j < 1; ++j)
//    {
//        if (i * 1 + j < files.Length)
//        {
//            datas.Add(Task.Run(() =>
//            {
//                var test = PSReplayAnalysis.PSReplayAnalysis.ConvFile(files[i * 1 + j]);
//                var cc = Regex.Replace(test, @"\S*?$", "");
//                if (cc.Contains("Zoroark"))
//                    return null;
//                //File.WriteAllText($"newReplay/{idx++}.sci", cc);
//                BattleData a = PSReplayAnalysis.PSReplayAnalysis.Thonk(cc);
//                return a;
//            }));
//        }
//        foreach (var item in datas)
//        {
//            var dd = await item;
//            if (item != null)
//            {
//                batches.Add(dd);
//            }
//        }
//    }
//}
//foreach (var file in files)
////Parallel.ForEach(files, file =>
//{
//    var test = PSReplayAnalysis.PSReplayAnalysis.ConvFile(file);
//    var cc = Regex.Replace(test, @"\S*?$", "");
//    if (cc.Contains("Zoroark"))
//        continue;
//    //File.WriteAllText($"newReplay/{idx++}.sci", cc);
//    BattleData a = PSReplayAnalysis.PSReplayAnalysis.Thonk(cc);
//    if (a != null)
//        batches.Add(a);
//}
//);
//File.WriteAllText("test1w.json", JsonSerializer.Serialize(batches, new JsonSerializerOptions
//{
//    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
//    WriteIndented = true
//})); ;
File.WriteAllText("testdata.v3.json", JsonSerializer.Serialize(ExporttoTrainData.ExportBattleData(batches), new JsonSerializerOptions
{
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false
})); ;
return;

var pc1 = new PSClient("kirbyrbp", "11998whs").LogTo(Console.WriteLine);
await pc1.ConnectAsync();
await Task.Delay(1000);
PokeCommon.PokemonHome.PokemonHomeTools PokemonHomeTools = new();
await PokemonHomeTools.UpdateSVLastRankMatchAsync();

Console.WriteLine(await pc1.LoginAsync());
return;

var SWSHTools = PokeCommon.PokeMath.CalcUnits.SWSHTools;

int hp = SWSHTools.GetHP(95, 31, 0);
int ehb = SWSHTools.GetEVHP(hp, 95, 31);
int bhb = SWSHTools.GetPureBaseHP(hp, 31, 0);
int spe = SWSHTools.GetOtherStat(60, 31, 0);
int bspe = SWSHTools.GetPureBaseOtherStat(spe, 31, 0);
Console.WriteLine(hp);
Console.WriteLine(ehb);
Console.WriteLine(bhb);
Console.WriteLine(spe);
Console.WriteLine(bspe);
return;
// PokemonHomeTools PokemonHomeTools = new PokemonHomeTools();
// var data = await PokemonHomeTools.GetRankMatchAsync();
// var tdata = await PokemonHomeTools.GetTrainerDataAsync(data[0], -1);
// int aaa = 1;
// return;

var team1 = await PSConverter.ConvertToPokemonsAsync(@"Kyogre @ Mystic Water  
Ability: Drizzle  
Level: 50  
EVs: 124 HP / 156 SpA / 228 Spe  
Modest Nature  
IVs: 0 Atk  
- Water Spout  
- Origin Pulse  
- Ice Beam  
- Protect  

Zacian-Crowned @ Rusted Sword  
Ability: Intrepid Sword  
Level: 50  
EVs: 188 HP / 164 Atk / 4 Def / 4 SpD / 148 Spe  
Adamant Nature  
- Behemoth Blade  
- Sacred Sword  
- Play Rough  
- Protect  

Tornadus (M) @ Focus Sash  
Ability: Prankster  
Level: 50  
EVs: 4 HP / 252 SpA / 252 Spe  
Timid Nature  
IVs: 0 Atk  
- Hurricane  
- Icy Wind  
- Tailwind  
- Leer  

Landorus-Therian (M) @ Life Orb  
Ability: Intimidate  
Level: 50  
EVs: 36 HP / 212 Atk / 4 Def / 4 SpD / 252 Spe  
Jolly Nature  
- Rock Slide  
- Earthquake  
- Fly  
- Protect  

Kartana @ White Herb  
Ability: Beast Boost  
Level: 50  
EVs: 4 HP / 252 Atk / 252 Spe  
Jolly Nature  
- Leaf Blade  
- Sacred Sword  
- Smart Strike  
- Aerial Ace  

Amoonguss @ Coba Berry  
Ability: Regenerator  
Level: 50  
EVs: 236 HP / 156 Def / 116 SpD  
Relaxed Nature  
IVs: 0 Atk / 0 Spe  
- Pollen Puff  
- Rage Powder  
- Spore  
- Protect");


var pc = new PSClient("scixing", "11998whs").LogTo(Console.WriteLine);
await pc.ConnectAsync();
await Task.Delay(500);
Console.WriteLine(await pc.LoginAsync());
;



pc.ChallengeAction += async (player, rule) =>
{
    // if (rule == "gen8randombattle")
    //if (rule == "gen7vgc2019")
    if (rule == "gen8vgc2022")
    {
        await pc.ChatWithIdAsync(player, "随机战斗，玩了");
        await pc.ChatWithIdAsync(player, "就决定是你了");
        // await pc.ChangeYourTeamAsync("null");
        await pc.ChangeYourTeamAsync(await PSConverter.ConvertToPsOneLineAsync(team1));
        await pc.AcceptChallengeAsync(player);
    }
};
int id = 200;
while (true)
{
    await Task.Delay(10000000);
    await pc.GetRoomListAsync("gen8vgc2022", 1500);
    // await pc.SetAvatarAsync(id++.ToString());
}
return;

//SWSHBattleEngine engine = BattleEngine.CreateBattleEngine(BattleVersion.SWSH) as SWSHBattleEngine;
//engine.CreateBattle();
//PokemonHomeTools PokemonHomeTools = new PokemonHomeTools();
//var data =  await PokemonHomeTools.GetRankMatchAsync();
//var tdata = await PokemonHomeTools.GetTrainerDataAsync(data[0], 1);
//int aaa = 1;
//return;

//var a1 = OCRTools.SplitSWSHTeamPage("test10.jpg");
//List<string> res = new List<string>();
//foreach (var item in a1)
//{
//    //res.Add(OCRTools.GetText(item.PokeNameImgPath));
//    res.Add(OCRTools.GetText(item.MoveImgPath));
//}
//res.ForEach(item => Console.Write(item));
//return;

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
//Console.WriteLine(aa.Gmax);
//Console.WriteLine(aa.MetaPokemon.NameChs);
//Console.WriteLine(aa.LV);
//Console.WriteLine(aa.NickName);
//Console.WriteLine(aa.Ability.Name_Chs);
//Console.WriteLine(aa.Nature.Name_Chs);
//Console.WriteLine(aa.Item.Name_Chs);
//Console.WriteLine(string.Join(", ", aa.IVs.ToSixArray()));
//Console.WriteLine(string.Join(", ", aa.EVs.ToSixArray()));
Stopwatch stopWatch1 = new Stopwatch();
stopWatch1.Start();
for (int i = 0; i < 10000; i++)
{
    await PSConverter.ConvertToPsAsync(aa);
}
stopWatch1.Stop();
Console.WriteLine($"10000次输出ps文字耗时 {stopWatch1.ElapsedMilliseconds}ms");
Console.WriteLine(await PSConverter.ConvertToPsAsync(aa));

//foreach (var item in aa.Moves)
//{
//    Console.WriteLine(item.NameChs);
//}


var team = await PSConverter.ConvertToPokemonsAsync(@"鳄子 (Feraligatr) @ Life Orb
Ability: Sheer Force
Shiny: Yes
EVs: 30 HP / 204 Atk / 80 Def / 86 SpD / 108 Spe
Impish Nature
- Crunch
- Dragon Dance
- Ice Punch
- Waterfall

痞子 (Scrafty) @ Assault Vest
Ability: Moxie
Shiny: Yes
EVs: 8 HP / 210 Atk / 100 Def / 90 SpD / 100 Spe
Jolly Nature
IVs: 0 SpA
- Fake Out
- Drain Punch
- Crunch
- Poison Jab

吓死你 (Krookodile) @ Choice Scarf
Ability: Moxie
EVs: 252 Atk / 4 Def / 252 Spe
Adamant Nature
IVs: 0 SpA
- Earthquake
- Crunch
- Dragon Tail
- Facade

幕后黑手 (Tyranitar-Mega) @ Tyranitarite
Ability: Sand Stream
Shiny: Yes
EVs: 144 Atk / 120 Def / 120 SpD / 124 Spe
Adamant Nature
IVs: 0 SpA
- Dragon Dance
- Rock Slide
- Crunch
- Ice Punch

鼹鼠老大（ (Excadrill) @ Choice Band
Ability: Sand Rush
Shiny: Yes
EVs: 252 Atk / 4 Def / 252 Spe
Adamant Nature
- Iron Head
- Earthquake
- Brick Break
- Rock Slide

万金油 (Landorus-Therian) @ Rocky Helmet
Ability: Intimidate
Shiny: Yes
EVs: 8 HP / 120 Atk / 140 Def / 60 SpD / 180 Spe
Impish Nature
- Defog
- Stealth Rock
- Earthquake
- U-turn");

stopWatch1.Restart();
for (int i = 0; i < 10000; i++)
{
    await PSConverter.ConvertToPsAsync(team);
}
Console.WriteLine(await PSConverter.ConvertToPsAsync(team));
stopWatch1.Stop();
Console.WriteLine($"10000次输出ps队伍文字耗时 {stopWatch1.ElapsedMilliseconds}ms");

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

Console.WriteLine(PSConverter.ConvertToPsOneLineAsync(team));
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
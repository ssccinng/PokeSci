// See https://aka.ms/new-console-template for more information

using PokeCommon.PokemonShowdownTools;
using PokePSCore;

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

pc.OnTeampreview += async battle =>
{
    await battle.SendMessageAsync("让我康康");
    await battle.OrderTeamAsync("123456");
};

pc.OnForceSwitch += async (battle, bools) =>
{
    Console.WriteLine("让我康康你有没有触发");
    List<ChooseData> chooseDatas = new List<ChooseData>();
    for (int i = 0; i < bools.Length; i++)
    {
        if (bools[i])
        {
            int idx = -1;
            for (int j = 0; j < 4; j++)
            {
                if (battle.Actives[j] == false && !battle.MyTeam.GamePokemons[j].IsDead)
                {
                    idx = j;
                    battle.Actives[j] = true;
                    break;;
                }
            }
            if (idx == -1)
            {
                chooseDatas.Add(new SwitchData{IsPass = true});
            }
            else
            {
                chooseDatas.Add(new SwitchData{PokeId = idx + 1});

            }
        }
    }

    await battle.SendMoveAsunc(chooseDatas.ToArray());
};

pc.OnChooseMove += async battle =>
{
    List<ChooseData> chooseDatas = new List<ChooseData>();

    for (int i = 0; i < battle.ActiveStatus.Length; i++)
    {
        var target = battle.ActiveStatus[i].GetProperty("moves")[0].GetProperty("target").GetString();
        if (target == "any" || target == "normal")
        {
            chooseDatas.Add(new MoveChooseData(1){ Target = 1});

        }
        else
        {
            chooseDatas.Add(new MoveChooseData(1));

        }
    }
    await battle.SendMoveAsunc(chooseDatas.ToArray());
};
int id = 200;
while (true)
{
    await Task.Delay(10000000);
    await pc.GetRoomListAsync("gen8vgc2022", 1500);
    // await pc.SetAvatarAsync(id++.ToString());
}
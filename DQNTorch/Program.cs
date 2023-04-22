using DQNTorch;
using NumSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using TorchSharp;
using static TorchSharp.torch;

//var agent = new DQNAgent();
//agent.Env2 = new PokeDanEnvTest(@"F:\VSProject\PokeDanAI\testdata.v9.json");

//agent.train(1000);

//agent.model.save("dasd.data");
PokeDanLadder pokeDanLadder = new(32);
await pokeDanLadder.train(10000);
pokeDanLadder.SaveAll();

//await zQDQNAgent.train(1);
Console.ReadLine();


return;
//return;
var aaaa = np.random.rand(1)[0].GetDouble()
    ;
DQNAgent dQNAgent = new DQNAgent();
//dQNAgent.model.load("temp.1200.data");
//dQNAgent.target_model.load("temp.1200.data");
await dQNAgent.train1(10000);
dQNAgent.model.save("dani.dat");
var afaa= from_array(new[,] { 
    { 1, 2, 5, 4 },
    //{ 1, 2, 3, 4 },
});
//var cc = afaa.slice(1, 1, 3, 1);
var cc = argmax(afaa, 1);
for (int i = 0; i < cc.shape[0]; i++)
{
    Console.WriteLine(cc[i].ToInt32());
}
return;
PokemonDataAccess.PokemonContext pokemonContext = new PokemonDataAccess.PokemonContext("PokemonDataBase.db");


// pokemonContext.Database.Migrate();
Console.WriteLine(pokemonContext.Egg_Groups.ToArray().Length);

// 补充朱紫信息
//var abil = JsonDocument.Parse(File.ReadAllBytes("abilities.zqd")).RootElement;

//for (int i = 0; i < abil.GetArrayLength(); i++)
//{
//    if (abil[i].GetProperty("num").GetInt32() > 267)
//    {

//    }
//}
if (false)
{


    var ab = File.ReadAllLines("ab.zqd");
    foreach (var item in ab)
    {
        var data = item.Split('\t');
        pokemonContext.Abilities.Add(new PokemonDataAccess.Models.Ability
        {
            Name_Chs = data[1],
            Name_Eng = data[3],
            Name_Jpn = data[2],
            AbilityId = int.Parse(data[0]),
            description_Chs = data[4]
        });

    }
    pokemonContext.SaveChanges();
}

if (false)
{
    var mo = File.ReadAllLines("mo.zqd");

    int lent = pokemonContext.Moves.Count();
    foreach (var item in mo)
    {
        var data = item.Split('\t');
        int.TryParse(data[6], out int pow);
        int.TryParse(data[7], out int acc);

        pokemonContext.Moves.Add(new PokemonDataAccess.Models.Move
        {
            Name_Chs = data[1],
            Name_Eng = data[3],
            Name_Jpn = data[2],
            description_Chs = data[9].Trim(),
            MoveId = ++lent,
            MoveType = pokemonContext.PokeTypes.FirstOrDefault(s => s.Name_Chs == data[4]),
            Damage_Type = data[5],
            Pow = pow,
            Acc = acc,
            PP = int.Parse(data[8]),
        });
    }
    pokemonContext.SaveChanges();
}

if (false)
{
    int lent = pokemonContext.PSPokemons.Count();

    var ps = File.ReadAllLines("PsPokemons.csv");
    foreach (var p in ps)
    {
        var data = p[..].Replace("\"", "").Split(",");
        int.TryParse(data[5], out int pokeid);
        pokemonContext.PSPokemons.Add(new PokemonDataAccess.Models.PSPokemon
        {
            Id = int.Parse(data[0]),
            PSName = data[1],
            PSImgName = data[2],
            PSChsName = data[3],
            AllValue = int.Parse(data[4]),
            PokemonId = pokeid == 0 ? null : (int?)pokeid,
        });
    }
    pokemonContext.SaveChanges();
}

if (false)
{
    var pokes = JsonSerializer.Deserialize<List<PPoke>>(File.ReadAllText("Pokedata.zqd"));
    int lent = pokemonContext.Pokemons.ToArray
        ().Last().Id;
    int lent1 = pokemonContext.PSPokemons.ToArray().Last().Id;

    foreach (var poke in pokes)
    {
        if (poke.num > 898 || poke.name.Contains("Hishu") || poke.name.Contains("Paldea"))
        {
            var name = poke.baseSpecies == null ? poke.name : GetNormalName(poke.baseSpecies, poke.name);
            var newpoke = new PokemonDataAccess.Models.Pokemon
            {
                Id = ++lent,
                NameEng = poke.baseSpecies ?? poke.name,
                FormNameEng = name,
                Weight = (decimal)poke.weightkg,
                Height = (decimal)poke.heightm,
                Ability1 = pokemonContext.Abilities.FirstOrDefault(s => poke.abilities._0 == s.Name_Eng),
                Ability2 = pokemonContext.Abilities.FirstOrDefault(s => poke.abilities._1 == s.Name_Eng),
                AbilityH = pokemonContext.Abilities.FirstOrDefault(s => poke.abilities.H == s.Name_Eng),
                BaseAtk = poke.baseStats.atk,
                BaseDef = poke.baseStats.def,
                BaseHP = poke.baseStats.hp,
                BaseSpa = poke.baseStats.spa,
                BaseSpd = poke.baseStats.spd,
                BaseSpe = poke.baseStats.spe,
                DexId = poke.num,
                PokeFormId = 0, 
                 
                
                 
                  
            };
            if (poke.formeOrder != null)
            {
                newpoke.PokeFormId = Array.IndexOf(poke.formeOrder, poke.name);
             
            }
            if (newpoke.Ability2 == null)
            {
                newpoke.Ability2 = newpoke.Ability1;
            }
            if (newpoke.AbilityH == null)
            {
                newpoke.AbilityH = newpoke.Ability1;
            }
            newpoke.Type1 = pokemonContext.PokeTypes.FirstOrDefault(s => s.Name_Eng == poke.types[0]);

            if (poke.types.Length == 2)
            {
                newpoke.Type2 = pokemonContext.PokeTypes.FirstOrDefault(s => s.Name_Eng == poke.types[1]);
            }
            newpoke.EggGroup1 = pokemonContext.Egg_Groups.FirstOrDefault(s => s.Name_Eng == poke.eggGroups[0]);
            if (poke.eggGroups.Length == 2)
            {
                newpoke.EggGroup2 = pokemonContext.Egg_Groups.FirstOrDefault(s => s.Name_Eng == poke.eggGroups[1]);
            }
            pokemonContext.Pokemons.Add(newpoke);
            pokemonContext.PSPokemons.Add(new PokemonDataAccess.Models.PSPokemon
            {
                PokemonId = newpoke.Id,
                PSName = poke.name,
                AllValue = newpoke.Base_Value.Sum(),
                Id = ++lent1
            });
            System.Console.WriteLine(poke.baseSpecies + " " + poke.name);
        }

    }
    pokemonContext.SaveChanges();
}
if (false) {
    var it = File.ReadAllLines("it.zqd");
    int lent = pokemonContext.Items.Count();
    foreach (var item in it)
    {
        var data = item.Split('\t');
        pokemonContext.Items.Add(new PokemonDataAccess.Models.Item
        {
            Name_Chs = data[1],
            Name_Jpn = data[2],
            Name_Eng = data[3],
            description_Chs = data[4].Trim(),
            ItemId = ++lent,
             
            })
            ;
            }
            pokemonContext.SaveChanges();

}

return;


//var agent = new DQNAgent(new PokeDanEnvTest(@"F:\VSProject\PokeDanAI\testdata.v9.json"));

//agent.train(1000);

//agent.model.save("dasd.data");



string GetNormalName(string baseSpecies, string name)
{
    return name[baseSpecies.Length..].Replace("-", " ").Trim();
}
public class PPoke
{
    public int num { get; set; }
    public string name { get; set; }
    public string[] types { get; set; }
    public string baseSpecies { get; set; }
    public Basestats baseStats { get; set; }
    public Abilities abilities { get; set; }
    public float heightm { get; set; }
    public float weightkg { get; set; }
    public string color { get; set; }
    public string prevo { get; set; }
    public string evoType { get; set; }
    public string evoCondition { get; set; }
    public string[] eggGroups { get; set; }
    public string[] formeOrder { get; set; }
}

public class Basestats
{
    public int hp { get; set; }
    public int atk { get; set; }
    public int def { get; set; }
    public int spa { get; set; }
    public int spd { get; set; }
    public int spe { get; set; }
}

public class Abilities
{
    [JsonPropertyName("0")]
    public string _0 { get; set; }
    [JsonPropertyName("1")]
    public string _1 { get; set; }
    public string H { get; set; }
}

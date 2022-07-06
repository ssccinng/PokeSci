using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.VisualBasic.CompilerServices;
using PokeCommon.Models;
using PokeCommon.Utils;

namespace PokePSCore;

public enum PlayerPos
{
    [Display(Name = "p1")]
    Player1,
    [Display(Name = "p2")]
    Player2,
}

public class PsBattle
{
    public PSClient Client;


    /// <summary>
    /// 房间Id
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 我是p1 还是p2
    /// </summary>
    public PlayerPos PlayerPos { get; set; }
    /// <summary>
    /// 玩家1名字
    /// </summary>
    public string Player1 { get; set; }
    public GamePokemonTeam GamePokemonTeam1 { get; set; } = new GamePokemonTeam();
    /// <summary>
    /// 玩家2名字
    /// </summary>
    public string Player2 { get; set; }
    public GamePokemonTeam GamePokemonTeam2 { get; set; } = new GamePokemonTeam();

    /// <summary>
    /// 暂时的 以后需要整合到对战队伍中去
    /// </summary>
    public bool[] Actives { get; set; } = new bool[6];

    public JsonElement[] ActiveStatus = new JsonElement[2];
    public GamePokemonTeam OppTeam => PlayerPos == PlayerPos.Player1 ? GamePokemonTeam2 : GamePokemonTeam1;
    public GamePokemonTeam MyTeam => PlayerPos == PlayerPos.Player1 ? GamePokemonTeam1 : GamePokemonTeam2;

    /// <summary>
    /// 内部回合数，用于发送命令
    /// </summary>
    public int Turn { get; set; } = 0;
    public int idx = 1;

    
    
    public PsBattle(PSClient client, string tag)
    {
        Client = client;
        Tag = tag;
        MyTeam.GamePokemons = new List<GamePokemon>() { null, null, null, null, null, null };
        OppTeam.GamePokemons = new List<GamePokemon>() { null, null, null, null, null, null };
    }

    public async Task OrderTeamAsync(string order)
    {
        await Client.SendTeamOrderAsync(Tag, order, Turn);
    }

    public async Task SendMoveAsunc(ChooseData[] chooseDatas)
    {
        await Client.SendMoveAsync(Tag, Turn, chooseDatas);
    }
    public void UpdateOppTeam(string name, int lv = 50)
    {
        //OppTeam.GamePokemons.Any(s => s.MetaPokemon.)
        // 要getps的id
    }

    public async Task SendMessageAsync(string message)
    {
        await Client.SendAsync(Tag, message);
    }
    public async Task RefreshByRequestAsync(string request)
    {
        var data = JsonDocument.Parse(request).RootElement;

        if (data.TryGetProperty("rqid", out var jsonId))
        {
            Turn = jsonId.GetInt32();
            // Console.WriteLine(Turn);
        }
        if (data.TryGetProperty("forceSwitch", out var jsongFSwitch))
        {
            bool[] fArray;
            if (jsongFSwitch.ValueKind == JsonValueKind.Array)
            {
                fArray = new bool[jsongFSwitch.GetArrayLength()];
                for (int i = 0; i < jsongFSwitch.GetArrayLength(); i++)
                {
                    fArray[i] = jsongFSwitch[i].GetBoolean();
                }
                Console.WriteLine(fArray.Length);

            }
            else
            {
                fArray = new bool[] { jsongFSwitch.GetBoolean() };
            }
            // 可能不是array
            Console.WriteLine("检测到需要换人");
            // 需要换人
            Client.OnForceSwitch?.Invoke(this, fArray);
        }
        if (data.TryGetProperty("side", out var side))
        {
            Console.WriteLine("side");
            var pokes = side.GetProperty("pokemon");
            Console.WriteLine(pokes.GetArrayLength());
            for (int i = 0; i < pokes.GetArrayLength(); i++)
            {
                Console.WriteLine(pokes[i]);
                var detail = pokes[i].GetProperty("details").GetString().Split(", ");
                // var poke = MyTeam.GamePokemons.FirstOrDefault(s =>
                //     PokemonTools.GetPsPokemonAsync(s.MetaPokemon.Id).Result?.PSName == detail[0]);
                string[] condition = pokes[i].GetProperty("condition").GetString().Split('/');
                int hp = 0;
                if (condition.Length > 1)
                {
                    hp = int.Parse(condition[0]);
                }
                bool pokeActive = pokes[i].GetProperty("active").GetBoolean();
                Actives[i] = pokeActive;
                MyTeam.GamePokemons[i] = (new GamePokemon(await PokemonTools.GetPokemonFromPsNameAsync(detail[0])));
                MyTeam.GamePokemons[i].NowHp = hp;
                // if (poke != null)
                // {
                //     poke.NowHp = hp;
                //     // 更新数据 
                // }
                // else
                // {
                //     MyTeam.GamePokemons.Add(new GamePokemon(await PokemonTools.GetPokemonFromPsNameAsync(detail[0])));
                //
                // }

            }
        }
        if (data.TryGetProperty("active", out var active))
        {
            for (int i = 0; i < active.GetArrayLength(); i++)
            {
                ActiveStatus[i] = active[i];
            }
            // 更新技能信息
        }
    }
}
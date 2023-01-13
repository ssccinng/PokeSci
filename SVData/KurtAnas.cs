using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PokemonDataAccess.Models;
using SVData.Models;
using PokeCommon.Utils;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace SVData;
internal class KurtAnas
{
    public static void Run(string data)
    {

        var dada = File.Open("pokedatasv1", FileMode.Create);


        var moves = JsonSerializer.Deserialize<List<Move>>(File.ReadAllBytes("moves.json"));
        var names = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("monsname.json"));
        var tokuseis = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("tokusei.json"));
        var Forms = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("zkn_form.json"));
        JsonNode jsonObj = JsonNode.Parse(File.ReadAllBytes("idname.json"));

        var engnames = names.Select(s => s.Name_Eng).ToList();
        var tosnames = tokuseis.Select(s => s.Name_Eng).ToList();
        var tosnames1 = tokuseis.Select(s => s.Name_Chs).ToList();


        var map = moves.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var pokenamemap = names.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var tokuseismap = tokuseis.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);

        var pokes = Regex.Split(data, @"\n\s*?\n");

        Dictionary<string, HashSet<string>> tt = new();

        foreach (var p in tosnames1)
        {
            tt.TryAdd(p, new());
        }
        foreach (var pokedata in pokes)
        {

            StringBuilder sb = new StringBuilder();

            var poke1 = pokedata.Trim().Split('\n');

            string[] line = poke1[1].Split(" - ");
            var namedata = line[1].Split(' ');
            var name = namedata[0];
            if (namedata[1][0] == '#' || namedata[1][0] == '(')
            {
            }
            else
            {
                name = namedata[0] + ' ' + namedata[1];

            }
            Console.WriteLine("name = {0}", name);
            int.TryParse(name.Split('-').Last(), out int formId);
            if (formId != 0)
            {
                name = name[..name.IndexOf('-')];
            }
            int dexid = engnames.IndexOf(name) + 1;
            var formname = jsonObj[dexid.ToString("000")][formId.ToString("000")];
            Console.WriteLine("formname: {0}", formname);

            sb.Append(pokenamemap[name]);


            if (formname.GetValue<string>() != "")
                sb.Append($"-{formname}");
            sb.Append(',');

            if (namedata.Length == 4)
            {

                Console.WriteLine("Dex = {0}", namedata[1]);
                Console.WriteLine("Total = {0}", namedata[3]);
            }
            else
            {
                Console.WriteLine("Total = {0}", namedata[2]);

            }
            string basedata = null;
            string type = null;
            string[] abli = null;
            string ev = null;
            string[] eggroup = null;
            for (int i = 3; i < poke1.Length; i++)
            {
                if (poke1[i].StartsWith("Base Stats"))
                {
                    basedata = poke1[i][12..].Split(' ')[0].Trim();
                }
                else if (poke1[i].StartsWith("Type"))
                {
                    type = poke1[i][6..].Trim();
                }
                else if (poke1[i].StartsWith("Abilities"))
                {
                    abli = poke1[i][11..].Split(" | ");

                }
                else if (poke1[i].StartsWith("EV Yield"))
                {
                    ev = poke1[i][10..].Trim();
                }
                else if (poke1[i].StartsWith("Egg Group"))
                {
                    eggroup = poke1[i][10..].Trim().Split(" / ");
                }
            }
            foreach (var item in abli.Select(s => tokuseismap[s[..^4].Trim()]))
            {
                tt[item].Add(pokenamemap[name]);

            }


            Console.WriteLine("bv = {0}", basedata);
            var aa = type.Split(" / ")
                .Select(s => PokemonTools.GetTypeAsync(s.Trim()).Result.Id);
            if (aa.Count() < 2) aa = aa.Append(0);
            sb.Append(string.Join(',', aa));
            sb.Append(',');
            sb.Append(basedata.Replace('.', ','));
            sb.Append(',');
            sb.Append(string.Join(',', abli.Select(s => tosnames.IndexOf(s[..^4].Trim()))));
            sb.Append(',');
            sb.Append(dexid);
            sb.Append(',');
            sb.Append(ev);
            sb.Append(',');
            sb.Append(string.Join(" / ", eggroup));

            dada.Write(Encoding.UTF8.GetBytes(sb.ToString()));
            dada.Write("\n"u8);

            Console.WriteLine(sb.ToString());



        }

        //foreach (var item in tt)
        //{
        //    Console.WriteLine($"{item.Key},{string.Join(',', item.Value)}");
        //}

        dada.Close();
    }



    public static void RunLearnset(string data)
    {
        var dada = File.Open("pokedatasvMove", FileMode.Create);

        var EvolutionDatass = 
            
            JsonSerializer.Deserialize<Dictionary<string, List<EvolutionData>>>(File.ReadAllBytes("Evo.json"));


        var moves = JsonSerializer.Deserialize<List<Move>>(File.ReadAllBytes("moves.json"));
        var names = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("monsname.json"));
        var tokuseis = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("tokusei.json"));
        var Forms = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("zkn_form.json"));
        JsonNode jsonObj = JsonNode.Parse(File.ReadAllBytes("idname.json"));

        var engnames = names.Select(s => s.Name_Eng).ToList();
        var tosnames = tokuseis.Select(s => s.Name_Eng).ToList();
        var tosnames1 = tokuseis.Select(s => s.Name_Chs).ToList();


        var map = moves.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var pokenamemap = names.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var tokuseismap = tokuseis.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);

        var pokes = Regex.Split(data, @"\n\s*?\n");

        Dictionary<string, PokeWithMove> le = new();

        foreach (var pokedata in pokes)
        {
            PokeWithMove pokeWithMove = new PokeWithMove();
            StringBuilder sb = new StringBuilder();

            var poke1 = (pokedata.Trim() + "\nend").Split('\n');

            string[] line = poke1[1].Split(" - ");
            var namedata = line[1].Split(' ');
            var name = namedata[0];
            if (namedata[1][0] == '#' || namedata[1][0] == '(')
            {
            }
            else
            {
                name = namedata[0] + ' ' + namedata[1];

            }
            Console.WriteLine("name = {0}", name);
            int.TryParse(name.Split('-').Last(), out int formId);
            if (formId != 0)
            {
                name = name[..name.IndexOf('-')];
            }
            int dexid = engnames.IndexOf(name) + 1;
            var formname = jsonObj[dexid.ToString("000")][formId.ToString("000")];
            Console.WriteLine("formname: {0}", formname);

            sb.Append(pokenamemap[name]);
             

            if (formname.GetValue<string>() != "")
                sb.Append($"-{formname}");
            pokeWithMove.Name = sb.ToString();
            sb.Append(',');


            if (namedata.Length == 4)
            {

                Console.WriteLine("Dex = {0}", namedata[1]);
                Console.WriteLine("Total = {0}", namedata[3]);
            }
            else
            {
                Console.WriteLine("Total = {0}", namedata[2]);

            }
            string basedata = null;
            string type = null;
            string[] abli = null;
            string ev = null;
            string[] eggroup = null;
            for (int i = 3; i < poke1.Length; i++)
            {
                if (poke1[i].StartsWith("Level Up Moves"))
                {
                    while (poke1[++i][0] == '-')
                    {
                        var match = Regex.Match(poke1[i].Trim(), @"- \[(\d+?)\] (.+)");
                        pokeWithMove.LearnMoves.Add((match.Groups[2].Value, match.Groups[1].Value));

                    }
                    --i;
                }
                else if (poke1[i].StartsWith("TM Learn"))
                {
                    while (poke1[++i][0] == '-')
                    {
                        var match = Regex.Match(poke1[i].Trim(), @"- \[TM(\d+?)\] (.+)");
                        pokeWithMove.TMMoves.Add((match.Groups[2].Value, match.Groups[1].Value));
                    }
                    --i;
                }
                else if (poke1[i].StartsWith("Egg Moves"))
                {
 
                    while (poke1[++i][0] == '-')
                    {
                        pokeWithMove.EggMoves.Add((poke1[i][2..].Trim(), ""));
                    }
                    --i;

                }
            }

            le[$"{dexid}+{formId}"] = pokeWithMove;

            Console.WriteLine("bv = {0}", basedata);
            //var aa = type.Split(" / ")
            //    .Select(s => PokemonTools.GetTypeAsync(s.Trim()).Result.Id);
            //if (aa.Count() < 2) aa = aa.Append(0);

            //Console.WriteLine("Lv Move:");
            //Console.WriteLine(string.Join('\n', pokeWithMove.LearnMoves));
            //Console.WriteLine("TM Move:");
            //Console.WriteLine(string.Join('\n', pokeWithMove.TMMoves));
            //Console.WriteLine("EGG Move:");
            //Console.WriteLine(string.Join('\n', pokeWithMove.EggMoves));
            //Console.WriteLine(sb.ToString());



        }
        foreach (var ( key, val ) in le)
        {
            var dff = val.GetMoveSet();
            foreach (var item in EvolutionDatass[key])
            {
                //var evokey = jsonObj[item.DexId.ToString("000")][item.Form.ToString("000")].GetValue<string>();
                var evokey = item.DexId + "+" + item.Form;

                var set = le[evokey].GetMoveSet();

                le[evokey].OtherMoves = dff.Except(set).ToHashSet();
                Console.WriteLine(item.DexId);
                Console.WriteLine(item.Form);
                Console.WriteLine( );

                Console.WriteLine(item.Species);
                Console.WriteLine();


            }
        }
        foreach (var (key, val) in le)
        {
            var dff = val.GetMoveSet();
            foreach (var item in EvolutionDatass[key])
            {
                //var evokey = jsonObj[item.DexId.ToString("000")][item.Form.ToString("000")].GetValue<string>();
                var evokey = item.DexId + "+" + item.Form;

                var set = le[evokey].GetMoveSet();

                foreach (var item11 in dff.Except(set).ToHashSet())
                {
                    le[evokey].OtherMoves.Add(item11);
                } ;
                Console.WriteLine(item.DexId);
                Console.WriteLine(item.Form);
                Console.WriteLine();

                Console.WriteLine(item.Species);
                Console.WriteLine();


            }
        }

        foreach (var (key, val) in le)
        {
            StringBuilder stringBuilder = new StringBuilder();


            Console.WriteLine(val.Name);
            stringBuilder.Append(val.Name);
            foreach (var move in val.LearnMoves)
            {
                stringBuilder.Append(',');
                stringBuilder.Append($"{map[move.Item1]}-lv{int.Parse(move.Item2).ToString("00")}");

                //Console.WriteLine();
            }
            foreach (var move in val.TMMoves)
            {
                stringBuilder.Append(',');
                stringBuilder.Append($"{map[move.Item1]}-TM{int.Parse(move.Item2).ToString("00")}");
                //Console.WriteLine($"{map[move.Item1]}-TM{int.Parse(move.Item2).ToString("00")}");
            }
            foreach (var move in val.EggMoves)
            {
                stringBuilder.Append(',');
                stringBuilder.Append($"{map[move.Item1]}-遗传");
                //Console.WriteLine($"{map[move.Item1]}-遗传");
            }

            foreach (var move in val.OtherMoves)
            {
                stringBuilder.Append(',');
                stringBuilder.Append($"{map[move]}-进化前");
                //Console.WriteLine($"{map[move]}-进化前");
            }

            dada.Write(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            dada.Write("\n"u8);
        }
        dada.Close();
    }


    public static void RunImgPath(string data)
    {

        var imgs = Directory.GetFiles("E:\\PokemonSVData\\pm_big");
        var EvolutionDatass =

            JsonSerializer.Deserialize<Dictionary<string, List<EvolutionData>>>(File.ReadAllBytes("Evo.json"));


        var moves = JsonSerializer.Deserialize<List<Move>>(File.ReadAllBytes("moves.json"));
        var names = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("monsname.json"));
        var tokuseis = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("tokusei.json"));
        var Forms = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("zkn_form.json"));
        JsonNode jsonObj = JsonNode.Parse(File.ReadAllBytes("idname.json"));

        var engnames = names.Select(s => s.Name_Eng).ToList();
        var tosnames = tokuseis.Select(s => s.Name_Eng).ToList();
        var tosnames1 = tokuseis.Select(s => s.Name_Chs).ToList();


        var map = moves.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var pokenamemap = names.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
        var tokuseismap = tokuseis.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);

        var pokes = Regex.Split(data, @"\n\s*?\n");

        Dictionary<string, PokeWithMove> le = new();

        foreach (var pokedata in pokes)
        {
            PokeWithMove pokeWithMove = new PokeWithMove();
            StringBuilder sb = new StringBuilder();

            var poke1 = (pokedata.Trim() + "\nend").Split('\n');

            string[] line = poke1[1].Split(" - ");
            var namedata = line[1].Split(' ');
            var name = namedata[0];
            if (namedata[1][0] == '#' || namedata[1][0] == '(')
            {
            }
            else
            {
                name = namedata[0] + ' ' + namedata[1];

            }
            Console.WriteLine("name = {0}", name);
            int.TryParse(name.Split('-').Last(), out int formId);
            if (formId != 0)
            {
                name = name[..name.IndexOf('-')];
            }
            int dexid = engnames.IndexOf(name) + 1;
            var formname = jsonObj[dexid.ToString("000")][formId.ToString("000")];
            Console.WriteLine("formname: {0}", formname);

            sb.Append(pokenamemap[name]);


            if (formname.GetValue<string>() != "")
                sb.Append($"-{formname}");
            pokeWithMove.Name = sb.ToString();
            sb.Append(',');


            if (namedata.Length == 4)
            {

                Console.WriteLine("Dex = {0}", namedata[1]);
                Console.WriteLine("Total = {0}", namedata[3]);
            }
            else
            {
                Console.WriteLine("Total = {0}", namedata[2]);

            }
            string basedata = null;
            string type = null;
            string[] abli = null;
            string ev = null;
            string[] eggroup = null;

            le[$"{dexid}+{formId}"] = pokeWithMove;

            Console.WriteLine("bv = {0}", basedata);




        }
        int[] daaaa = new int[1500];
        var lea = le.Keys.Select(s => s.Split("+")[0]).Distinct().ToList();

        int idx = -1;
        int lastidx = -11;
        for (int i = 1; i < imgs.Length; i++)
        {
            var name = imgs[i].Split("\\").Last().Trim() ;

            var res = Regex.Match(name, @"pm(\d+)_(\d+)_(\d+)_(\d+)_big.png").Groups;
            var dexid = int.Parse(res[1].Value);
            var daniid = res[2].Value; // 若是01 忽略
            var veid = res[3].Value;
            var hoho = res[4].Value;
            if (lastidx != dexid) idx++;
            lastidx = dexid;
            if (dexid == 704) idx += 12;
            if (daniid == "01") continue;
            while (!le.ContainsKey($"{lea[idx]}+{daaaa[dexid]}"))
            {
                daaaa[dexid]++;
            }
            try
            {
                File.Move(imgs[i], imgs[i][..^name.Length] + le[$"{lea[idx]}+{daaaa[dexid]++}"].Name + ".png");

            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

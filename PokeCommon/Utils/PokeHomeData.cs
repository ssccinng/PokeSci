using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeCommon.Utils
{
    public static class PokeHomeData
    {
        public static Dictionary<int, PokeModel> WazaTable = LoadResourceToString<Dictionary<int, PokeModel>>("Data.Home.wazaTable.json");
        public static Dictionary<int, PokeModel> TokuseiTable = LoadResourceToString<Dictionary<int, PokeModel>>("Data.Home.tokuseiTable.json");
        public static Dictionary<int, PokeModel> ItemTable = LoadResourceToString<Dictionary<int, PokeModel>>("Data.Home.itemTable.json");
        public static List<PokeModel> SeikakuNames = LoadResourceToString<List<PokeModel>>("Data.Home.seikakunameall.json");
        static T LoadResourceToString<T>(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var full_path = $"{assembly.FullName.Split(',')[0]}.{path}";
            var rs = assembly.GetManifestResourceStream(full_path);
            return JsonSerializer.Deserialize<T>(rs);
            //var ss = new StreamReader(rs); var str = ss.ReadToEnd(); return str;
        }
    }
}

// See https://aka.ms/new-console-template for more information


using PSStatClawer;
using System.Collections.Generic;
using System.Text.Json;

//var aa = await Utils.GetDates();

//foreach (var item in aa)
//{
//    Console.WriteLine(item);
//}

//var chaos = await Utils.GetChaos("2024-08", "gen9vgc2024reghbo3");
JsonDocument doc = JsonDocument.Parse(File.ReadAllBytes("chaos.json"));
// 完美字典序列化
var aa1 = JsonSerializer.Deserialize<Dictionary<string, ItemProbability>>(doc.RootElement.GetProperty("data"));

//var cc = aa1.OrderByDescending(s => s.Value["usage"].GetDouble()).Select(s => (s.Key, s.Value)).ToList();

Console.WriteLine(JsonSerializer.Serialize(aa1.First(), new JsonSerializerOptions
{
    WriteIndented = true
}));


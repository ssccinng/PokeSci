// See https://aka.ms/new-console-template for more information
using System.Text.Json;

var list = await RK9Tool.RK9Client.GetMatchPairings("IND02mCwIuhUMs3NdM7W");


File.WriteAllText("parings.json", JsonSerializer.Serialize(list));
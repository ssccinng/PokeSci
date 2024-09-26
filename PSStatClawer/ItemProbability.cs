// See https://aka.ms/new-console-template for more information


using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ItemProbability
{
    public Dictionary<string, double> Items { get; set; }
    [JsonPropertyName("Raw count")]
    public int RawCount { get; set; }
    public Dictionary<string, double> Spreads { get; set; }
    [JsonPropertyName("Tera Types")]
    public Dictionary<string, double> TeraTypes { get; set; }

    [JsonPropertyName("Checks and Counters")]
    public Dictionary<string, double[]> ChecksAndCounters { get; set; }
    public Dictionary<string, double> Teammates { get; set; }

    /// <summary>
    /// 使用人数，最高gxe，99%gxe, 95%gxe
    /// </summary>
    [JsonPropertyName("Viability Ceiling")]
    public int[] ViabilityCeiling { get; set; }
    public double usage { get; set; }
    public Dictionary<string, double> Abilities { get; set; }
    public Dictionary<string, double> Moves { get; set; }
    public Dictionary<string, double> Happiness { get; set; }

  
}


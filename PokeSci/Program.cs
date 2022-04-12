//Double CPUtprt = 0;
//System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(@"root\WMI", "Select * From MSAcpi_ThermalZoneTemperature");
//foreach (System.Management.ManagementObject mo in mos.Get())
//{
//    CPUtprt = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
//    Console.WriteLine("CPU temp : " + CPUtprt.ToString() + " °C");
//}


using PokeMath;
using PokeBattleEngine;
using PokeBattleEngine.BattleEngines;

BattleEngine.CreateBattleEngine(BattleVersion.DPPt);

SWSHTools SWSHTools = new SWSHTools();

int hp = SWSHTools.GetHP(95, 31, 0);
int bhb = SWSHTools.GetPureBaseHP(hp, 31, 0);
int spe = SWSHTools.GetOtherStat(60, 31, 0);
int bspe = SWSHTools.GetPureBaseOtherStat(spe, 31, 0);
Console.WriteLine(hp);
Console.WriteLine(bhb);
Console.WriteLine(spe);
Console.WriteLine(bspe);
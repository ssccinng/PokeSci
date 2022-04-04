//Double CPUtprt = 0;
//System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(@"root\WMI", "Select * From MSAcpi_ThermalZoneTemperature");
//foreach (System.Management.ManagementObject mo in mos.Get())
//{
//    CPUtprt = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
//    Console.WriteLine("CPU temp : " + CPUtprt.ToString() + " °C");
//}


using PokeMath;

SWSHTools SWSHTools = new SWSHTools();

int hp = SWSHTools.GetHP(95, 31, 252);
int bhb = SWSHTools.GetPureBaseHP(hp, 31, 252);
Console.WriteLine(hp);
Console.WriteLine(bhb);
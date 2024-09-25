// See https://aka.ms/new-console-template for more information


using PSStatClawer;

var aa = await Utils.GetDates();

foreach (var item in aa)
{
    Console.WriteLine(item);
}
using DatabaseTool;
using PokeCommon.API.Data;

var options = CliOptions.Parse(args);
if (options.ShowHelp)
{
    Console.WriteLine(CliOptions.HelpText);
    return 0;
}

try
{
    var source = ShitLibDataSource.Load(options.ShitLibDataPath);
    await using var db = new PokeDBContext(options.ConnectionString);
    var service = new DatabaseMaintenanceService(db, source, options);

    return await service.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    if (options.Verbose)
    {
        Console.Error.WriteLine(ex);
    }

    return 1;
}

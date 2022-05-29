using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Util.Extensions.Logging;

var sc = new ServiceCollection();
var scp = new BreadcrumbScope
{
    { "key1", "val1" },
    { "key2", "val2" },
};
sc.AddLogging(l => l.AddBreadcrumbConsole(false, scp));
using var sp = sc.BuildServiceProvider();

var logger = sp.GetRequiredService<ILogger<Program>>();
logger.LogInformation("line1");
var ds = new BreadcrumbScope {
    { "key3", "val3" },
};
using (var scope = logger.BeginScope(ds))
{
    logger.LogInformation("line2");
    ds.Add("key3", "key3updated");
    ds.Add("key4", "val4");
    logger.LogInformation("line3");
}

logger.LogInformation("line4");

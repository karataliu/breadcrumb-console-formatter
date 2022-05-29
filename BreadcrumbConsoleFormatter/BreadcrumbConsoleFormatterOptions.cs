using Microsoft.Extensions.Logging.Console;

namespace Util.Extensions.Logging
{

    public sealed class BreadcrumbConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public BreadcrumbScope? StaticScope { get; set; }
        public bool Simple { get; set; }
    }
}

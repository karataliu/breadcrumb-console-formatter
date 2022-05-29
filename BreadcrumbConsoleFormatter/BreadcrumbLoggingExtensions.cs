using Microsoft.Extensions.Logging;

namespace Util.Extensions.Logging
{
    public static class BreadcrumbLoggingExtensions
    {
        public static ILoggingBuilder AddBreadcrumbConsole(
            this ILoggingBuilder builder, bool simple, BreadcrumbScope? staticScope)
            => builder.AddConsole(options => options.FormatterName = nameof(BreadcrumbConsoleFormatter))
                .AddConsoleFormatter<BreadcrumbConsoleFormatter, BreadcrumbConsoleFormatterOptions>(
                    o =>
                    {
                        o.StaticScope = staticScope;
                        o.Simple = simple;
                    }
                );
    }
}

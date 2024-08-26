using Serilog;

namespace GoodDayWebApp.Logging;

public static class ServiceExtensionsForLogging
{
  public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder)
  {
    Environment.SetEnvironmentVariable("GoodDayWebApp_LOG_DIR", AppDomain.CurrentDomain.BaseDirectory);

    Serilog.Core.Logger logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    _ = builder.Host.UseSerilog(logger);

    return builder;
  }
}
